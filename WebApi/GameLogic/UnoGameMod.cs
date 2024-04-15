using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.Hubs;
using WebApi.Models;
using Timer = System.Timers.Timer;

namespace WebApi.GameLogic;

public class UnoGameMod
{
    private IDeck<UnoCardMod> _deck;
    private Stack<UnoCardMod> _discardPile = new();
    private PlayerOrder _playerOrder = [];
    private Dictionary<string, UnoPlayer> _players = new();

    private TimeSpan _moveTimeLimit;
    private Timer _moveTimer;
    private readonly object _lock = new();

    private int _toDrawAmount = 0;
    public event EventHandler ColorPicked;
    private UnoColor? _lastColor;
    private string _playerToPickColor = "";

    private static readonly UnoColor[] NormalColors = [UnoColor.Blue, UnoColor.Green, UnoColor.Red, UnoColor.Yellow];

    public string Gameboard { get; set; }

    IHubContext<BaseHub> _hubContext;
    private readonly UnoDeckBuilder _deckBuilder;


    public UnoGameMod(IHubContext<BaseHub> hubContext, UnoDeckBuilder deckBuilder)
    {
        _deckBuilder = deckBuilder;

        _moveTimeLimit = TimeSpan.FromSeconds(30);
        _moveTimer = new Timer(_moveTimeLimit);
        _moveTimer.Elapsed += OnMoveTimeElapsed;
        _hubContext = hubContext;
    }

    public void InitGame(UnoSettings settings)
    {
        _deck = _deckBuilder.Build(settings);
        _deck.Shuffle();
        _playerOrder.ShuffleOrder();
    }

    public async Task StartGame()
    {
        InitGame(new UnoSettings());

        foreach (var player in _playerOrder)
        {
            var drawnCards = _deck.Draw(7);
            _players[player].AddCards(drawnCards);
        }

        var topCard = _deck.Draw();
        var counter = 0;
        while (counter++ < 10)
        {
            if (topCard.Color == UnoColor.Black) continue;
            topCard = _deck.Draw();
            _discardPile.Push(topCard);
            break;
        }

        _lastColor = topCard.Color;


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
        var isValueMatch = card.Value == lastCard.Value;
        var isColorMatch = card.Color == _lastColor &&
                           card.Color != UnoColor.Black && // Wild card edge case
                           _toDrawAmount == 0; // Color matched cards can't be played after drawing
        var isWildImmediatelyPlayable = (lastCard.Value != UnoValue.DrawTwo && card.Value == UnoValue.WildDrawFour) ||
                                        (lastCard.Value != UnoValue.DrawTwo &&
                                         lastCard.Value != UnoValue.WildDrawFour &&
                                         card.Value == UnoValue.Wild);
        var isWildPlayable = (lastCard.Value == UnoValue.Wild || lastCard.Value == UnoValue.WildDrawFour) &&
                             _toDrawAmount == 0;

        if (!isColorMatch && !isValueMatch && !isWildImmediatelyPlayable && !isWildPlayable) return false;

        _players[_playerOrder.Current()].RemoveCard(card);
        _discardPile.Push(card);

        var nextPlayerOffset = 1;
        switch (card.Value)
        {
            case UnoValue.DrawTwo:
                _toDrawAmount += 2;
                break;
            case UnoValue.Reverse:
                _playerOrder.ToggleDirection();
                break;
            case UnoValue.Skip:
                nextPlayerOffset = 2;
                break;
            case UnoValue.SkipAll:
                nextPlayerOffset = _playerOrder.Count();
                break;
            case UnoValue.Wild:
                _lastColor = UnoColor.Black;
                _playerToPickColor = playerName;
                break;
            case UnoValue.WildDrawFour:
                _toDrawAmount += 4;
                _lastColor = UnoColor.Black;
                _playerToPickColor = playerName;
                break;
        }

        if (_lastColor == UnoColor.Black)
        {
            await _hubContext.Clients.Client(playerName).SendAsync("RequestColor", playerName);
        }
        else
        {
            await CancelTimer();
            _playerOrder.SetNextCurrent(nextPlayerOffset);
            await InitiateTurn();
        }

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

    public async Task SetColor(string playerName, UnoColor color)
    {
        if (_lastColor != UnoColor.Black || _playerToPickColor != playerName) return;
        await CancelTimer();
        _playerOrder.SetNextCurrent();
        await InitiateTurn();
    }

    public async Task InitiateTurn()
    {
        lock (_lock)
        {
            _moveTimer.Start();
        }

        var player = _playerOrder.Current();
        await _hubContext.Clients.Clients(player, Gameboard).SendAsync("SetTimer", _moveTimeLimit.Seconds);

        // Debug
        await _hubContext.Clients.Client(player).SendAsync("ReceiveMessage", "It's your turn!");

        Console.WriteLine("Timer started.");
    }

    private async Task CancelTimer()
    {
        lock (_lock)
        {
            _moveTimer.Stop();
        }

        await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("SetTimer", 0);
    }

    private async void OnMoveTimeElapsed(object? source, ElapsedEventArgs e)
    {
        if (_lastColor == UnoColor.Black)
        {
            _lastColor = NormalColors[new Random().Next(0, NormalColors.Length)];
        }

        if (!await CompleteTurn())
        {
            var drawnCard = _deck.Draw();
            var currentPlayer = _playerOrder.Current();
            _players[currentPlayer].AddCard(drawnCard);

            await _hubContext.Clients.Client(currentPlayer).SendAsync("ReceiveCard", "Gameboard", drawnCard);
        }

        lock (_lock)
        {
            _moveTimer.Stop();
            Console.WriteLine($"Timer cancelled by timeout.");
        }

        _playerOrder.SetNextCurrent();
        await InitiateTurn();
    }


    private async Task<bool> CompleteTurn()
    {
        if (_toDrawAmount <= 0) return false;

        var currentPlayer = _playerOrder.Current();
        var drawnCards = _deck.Draw(_toDrawAmount);
        _players[currentPlayer].AddCards(drawnCards);
        _toDrawAmount = 0;

        await _hubContext.Clients.Client(currentPlayer).SendAsync("ReceiveCards", drawnCards);
        return true;
    }
}