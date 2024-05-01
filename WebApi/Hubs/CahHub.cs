using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Hubs;

public class CahHub : Hub
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
        
        await Clients.Group(GetRoomId()).SendAsync("GameStarted");
        foreach (var (player, cards) in playerHands)
        {
            await Clients.Client(player).SendAsync("ReceiveCards", cards);
        }
    }
    
    // await _hubContext.Clients.Group(Room).SendAsync("ReceiveBlackCard", _currentBlackCard);
    //
    // var nonCzarPlayers = GetNonCzarPlayers();
    // var cardCzar = _playerOrder.Current();
    //     if (_pickAmount == 3)
    // {
    //     foreach (var player in nonCzarPlayers)
    //     {
    //         var cards = _whiteDeck.Draw(2);
    //         await _hubContext.Clients.Client(player).SendAsync("ReceiveWhiteCards", cards);
    //     }
    // }
    //     
    // await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("StartTimer", _pickingTimeLimit.Seconds);
    // await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("SetPickAmount", _pickAmount);
    //
    // await _hubContext.Clients.Client(cardCzar).SendAsync("CardCzar");

    public async Task SendCards(List<CahCard> cards)
    {
        if (!TryGetGame(out var game)) return;
        if (!game.PlayCards(ContextId, cards)) return;

        await Clients.Client(ContextId).SendAsync("CardsPlayed", cards);
    }

    public async Task Gamble(CahCard pickedCard, CahCard gambledCard)
    {
        if (!TryGetGame(out var game)) return;
        if (!game.Gamble(ContextId, pickedCard, gambledCard)) return;

        await Clients.Client(ContextId).SendAsync("Gambled", pickedCard, gambledCard);
    }

    public async Task SendWinner(string winner)
    {
        if (!TryGetGame(out var game)) return;
        if (!game.SelectWinner(ContextId, winner)) return;

        await Clients.Group(GetRoomId()).SendAsync("ReceiveWinner", winner);
    }

    public async Task Ping() => await Clients.Caller.SendAsync("Pong");
    

  
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
