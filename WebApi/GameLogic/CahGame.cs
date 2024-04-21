using System.Text.Json;
using System.Text.Json.Serialization;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.Hubs;
using WebApi.Models;
using Timer = System.Timers.Timer;

namespace WebApi.GameLogic;

public enum CahHouseRules
{
    WheatonsLaw,
    RebootingTheUniverse,
    PackingHeat,
    Meritocracy,
    SmoothOperator,
    TieBreaker
}

public class CahPack
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("white")] public List<CahCard> WhiteCards { get; set; }

    [JsonPropertyName("black")] public List<CahCard> BlackCards { get; set; }

    public CahPack()
    {
    }

    public CahPack(string name, List<CahCard> whiteCards, List<CahCard> blackCards)
    {
        this.Name = name;
        this.WhiteCards = whiteCards;
        this.BlackCards = blackCards;
    }
}

public class CahGame
{
    private Deck<CahCard> _whiteDeck = new();
    private Deck<CahCard> _blackDeck = new();
    private PlayerOrder _cardCzarOrder = [];
    private Dictionary<string, CahPlayer> _players = new();
    private HashSet<string> _playersMoved = [];
    private CahCard? _currentBlackCard;
    private int _pickAmount;

    private TimeSpan _pickingTimeLimit;
    private TimeSpan _judgingTimeLimit;
    private Timer _pickingTimer;
    private Timer _judgingTimer;
    private readonly object _lock = new();

    public string Gameboard { get; set; }

    IHubContext<BaseHub> _hubContext;

    private readonly List<CahPack> _cahPacks;

    public CahGame(IHubContext<BaseHub> hubContext)
    {
        var json = File.ReadAllText(
            "C:\\Users\\admoz\\source\\repos\\SeniorProjects\\CardHubV2\\WebApi\\Data\\cah-cards-full.json");
        _cahPacks = JsonSerializer.Deserialize<List<CahPack>>(json)!;
        foreach (var pack in _cahPacks)
        {
            foreach (var card in pack.WhiteCards)
            {
                card.Type = CahCardType.White;
            }

            foreach (var card in pack.BlackCards)
            {
                card.Type = CahCardType.Black;
            }
        }

        _pickingTimeLimit = TimeSpan.FromSeconds(5);
        _pickingTimer = new Timer(_pickingTimeLimit);
        _pickingTimer.Elapsed += OnPickingTimeElapsed;
        
        _judgingTimeLimit = TimeSpan.FromSeconds(5);
        _judgingTimer = new Timer(_judgingTimeLimit);
        _judgingTimer.Elapsed += OnJudgingTimeElapsed;
        _hubContext = hubContext;
    }

    public bool AddPlayer(string playerName)
    {
        if (_players.ContainsKey(playerName))
        {
            return false;
        }

        _players.Add(playerName, new CahPlayer(playerName));
        _cardCzarOrder.Add(playerName);
        return true;
    }

    public async Task InitGame()
    {
        _whiteDeck = new Deck<CahCard>(_cahPacks[1].WhiteCards);
        _blackDeck = new Deck<CahCard>(_cahPacks[1].BlackCards);
    }

    public async Task StartGame()
    {
        await InitGame();
        
        _whiteDeck.Shuffle();
        _blackDeck.Shuffle();
        _cardCzarOrder.ShuffleOrder();

        foreach (var player in _cardCzarOrder)
        {
            var drawnCards = _whiteDeck.Draw(10);
            _players[player].AddCards(drawnCards);
            
            await _hubContext.Clients.Client(player).SendAsync("ReceiveWhiteCards", drawnCards);
        }

        await InitiatePicking();
    }

    public async Task InitiatePicking()
    {
        lock (_lock)
        {
            _pickingTimer.Start();
        }

        _currentBlackCard = _blackDeck.Draw();
        _pickAmount = _currentBlackCard.Text!.Count(c => c == '_');
        await _hubContext.Clients.All.SendAsync("ReceiveBlackCard", _currentBlackCard);

        var cardCzar = _cardCzarOrder.Current();
        if (_pickAmount == 3)
        {
            var cards = _whiteDeck.Draw(2);
            await _hubContext.Clients.AllExcept(cardCzar).SendAsync("ReceiveWhiteCards", cards);
        }

        var nonCzarPlayers = GetNonCzarPlayers();
        await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("StartTimer", _pickingTimeLimit.Seconds);
        await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("SetPickAmount", _pickAmount);

        await _hubContext.Clients.Client(cardCzar).SendAsync("CardCzar");
    }

    public async Task<bool> PlayCard(string playerName, CahCard card)
    {
        var playerPickedCards = _players[playerName].PickedCards;
        if (playerPickedCards.Contains(card))
        {
            return false;
        }

        if (playerPickedCards.Count >= _pickAmount)
        {
            _playersMoved.Add(playerName);
            if (_playersMoved.Count == _players.Count)
            {
                await AllPlayersMoved();
            }
        }

        _players[playerName].RemoveCard(card);
        _players[playerName].PickedCards.Enqueue(card);
        return true;
    }

    public async Task<bool> SelectWinner(string playerSelecting, string playerSelected)
    {
        if (_cardCzarOrder.Current() != playerSelecting || _cardCzarOrder.Current() == playerSelected)
        {
            return false;
        }

        _players[playerSelected].AddWonCard(_currentBlackCard!);
        _cardCzarOrder.SetNextCurrent();
        await InitiatePicking();
        return true;
    }

    private async void OnPickingTimeElapsed(object? source, ElapsedEventArgs e)
    {
        var nonCzarPlayers = GetNonCzarPlayers();
        foreach (var playerName in nonCzarPlayers)
        {
            var pickedCards = _players[playerName].PickedCards;
            while (pickedCards.Count < _pickAmount)
            {
                pickedCards.Enqueue(_players[playerName].PickRandomCard());
            }
            
            await _hubContext.Clients.Client(_cardCzarOrder.Current()).SendAsync("ReceivePlayerCardPicks", playerName,
                _players[playerName].PickedCards);
        }
        
        await _hubContext.Clients.Client(_cardCzarOrder.Current())
            .SendAsync("StartTimer", _judgingTimeLimit.Seconds);
    }

    private async Task AllPlayersMoved()
    {
        lock (_lock)
        {
            _pickingTimer.Stop();
            _judgingTimer.Start();
        }

        var nonCzarPlayers = GetNonCzarPlayers();
        foreach (var playerName in nonCzarPlayers)
        {
            await _hubContext.Clients.Client(_cardCzarOrder.Current()).SendAsync("ReceivePlayerCardPicks", playerName,
                _players[playerName].PickedCards);
        }
        
        await _hubContext.Clients.Client(_cardCzarOrder.Current())
            .SendAsync("StartTimer", _judgingTimeLimit.Seconds);
    }

    private async Task InitiateJudging()
    {
        lock (_lock)
        {
            _pickingTimer.Stop();
            _judgingTimer.Start();
        }
        
        var nonCzarPlayers = GetNonCzarPlayers();
        foreach (var playerName in nonCzarPlayers)
        {
            await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("ReceivePlayerCardPicks", playerName,
                _players[playerName].PickedCards);
        }
    }
    
    private async void OnJudgingTimeElapsed(object? sender, ElapsedEventArgs e)
    {
        _cardCzarOrder.SetNextCurrent();
        await InitiatePicking();
    }

    private string[] GetNonCzarPlayers() => _cardCzarOrder.Where(player => player != _cardCzarOrder.Current()).ToArray();

    public List<CahCard> GetPlayerHand(string playerName)
    {
        var player = _cardCzarOrder.GetPlayer(playerName);
        return _players[player].GetHand();
    }
}