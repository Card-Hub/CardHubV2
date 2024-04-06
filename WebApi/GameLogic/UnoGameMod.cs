using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.Hubs;
using WebApi.Models;
using Timer = System.Timers.Timer;

namespace WebApi.GameLogic;

public class UnoGameMod
{
    private IDeck<UnoCard> _deck;
    private Stack<UnoCard> _discardPile = new();
    private PlayerOrder<UnoPlayer, UnoCard> _playerOrder = [];
    // dictionary<string, UnoPlayer>

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
        _deck.Shuffle();
        _playerOrder.ShuffleOrder();

        foreach (var player in _playerOrder)
        {
            var drawnCards = _deck.Draw(7);
            player.AddCards(drawnCards);
        }

        var counter = 0;
        while (counter++ < 10)
        {
            var topCard = _deck.Draw();
            if (topCard.Color == "black") continue;
            _discardPile.Push(topCard);
            break;
        }

        await _hubContext.Clients.Client(Gameboard).SendAsync("ReceiveCards", _discardPile);
        await InitiateTurn();
    }

    public void AddPlayer(string player)
    {
        _playerOrder.Add(new UnoPlayer(player));
    }

    public async Task<bool> PlayCard(string playerName, UnoCard card)
    {
        if (_playerOrder.Current().Name != playerName) return false;
        
        
        
        _playerOrder.Current().RemoveCard(card);
        _discardPile.Push(card);

        await CancelTimer(card.ToString());
        _playerOrder.SetNextCurrent();
        await InitiateTurn();
        return true;
    }

    public UnoCard? DrawCard(string playerName)
    {
        var player = _playerOrder.GetPlayer(playerName);
        
        if (_playerOrder.Current().Name != playerName) return null;
        
        var card = _deck.Draw();
        player.AddCard(card);
        return card;
    }

    public List<UnoCard> GetPlayerHand(string playerName)
    {
        var player = _playerOrder.GetPlayer(playerName);
        return player.GetHand();
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
        
        var player = _playerOrder.Current().Name;
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
        
        await _hubContext.Clients.Client(_playerOrder.Current().Name).SendAsync("StartTimer", 0);
    }

    private async void OnMoveTimeElapsed(object? source, ElapsedEventArgs e)
    {
        Console.WriteLine("Turn elapsed at {0:mm:ss}", e.SignalTime);
        var drawnCard = _deck.Draw();
        _playerOrder.Current().AddCard(drawnCard);

        await _hubContext.Clients.Client(_playerOrder.Current().Name).SendAsync("ReceiveCard", "Gameboard", drawnCard);
        lock (_lock)
        {
            _moveTimer.Stop();
            Console.WriteLine($"Timer cancelled by timeout.");
        }

        _playerOrder.SetNextCurrent();
        await InitiateTurn();
    }
}