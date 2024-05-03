using System.Text.Json;
using System.Text.Json.Serialization;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.Hubs;
using WebApi.Models;
using Timer = System.Timers.Timer;

namespace WebApi.GameLogic;

/*
 * Required player data
 * - List<CahCard>
 * - 
 */

public enum CahHouseRules
{
    RandoCardrissian,
    HappyEnding,
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
    private List<CahCard> _gambledCards = [];
    public CahCard? CurrentBlackCard { get; set; }
    
    private int _pickAmount;
        
    private PlayerOrder _playerOrder = [];
    private Dictionary<string, CahPlayer> _players = new();
    private HashSet<string> _playersMoved = [];
    
    
    private TimeSpan _pickingTimeLimit;
    private TimeSpan _judgingTimeLimit;
    private Timer _pickingTimer;
    private Timer _judgingTimer;
    private readonly object _lock = new();

    public string Gameboard { get; set; }
    public string Room { get; set; }
    
    private static List<CahPack>? _cahPacks;
    
    public event EventHandler PickingFinished;

    public CahGame(IHubContext<BaseHub> hubContext)
    {
        if (_cahPacks is null)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "cah-cards-full.json");
            var json = File.ReadAllText(path);
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
        }

        // _pickingTimeLimit = TimeSpan.FromSeconds(5);
        // _pickingTimer = new Timer(_pickingTimeLimit);
        // _pickingTimer.Elapsed += OnPickingTimeElapsed;
        // _pickingTimer.Elapsed += (_, _) => PickingFinished?.Invoke(this, EventArgs.Empty);
        //
        // _judgingTimeLimit = TimeSpan.FromSeconds(5);
        // _judgingTimer = new Timer(_judgingTimeLimit);
        // _judgingTimer.Elapsed += OnJudgingTimeElapsed;
    }

    public bool AddPlayer(string playerName)
    {
        if (_players.ContainsKey(playerName) || playerName == Gameboard) return false;

        _players.Add(playerName, new CahPlayer(playerName));
        _playerOrder.Add(playerName);
        return true;
    }

    public void InitGame()
    {
        _whiteDeck = new Deck<CahCard>(_cahPacks![1].WhiteCards);
        _blackDeck = new Deck<CahCard>(_cahPacks[1].BlackCards);
    }

    public Dictionary<string, List<CahCard>> StartGame()
    {
        InitGame();

        _whiteDeck.Shuffle();
        _blackDeck.Shuffle();
        _playerOrder.ShuffleOrder();

        foreach (var player in _playerOrder)
        {
            var drawnCards = _whiteDeck.Draw(10);
            _players[player].AddCards(drawnCards);
        }
        
        return GetPlayerHands();
    }

    public object InitiatePicking()
    {
        // lock (_lock)
        // {
        //     _pickingTimer.Start();
        // }

        CurrentBlackCard = _blackDeck.Draw();
        _pickAmount = CurrentBlackCard.PickAmount;

        var nonCzarPlayers = GetNonCzarPlayers();
        var cardCzar = _playerOrder.Current();
        if (_pickAmount == 3)
        {
            foreach (var player in nonCzarPlayers)
            {
                var cards = _whiteDeck.Draw(2);
                _players[player].AddCards(cards);
            }
        }
        
        // return object that contains the current black card, the pick amount, playerhands, and the card czar
        return new
        {
            PickAmount = _pickAmount,
            PlayerHands = _playerOrder.Select(player => new
            {
                Player = player,
                Cards = _players[player].GetHand()
            }),
            CardCzar = cardCzar
        };
    }

    public bool PlayCards(string playerName, List<CahCard> cards, out bool allPlayersMoved)
    {
        allPlayersMoved = false;
        if (IsCzar(playerName) || _pickAmount != cards.Count) return false;
        if (!_players[playerName].EnqueueCards(cards)) return false;
        
        _playersMoved.Add(playerName);
        if (_playersMoved.Count == _players.Count)
        {
            allPlayersMoved = true;
        }
        return true;
    }
    
    public bool Gamble(string playerName, CahCard pickedCard, CahCard gambledCard)
    {
        if (IsCzar(playerName) || _pickAmount != 1) return false;
        if (!_players[playerName].Gamble(pickedCard, gambledCard)) return false;
        
        _gambledCards.Add(gambledCard);
        return true;
    }
    
    // All players have picked their cards
    public async Task AllPlayersMoved()
    {
        // lock (_lock)
        // {
        //     _pickingTimer.Stop();
        // }

        var nonCzarPlayers = GetNonCzarPlayers();
        foreach (var playerName in nonCzarPlayers)
        {
            // await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("ReceivePlayerCardPicks", playerName,
            //     _players[playerName].DequeueCards());
        }

        // await _hubContext.Clients.Client(_playerOrder.Current())
        //     .SendAsync("StartTimer", _judgingTimeLimit.Seconds);
        
        // lock (_lock)
        // {
        //     _judgingTimer.Start();
        // }
    }

    // Some players did not pick their cards in time
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

            // await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("ReceivePlayerCardPicks", playerName,
            //     _players[playerName].DequeueCards());
        }

        // await _hubContext.Clients.Client(_playerOrder.Current())
        //     .SendAsync("StartTimer", _judgingTimeLimit.Seconds);
    }

    public bool SelectWinner()
    {
        var nonCzarPlayers = GetNonCzarPlayers().ToArray();
        var index = new Random().Next(nonCzarPlayers.Length);
        var player = nonCzarPlayers.ElementAt(index);
        
        return SelectWinner(Czar, player);
    }
    
    public bool SelectWinner(string playerSelecting, string playerSelected)
    {
        if (!IsCzar(playerSelecting) || IsCzar(playerSelected)) return false;

        _players[playerSelected].AddWonCard(CurrentBlackCard!);
        _playerOrder.SetNextCurrent();

        // lock (_lock)
        // {
        //     _judgingTimer.Stop();
        // }

        return true;
    }
    
    // Czar did not select a winner in time
    private async void OnJudgingTimeElapsed(object? sender, ElapsedEventArgs e)
    {
        _playerOrder.SetNextCurrent();
        InitiatePicking();
    }

    #region Helper methods

    public List<CahCard> GetPlayerHand(string playerName)
    {
        var player = _playerOrder.GetPlayer(playerName);
        return _players[player].GetHand();
    }
    
    public Dictionary<string, List<CahCard>> GetPlayerHands()
    {
        return _playerOrder.Select(player => new
        {
            Player = player,
            Cards = _players[player].GetHand()
        }).ToDictionary(x => x.Player, x => x.Cards);
    }
    
    private string Czar => _playerOrder.Current();

    private bool IsCzar(string playerName) =>
        _playerOrder.Current() == playerName;


    private IEnumerable<string> GetNonCzarPlayers() =>
        _playerOrder.Where(player => player != _playerOrder.Current()).ToArray();

    #endregion
}