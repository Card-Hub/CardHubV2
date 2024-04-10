using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.Hubs;
using WebApi.Models;
using Timer = System.Timers.Timer;

namespace WebApi.GameLogic;

public class UnoGameMod
{
    private IDeck<UnoCardMod>? _deck;
    private Stack<UnoCardMod> _discardPile = new();
    private PlayerOrder _playerOrder = [];
    private Dictionary<string, UnoPlayer> _players = new();

    private TimeSpan _moveTimeLimit;
    private Timer _moveTimer;
    private readonly object _lock = new();

    public string Gameboard { get; set; }

    IHubContext<BaseHub> _hubContext;
    private readonly UnoDeckBuilder _deckBuilder;

    public UnoGameMod(IHubContext<BaseHub> hubContext, UnoDeckBuilder deckBuilder)
    {
        _deckBuilder = deckBuilder;

        _moveTimeLimit = TimeSpan.FromSeconds(5);
        _moveTimer = new Timer(_moveTimeLimit);
        _moveTimer.Elapsed += OnMoveTimeElapsed;
        _hubContext = hubContext;
    }

    public void InitGame(UnoSettings settings)
    {
        _deck = _deckBuilder.Build(settings);
    }

    public async Task StartGame()
    {
        InitGame(new UnoSettings());
        _deck.Shuffle();
        _playerOrder.ShuffleOrder();

        foreach (var player in _playerOrder)
        {
            var drawnCards = _deck.Draw(7);
            _players[player].AddCards(drawnCards);
        }

        var counter = 0;
        while (counter++ < 10)
        {
            var topCard = _deck.Draw();
            if (topCard.Color == UnoColor.Black) continue;
            _discardPile.Push(topCard);
            break;
        }

        await _hubContext.Clients.Client(Gameboard).SendAsync("ReceiveCards", _discardPile);
        await InitiateTurn();
    }

    public void AddPlayer(string player)
    {
        _playerOrder.Add(player);
        if (!_players.ContainsKey(player)) _players[player] = new UnoPlayer(player);
    }

    public async Task<bool> PlayCard(string playerName, UnoCardMod card)
    {
        if (_playerOrder.Current() != playerName) return false;

        var lastCard = _discardPile.Peek();
        var isColorMatch = card.Color == lastCard.Color
                           && card.Color != UnoColor.Black; // Non-wild card edge case
        var isValueMatch = card.Value == lastCard.Value;
        var isWildPlayable = card is { Color: UnoColor.Black, Value: UnoValue.WildDrawFour };

        if (!isColorMatch && !isValueMatch && !isWildPlayable) return false;
        

        _players[_playerOrder.Current()].RemoveCard(card);
        _discardPile.Push(card);

        await CancelTimer(card.ToString());
        _playerOrder.SetNextCurrent();
        await InitiateTurn();
        return true;
    }

    public bool AllEqual<T>(params T[]? values)
    {
        if (values is null || values.Length == 0) return false;
        return values.All(v => v.Equals(values[0]));
    }

    public UnoCardMod? DrawCard(string player)
    {
        var currentPlayer = _playerOrder.GetPlayer(player);

        if (_playerOrder.Current() != player) return null;

        var card = _deck.Draw();
        _players[currentPlayer].AddCard(card);
        return card;
    }

    public List<UnoCardMod> GetPlayerHand(string playerName)
    {
        var player = _playerOrder.GetPlayer(playerName);
        return _players[player].GetHand();
    }

    public void RemovePlayer(string playerName)
    {
        _playerOrder.Remove(playerName);
    }

    public async Task InitiateTurn()
    {
        lock (_lock)
        {
            _moveTimer.Start();
        }

        var player = _playerOrder.Current();
        await _hubContext.Clients.Clients(player, Gameboard).SendAsync("StartTimer", _moveTimeLimit.Seconds);

        Console.WriteLine("Timer started.");
    }

    public async Task CancelTimer(string card)
    {
        lock (_lock)
        {
            _moveTimer.Stop();
            Console.WriteLine($"Timer cancelled by card {card}.");
        }

        await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("StartTimer", 0);
    }

    private async void OnMoveTimeElapsed(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine("Turn elapsed at {0:mm:ss}", e.SignalTime);
        var drawnCard = _deck.Draw();
        var currentPlayer = _playerOrder.Current();
        _players[currentPlayer].AddCard(drawnCard);

        await _hubContext.Clients.Client(currentPlayer).SendAsync("ReceiveCard", "Gameboard", drawnCard);
        lock (_lock)
        {
            _moveTimer.Stop();
            Console.WriteLine($"Timer cancelled by timeout.");
        }

        _playerOrder.SetNextCurrent();
        await InitiateTurn();
    }
}