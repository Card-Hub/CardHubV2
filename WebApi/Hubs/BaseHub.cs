using WebApi.GameLogic;
using WebApi.Models;

namespace WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;

public class UserConnection
{
    public string? ConnectionId { get; set; }
    public required string User { get; set; }
    public required string Room { get; set; }
    public required UserType UserType { get; set; }
    
    public override string ToString() => $"{{ User: {User}, Room: {Room}, Type: {UserType}, ConnectionId: {ConnectionId} }}";
}

public class UserMessage
{
    public required string User { get; set; }
    public required string Message { get; set; }
}

public interface IBaseClient
{
    
}

public partial class BaseHub : Hub
{
    private const string BotUser = "Bot";
    private IDictionary<string, UserConnection> _userConnections;
    private UnoGame _game;

    public BaseHub(IDictionary<string, UserConnection> userConnections, IBaseGame game)
    {
        _userConnections = userConnections;
        _game = (UnoGame) game;
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Connected");
        return base.OnConnectedAsync();
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        Console.WriteLine("Join Room");
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        userConnection.ConnectionId = Context.ConnectionId;
        _userConnections[Context.ConnectionId] = userConnection;
        Console.WriteLine(userConnection.ToString());

        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
            new UserMessage
            {
                User = BotUser,
                Message = $"{userConnection.User} has joined the room {userConnection.Room}"
            });
        await SendConnectedUsers(userConnection.Room);

        if (userConnection.UserType == UserType.Player)
        {
            _game.AddPlayer(userConnection.User);    
        }
    }

    public async Task SendMessage(string message)
    {
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveMessage",
                    new UserMessage
                    {
                        User = userConnection.User,
                        Message = message
                    });
        }
    }

    public async Task SendCard(UnoCard card)
    {
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            var gameboardId = _userConnections.Values
                .Where(x => x.Room == userConnection.Room && x.UserType == UserType.Gameboard)
                .Select(x => x.ConnectionId).SingleOrDefault();
            if (string.IsNullOrEmpty(gameboardId))
            {
                throw new Exception("Gameboard not found");
            }

            await Clients.Client(gameboardId).SendAsync("ReceiveCard", userConnection.User, card);
        }
    }

    public Task DrawCard()
    {
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            var userName = userConnection.User;
            var card = _game.DrawCard(userName);
            var playerHand = _game.GetPlayerHand(userName);
            
            foreach (var pCard in playerHand)
            {
                Console.WriteLine(pCard);
                Clients.Caller.SendAsync("ReceiveCard", "Gameboard", pCard);
            }
        }

        return Task.CompletedTask;
    }
    
    public Task StartGame()
    {
        _game.ShuffleDeck();
        _game.StartGame();
        
        var roomConnections = _userConnections.Values.Where(x => x.Room == _userConnections[Context.ConnectionId].Room);

        foreach (var conn in roomConnections.Where(x => x.UserType == UserType.Player))
        {
            var userName = conn.User;
            var hand = _game.GetPlayerHand(userName);
            Clients.Client(conn.ConnectionId!).SendAsync("StartedGame", hand);
        }

        return Task.CompletedTask;
    }

    public Task SendConnectedUsers(string room)
    {
        var users = _userConnections.Values.Where(x => x.Room == room && x.UserType == UserType.Player)
            .Select(x => x.User);
        Console.WriteLine("in send connected: ", users);
        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
            return base.OnDisconnectedAsync(exception);

        _game.RemovePlayer(userConnection.User);
        
        Console.WriteLine($"Disconnected", userConnection.ToString());
        
        _userConnections.Remove(Context.ConnectionId);
        Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
            new UserMessage
            {
                User = BotUser,
                Message = $"{userConnection.User} has left the room {userConnection.Room}"
            });
        SendConnectedUsers(userConnection.Room);

        return base.OnDisconnectedAsync(exception);
    }
}