using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Hubs;

public class BlackJackHub : Hub
{
    private const string Name = "Name";
    private const string Room = "Room";
    private const string PlayerType = "PlayerType";
    
    private string ContextId => Context.ConnectionId;

    private IDictionary<string, BlackJackGame?> _games;

    private BlackJackMessenger _messenger;
    // private readonly CahFactory _factory;

    public BlackJackHub(IDictionary<string, BlackJackGame?> games, BlackJackMessenger messenger)
    {
        _games = games;
        _messenger = messenger;
    }

    public async Task JoinRoom(ConnectionOptions connectionOptions)
    {
        string Name = connectionOptions.Name;
        var roomId = connectionOptions.Room;
        if (!TryGetGame(out var game, roomId))
        {
            var newGame = new BlackJackGame(_messenger, ContextId);
            _games.Add(roomId, newGame);
        }
        else
        {
            if (connectionOptions.Name is null) return;
            if (!game.AddPlayer(Name, ContextId)) return;
        }
        
        Context.Items.Add(Room, roomId);

        await Groups.AddToGroupAsync(ContextId, roomId);
    }

    public async Task DrawCardBlackJackHub()
    {
        if (!TryGetGame(out var game)) return;
        game.DrawCard(ContextId);
    }


    public async Task StandBlackJackHub()
    {
        if (!TryGetGame(out var game)) return;
        game.Stand(ContextId);
    }


    public async Task RestartBlackJackHub()
    {
        if (!TryGetGame(out var game)) return;
        game.Restart();
    }

    public async Task StartGame()
    {
        if (!TryGetGame(out var game)) return;
        
        game.StartGame();
        
        await Clients.Group(GetRoomId()).SendAsync("GameStarted");

    }

    public async Task BetBlackJackHub(int amt) {
        if (!TryGetGame(out var game)) return;
        game.TakeBet(ContextId, amt);
    }

    public async Task Ping() => await Clients.Caller.SendAsync("Pong");
    

  
    #region Helpers
    
    private bool TryGetGame(out BlackJackGame game, string? roomId = null)
    {
        roomId ??= GetRoomId();
        return _games.TryGetValueAs(roomId, out game);
    }

    private string GetRoomId()
    {
        return Context.Items.TryGetValueAs(Room, out string roomName)
            ? roomName
            : throw new InvalidOperationException("BlackJack Room id not found");
    }
    
    #endregion
}
