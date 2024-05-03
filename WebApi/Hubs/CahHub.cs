using System.Timers;
using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Hubs;

public interface ICahClient
{
    Task GameStarted();
    Task ReceiveCards(List<CahCard> cards);
    Task CardsPlayed(List<CahCard> cards);
    Task Gambled(CahCard pickedCard, CahCard gambledCard);
    Task ReceiveWinner(string winner);
    Task ReceiveBlackCard(CahCard card);
    Task Pong();
}

public class CahHub : Hub<ICahClient>
{
    private const string Name = "Name";
    private const string Room = "Room";
    private const string PlayerType = "PlayerType";
    
    private string ContextId => Context.ConnectionId;

    private IDictionary<string, CahGame?> _games;
    private readonly CahFactory _factory;

    public CahHub(IDictionary<string, CahGame?> games, CahFactory factory)
    {
        _games = games;
        _factory = factory;
    }

    public async Task JoinRoom(ConnectionOptions connectionOptions)
    {
        var roomId = connectionOptions.Room;
        if (!TryGetGame(out var game, roomId))
        {
            var newGame = _factory.Build();
            newGame.Gameboard = ContextId;
            newGame.Room = roomId;
            newGame.PickingFinished += TimerTest;
            _games.Add(roomId, newGame);
        }
        else
        {
            if (connectionOptions.Name is null) return;
            if (!game.AddPlayer(ContextId)) return;
        }
        
        Context.Items.Add(Room, roomId);

        await Groups.AddToGroupAsync(ContextId, roomId);
    }

    private void TimerTest(object? sender, EventArgs e)
    {
        Console.WriteLine("TTT Timer test");
        TryGetGame(out var game);
        Console.Write(game.Room);
    }

    public async Task StartGame()
    {
        if (!TryGetGame(out var game)) return;
        
        var playerHands = game.StartGame();
        
        await Clients.Group(GetRoomId()).GameStarted();
        foreach (var (player, cards) in playerHands)
        {
            await Clients.Client(player).ReceiveCards(cards);
        }

        await InitiatePicking();
    }

    public async Task InitiatePicking()
    {
        if (!TryGetGame(out var game)) return;
        
        var beforePlayerHands = game.GetPlayerHands();
        game.InitiatePicking();
        var afterPlayerHands = game.GetPlayerHands();
        
        foreach (var (player, cards) in afterPlayerHands)
        {
            var newCards = cards.Except(beforePlayerHands[player]).ToList();
            await Clients.Client(player).ReceiveCards(newCards);
        }
        await Clients.Group(GetRoomId()).ReceiveBlackCard(game.CurrentBlackCard!);
        
    }
    
    // await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("StartTimer", _pickingTimeLimit.Seconds);
    // await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("SetPickAmount", _pickAmount);
    //
    // await _hubContext.Clients.Client(cardCzar).SendAsync("CardCzar");

    public async Task SendCards(List<CahCard> cards)
    {
        if (!TryGetGame(out var game)) return;
        if (!game.PlayCards(ContextId, cards, out var allPlayersMoved)) return;

        await Clients.Client(ContextId).CardsPlayed(cards);
        if (allPlayersMoved)
        {
            await AllPlayersMoved();
        }
    }

    public async Task Gamble(CahCard pickedCard, CahCard gambledCard)
    {
        if (!TryGetGame(out var game)) return;
        if (!game.Gamble(ContextId, pickedCard, gambledCard)) return;

        await Clients.Client(ContextId).Gambled(pickedCard, gambledCard);
    }

    public async Task SendWinner(string winner)
    {
        if (!TryGetGame(out var game)) return;
        if (!game.SelectWinner(ContextId, winner)) return;

        await Clients.Group(GetRoomId()).ReceiveWinner(winner);
    }
    
    private async Task AllPlayersMoved()
    {
        // var nonCzarPlayers = GetNonCzarPlayers();
        // foreach (var playerName in nonCzarPlayers)
        // {
        //     await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("ReceivePlayerCardPicks", playerName,
        //         _players[playerName].DequeueCards());
        // }
        //
        // await _hubContext.Clients.Client(_playerOrder.Current())
        //     .SendAsync("StartTimer", _judgingTimeLimit.Seconds);
        //
        // lock (_lock)
        // {
        //     _judgingTimer.Start();
        // }
    }
    
    // Timer callbacks
    // private async void OnPickingTimeElapsed(object? source, ElapsedEventArgs e)
    // {
    //     var nonCzarPlayers = GetNonCzarPlayers();
    //     foreach (var playerName in nonCzarPlayers)
    //     {
    //         var pickedCards = _players[playerName].PickedCards;
    //         while (pickedCards.Count < _pickAmount)
    //         {
    //             pickedCards.Enqueue(_players[playerName].PickRandomCard());
    //         }
    //
    //         await _hubContext.Clients.Client(_playerOrder.Current()).SendAsync("ReceivePlayerCardPicks", playerName,
    //             _players[playerName].DequeueCards());
    //     }
    //
    //     await _hubContext.Clients.Client(_playerOrder.Current())
    //         .SendAsync("StartTimer", _judgingTimeLimit.Seconds);
    // }
    
    // private async void OnJudgingTimeElapsed(object? sender, ElapsedEventArgs e)
    // {
    //     await InitiatePicking();
    // }

    public async Task Ping() => await Clients.Caller.Pong();
    

  
    #region Helpers
    
    private bool TryGetGame(out CahGame game, string? roomId = null)
    {
        roomId ??= GetRoomId();
        return _games.TryGetValueAs(roomId, out game);
    }

    private string GetRoomId()
    {
        return Context.Items.TryGetValueAs(Room, out string roomName)
            ? roomName
            : throw new InvalidOperationException("Cah Room id not found");
    }
    
    #endregion
}
