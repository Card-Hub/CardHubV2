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

    public async Task SendCard(StandardCard card)
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
        
        // Check if it's the user's turn
        //
        var card = _game.drawCard();

        return Clients.Caller.SendAsync("ReceiveCard", "Gameboard", card);
    }

    public Task SendConnectedUsers(string room)
    {
        var users = _userConnections.Values.Where(x => x.Room == room).Select(x => x.User);
        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
            return base.OnDisconnectedAsync(exception);
        
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