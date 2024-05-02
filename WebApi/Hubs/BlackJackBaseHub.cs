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
        var roomId = connectionOptions.Room;
        if (!TryGetGame(out var game, roomId))
        {
            var newGame = new BlackJackGame(_messenger, ContextId);
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
