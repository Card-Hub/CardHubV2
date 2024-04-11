using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.Common.LyssiePlayerOrder;
using WebApi.Hubs;
using WebApi.Models;
using Timer = System.Timers.Timer;
using WebApi.Hubs.HubMessengers;
using Newtonsoft.Json;


namespace WebApi.GameLogic.LyssieUno;

public class UnoGameModLyssie
{
  // Cards
    private IDeckLyssie<UnoCardModLyssie>? _deck;
    public Stack<UnoCardModLyssie> _discardPile { get; private set; } = new();
    // Players
    private LyssiePlayerOrder _playerOrder = new();
    private Dictionary<string, UnoPlayerLyssie> _players = new();
    public string GameboardConnStr { get; set; }

    // Timer
    private TimeSpan _moveTimeLimit;
    private Timer _moveTimer;
    private readonly object _lock = new();

    // Game state
    public bool GameStarted {get; private set;} 
    private UnoColorLyssie _CurrentColor;
    public bool SomeoneNeedsToSelectWildColor { get; private set; }

    // Misc
    private readonly UnoDeckBuilderLyssie _deckBuilder;
    private UnoSettingsLyssie Settings = new();
    private UnoJsonStateLyssie JsonState= new();
    private iUnoMessenger _messenger;

    // constructor
    public UnoGameModLyssie(iUnoMessenger messenger, UnoDeckBuilderLyssie deckBuilder)
    {
        _deckBuilder = deckBuilder;

        _moveTimeLimit = TimeSpan.FromSeconds(5);
        _moveTimer = new Timer(_moveTimeLimit);
        _messenger = messenger;
        //_moveTimer.Elapsed += OnMoveTimeElapsed;
        //_hubContext = hubContext;
        
    }

    public void ChangeSettings(UnoSettingsLyssie settings) {
      Settings = settings;
    }

    public async Task StartGame()
    {
      // Build deck, shuffle
      _deck = _deckBuilder.Build(Settings);
      _deck.Shuffle();
      _playerOrder.ShufflePlayers();

      foreach (var player in _playerOrder.GetPlayers(LyssiePlayerStatus.Active))
      {
          var drawnCards = _deck.Draw(7);
          _players[player].AddCards(drawnCards);
      }

      var counter = 0;
      while (counter++ < 10)
      {
          var topCard = _deck.Draw();
          if (topCard.ColorEnum == UnoColorLyssie.Black) continue;
          _discardPile.Push(topCard);
          break;
      }
        //await _hubContext.Clients.Client(Gameboard).SendAsync("ReceiveCards", _discardPile);
        //await InitiateTurn();
    }

    public bool AddPlayer(string playerName, string connectionString)
    {
        if (!_players.ContainsKey(connectionString)) {
          _playerOrder.AddPlayer(connectionString);
          _players[connectionString] = new UnoPlayerLyssie(playerName, connectionString);
          return true;
        }
        return false;
    }

    public async Task<bool> PlayCard(string connStr, UnoCardModLyssie card)
    {
        if (_playerOrder.GetCurrentPlayer() != connStr || SomeoneNeedsToSelectWildColor) return false;
        
        bool cardCanBePlayed = CanCardBePlayed(card);

        if (!cardCanBePlayed) return false;

        _players[_playerOrder.GetCurrentPlayer()].RemoveCard(card);
        _discardPile.Push(card);
        _CurrentColor = card.ColorEnum;
        
        Console.WriteLine($"{connStr} played a card: {JsonConvert.SerializeObject(card)}");

        
        await CancelTimer(card);
        // did someone win?
        if (_players[_playerOrder.GetCurrentPlayer()].Hand.Count == 0) {
          await _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
          return true;
        }

        // handle wild
        var isWildPlayable = new List<UnoValueLyssie> {UnoValueLyssie.Wild, UnoValueLyssie.WildDrawFour}.Contains(card.ValueEnum);
        if (!isWildPlayable) {
          await InitiateTurn();
        }
        else {
          SomeoneNeedsToSelectWildColor = true;
        }
        return true;
    }

  public async Task SelectWild(string connStr, string color) {
    if (connStr == _playerOrder.GetCurrentPlayer() && SomeoneNeedsToSelectWildColor) {
      // change current color
      switch (color.ToLower()) {
        case "red":
          _CurrentColor = UnoColorLyssie.Red;
          break;
        case "green":
          _CurrentColor = UnoColorLyssie.Green;
          break;
        case "yellow":
          _CurrentColor = UnoColorLyssie.Yellow;
          break;
        case "blue":
          _CurrentColor = UnoColorLyssie.Blue;
          break;
        default:
          throw new ArgumentException("Invalid color selected");
      }
      SomeoneNeedsToSelectWildColor = false;
      await InitiateTurn();
    }
  }
  
    //public bool AllEqual<T>(params T[]? values)
    //{
    //    if (values is null || values.Length == 0) return false;
    //    return values.All(v => v.Equals(values[0]));
    //}

    //public UnoCardMod? DrawCard(string player)
    //{
    //    var currentPlayer = _playerOrder.GetPlayer(player);

    //    if (_playerOrder.Current() != player) return null;

    //    var card = _deck.Draw();
    //    _players[currentPlayer].AddCard(card);
    //    return card;
    //}

    //public List<UnoCardMod> GetPlayerHand(string playerName)
    //{
    //    var player = _playerOrder.GetPlayer(playerName);
    //    return _players[player].GetHand();
    //}

    //public void RemovePlayer(string playerName)
    //{
    //    _playerOrder.Remove(playerName);
    //}

    public async Task InitiateTurn()
    {
        lock (_lock)
        {
            _moveTimer.Start();
        }
        _playerOrder.NextTurn();
        var player = _playerOrder.GetCurrentPlayer();
        await _messenger.SendFrontendTimerSet(5);
        //await _hubContext.Clients.Clients(player, Gameboard).SendAsync("StartTimer", _moveTimeLimit.Seconds);

        Console.WriteLine("Timer started.");
    }

    public async Task CancelTimer(UnoCardModLyssie card)
    {
        lock (_lock)
        {
            _moveTimer.Stop();
            Console.WriteLine($"Timer cancelled by card {card.ValueEnum} {card.ColorEnum}.");
        }

        //await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("StartTimer", 0);
    }

    //private async void OnMoveTimeElapsed(object? source, ElapsedEventArgs e)
    //{
    //    Console.WriteLine("Turn elapsed at {0:mm:ss}", e.SignalTime);
    //    var drawnCard = _deck.Draw();
    //    var currentPlayer = _playerOrder.Current();
    //    _players[currentPlayer].AddCard(drawnCard);

    //    await _hubContext.Clients.Client(currentPlayer).SendAsync("ReceiveCard", "Gameboard", drawnCard);
    //    lock (_lock)
    //    {
    //        _moveTimer.Stop();
    //        Console.WriteLine($"Timer cancelled by timeout.");
    //    }

    //    _playerOrder.SetNextCurrent();
    //    await InitiateTurn();
    //}
    // Misc
    public bool SetAvatar(string connStr, string avatarStr) {
      if (_players.ContainsKey(connStr)) {
        _players[connStr].Avatar = avatarStr;
        return true;
      }
      else {
        Console.WriteLine("Can't set avatar {avatarStr} for {connStr}, they aren't a player");
        return false;
      }
    }
    // For json stuff
    public string GetGameState() {
      JsonState.Update(this);
      return JsonConvert.SerializeObject(JsonState, Formatting.Indented);
    }
    public List<UnoPlayerLyssie> GetActivePlayers() {
      List<UnoPlayerLyssie> activePlayers = new List<UnoPlayerLyssie>();
      foreach (string connStr in _playerOrder.GetPlayers(LyssiePlayerStatus.Active)) {
        activePlayers.Add(_players[connStr]);
      }
      return activePlayers;
    }
    public List<UnoPlayerLyssie> GetSpectators() {
      List<UnoPlayerLyssie> spectators = new List<UnoPlayerLyssie>();
      foreach (string connStr in _playerOrder.GetPlayers(LyssiePlayerStatus.Spectator)) {
        spectators.Add(_players[connStr]);
      }
      return spectators;
    }
    public string GetDirection() {
      if (GameStarted) {
        if (_playerOrder.DirectionInt == Common.LyssiePlayerOrder.Direction.Forward) {
          return "Clockwise";
        }
        else {
          return "Counterclockwise";
        }
      }
      else { return ""; }
    }
    public string GetCurrentPlayer() {
      if (GameStarted) {
        return _playerOrder.GetCurrentPlayer();
      }
      else { return ""; }
    }
    public string GetCurrentColor() {
      if (GameStarted) {
        return _CurrentColor.ToString();
      }
      else { return "";}
    }
    public string PlayerWhoHasUnoPrompt {
      get {
        if (SomeoneNeedsToSelectWildColor) {
          return _players[_playerOrder.GetCurrentPlayer()].Name;
        }
        else { return ""; }
      }
    }
    public int DeckCardCount {
      get {
        if (_deck is not null) {
          return _deck.GetCards().Count;
        }
        else {return 0;}
    }
    }
    private List<string> GetAllConnStrsIncGameboard() {
      List<string> allConnStrs = _playerOrder.GetAllPlayers();
      allConnStrs.Add(GameboardConnStr);
      return allConnStrs;
    }
    // public to be accessible to tests lol
    public bool CanCardBePlayed(UnoCardModLyssie card) {
      var lastCard = _discardPile.Peek();
      var isColorMatch = card.ColorEnum == lastCard.ColorEnum;
      var isValueMatch = card.ValueEnum == lastCard.ValueEnum;
      var isWildPlayable = new List<UnoValueLyssie> {UnoValueLyssie.Wild, UnoValueLyssie.WildDrawFour}.Contains(card.ValueEnum);
      return isColorMatch || isValueMatch || isWildPlayable;
    }
    
}