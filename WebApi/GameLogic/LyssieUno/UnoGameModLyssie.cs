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
  public Stack<UnoCardModLyssie> _discardPile { get; set; } = new();
  // Players
  private LyssiePlayerOrder _playerOrder = new();
  public LyssiePlayerOrder PlayerOrder {get { return _playerOrder;}}
  public Dictionary<string, UnoPlayerLyssie> _players {get; private set;} = new();
  public string GameboardConnStr { get; set; }

  // Timer
  private TimeSpan _moveTimeLimit;
  private Timer _moveTimer;
  private readonly object _lock = new();

  // Game state
  public bool GameStarted {get; private set;} 
  public UnoColorLyssie _CurrentColor {get;set;}
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
    //_playerOrder.ShufflePlayers();

    foreach (var player in _playerOrder.GetPlayers(LyssiePlayerStatus.Active))
    {
        var drawnCards = _deck.Draw(7);
        _players[player].AddCards(drawnCards);
    }

    var counter = 0;
    while (counter++ < 10)
    {
        var topCard = _deck.Draw();
        _discardPile.Push(topCard);
        if (topCard.ColorEnum == UnoColorLyssie.Black) continue;
        break;
    }
    _CurrentColor = _discardPile.Peek().ColorEnum;
    GameStarted = true;
      //await _hubContext.Clients.Client(Gameboard).SendAsync("ReceiveCards", _discardPile);
      //await InitiateTurn();
  }

  public bool AddPlayer(string playerName, string connStr)
  {
      _messenger.SendFrontendJson(new List<string>() {GameboardConnStr}, GetGameState());
      if (!_players.ContainsKey(connStr)) {
        _playerOrder.AddPlayer(connStr);
        _players[connStr] = new UnoPlayerLyssie(playerName, connStr);
        return true;
      }
      return false;
  }

  public async Task<bool> PlayCard(string connStr, UnoCardModLyssie card)
  {
    if (_playerOrder.GetCurrentPlayer() != connStr || SomeoneNeedsToSelectWildColor) {
      List<string> peopleToSendTo = new List<string>() {connStr}; // init list with just the player's connStr
      await _messenger.SendFrontendError(peopleToSendTo, "Can't play a card when it's not your turn, silly!"); // might not need to await tbh
      return false;
    } 
      
    bool cardCanBePlayed = CanCardBePlayed(card);
    if (!cardCanBePlayed) {
      List<string> peopleToSendTo = new List<string>() {connStr}; // init list with just the player's connStr
      await _messenger.SendFrontendError(peopleToSendTo, "Can't play that card, silly!"); // might not need to await tbh
      return false;
    }
    else {
      _players[_playerOrder.GetCurrentPlayer()].RemoveCard(card);
      _discardPile.Push(card);
      _CurrentColor = card.ColorEnum;
      
      _messenger.Log($"{connStr} played a card: {JsonConvert.SerializeObject(card)}");
      await CancelTimer(card);
      // did someone win?
      if (_players[_playerOrder.GetCurrentPlayer()].Hand.Count == 0) {
        Winner = _players[_playerOrder.GetCurrentPlayer()].Name;
        _messenger.Log("Someone won.");
        await _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
        return true;
      }

      // handle special card
      var SpecialCards = new List<UnoValueLyssie> {
          UnoValueLyssie.DrawTwo,
          UnoValueLyssie.Skip,
          UnoValueLyssie.SkipAll,
          UnoValueLyssie.Reverse,
          UnoValueLyssie.Wild,
          UnoValueLyssie.WildDrawFour,
      };
      if (SpecialCards.Contains(card.ValueEnum)) {
        HandleSpecialCard(card);
        await _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
      }
      else {
        await InitiateTurn();
        await _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
      }
      return true;
    }
}

public async Task SelectWild(string connStr, UnoColorLyssie color) {
  if (connStr == _playerOrder.GetCurrentPlayer() && SomeoneNeedsToSelectWildColor) {
    SomeoneNeedsToSelectWildColor = false;
    _CurrentColor = color;
    await InitiateTurn();
  }
  else {

    await _messenger.SendFrontendError(GetAllConnStrsIncGameboard(), "Can't select a wild color right now, silly!"); // might not need the await tbh
  }
}

  //public bool AllEqual<T>(params T[]? values)
  //{
  //    if (values is null || values.Length == 0) return false;
  //    return values.All(v => v.Equals(values[0]));
  //}

  // handles wilds
  private async void HandleSpecialCard(UnoCardModLyssie card) {
    string nextPersonConnStr = _playerOrder.GetActivePlayersInOrder()[1];
    switch (card.ValueEnum) {
      case UnoValueLyssie.DrawTwo:
        List<UnoCardModLyssie> cards2 = _deck.Draw(2);
        _players[nextPersonConnStr].AddCards(cards2);
        _playerOrder.SetNextPlayer(2); // skips their turn
        _messenger.Log($"{_players[nextPersonConnStr].Name} drew 2 and was skipped.");
        await InitiateTurn();
        break;
      case UnoValueLyssie.Reverse:
        _playerOrder.ToggleDirection();
        _messenger.Log("Direction reversed.");
        await InitiateTurn();
        break;
      case UnoValueLyssie.SkipAll:
        _playerOrder.SetNextPlayer((_playerOrder.GetCurrentPlayer()));
        _messenger.Log("Skip All played.");
        await InitiateTurn();
        break;
      case UnoValueLyssie.Skip:
        _playerOrder.SetNextPlayer(2); // skips their turn
        _messenger.Log($"{_players[nextPersonConnStr].Name} was skipped.");
        await InitiateTurn();
        break;
      case UnoValueLyssie.Wild:
        _messenger.Log("Wild played.?");
        SomeoneNeedsToSelectWildColor = true;
        _playerOrder.SetNextPlayer(2); // skips next person's turn
        break;
      case UnoValueLyssie.WildDrawFour:
        _messenger.Log("Wild Draw 4 played.");
        SomeoneNeedsToSelectWildColor = true;
        _playerOrder.SetNextPlayer(2);
        break;
      default:
        _messenger.Log("Unsupported special card played.");
        break;
    }
  }
  public async Task<bool> DrawCard(string connStr)
  {
    if (!GameStarted) {
      return false;
    }
    else if (_playerOrder.GetCurrentPlayer() != connStr) {
        List<string> peopleToSendTo = new List<string>() {connStr}; // init list with just the player's connStr
        await _messenger.SendFrontendError(peopleToSendTo, "Can't DRAW a card when it's not your turn, silly!"); // might not need to await tbh
        return false;
    }
    else if (SomeoneNeedsToSelectWildColor) {
      List<string> peopleToSendTo = new List<string>() {connStr}; // init list with just the player's connStr
        await _messenger.SendFrontendError(peopleToSendTo, "Can't DRAW a card when another player needs to select a color, silly!"); // might not need to await tbh
        return false;
    }
    else {
      if (_deck.GetCards().Count == 0) {
        _deck.ReclaimCards();
        _deck.Shuffle();
        _messenger.Log($"Deck shuffled! {_deck.GetCards().Count} left");
        // grab first card off the pile
        UnoCardModLyssie topDiscardCard = _discardPile.Peek();
        _discardPile.Clear();
        _discardPile.Push(topDiscardCard);
        // remove later
        List<string> peopleToSendTo = new List<string> {GameboardConnStr};
        await _messenger.SendFrontendError(peopleToSendTo, "Discard cards were reclaimed!");
      }
      var card = _deck.Draw();
      _players[connStr].AddCard(card);
      _messenger.Log($"{connStr} drew a card: {card.Color.ToString()} {card.Value.ToString()}");
      await InitiateTurn();
      return true;
    }
  }

    public List<UnoCardModLyssie> GetPlayerHand(string connStr)
    {
      return _players[connStr].GetHand();
    }

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

        _messenger.Log("Timer started.");
    }

    public async Task CancelTimer(UnoCardModLyssie card)
    {
        lock (_lock)
        {
            _moveTimer.Stop();
            _messenger.Log($"Timer cancelled by card {card.ValueEnum} {card.ColorEnum}.");
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
    public string Winner { get; private set; } = "";
    private List<string> GetAllConnStrsIncGameboard() {
      List<string> allConnStrs = new List<string>(_playerOrder.GetAllPlayers())
      {
        GameboardConnStr // adds this to the end of all the others
      }; 
      // shallow copy - the list is different but the objects are references and can be messed with
      return allConnStrs;
    }
    // public to be accessible to tests lol
    public bool CanCardBePlayed(UnoCardModLyssie card) {
      var lastCard = _discardPile.Peek();
      var isColorMatch = card.ColorEnum == _CurrentColor;
      var isValueMatch = card.ValueEnum == lastCard.ValueEnum;
      var isWildPlayable = new List<UnoValueLyssie> {UnoValueLyssie.Wild, UnoValueLyssie.WildDrawFour}.Contains(card.ValueEnum);
      return isColorMatch || isValueMatch || isWildPlayable;
    }
    
}