namespace WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;

public class UserConnection
{
    public required string User { get; set; }
    public required string Room { get; set; }
}


public class GameHub : Hub
{
    private const string BotUser = "Bot";
    private IDictionary<string, UserConnection> _userConnections;
    
    public GameHub(IDictionary<string, UserConnection> userConnections)
    {
        _userConnections = userConnections;
    }

    public async Task SendCard(string message)
    {
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveCard", userConnection.User, message);
        }
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        await Clients.Group(userConnection.Room).SendAsync("ReceiveCard", BotUser,
            $"{userConnection.User} has joined the room {userConnection.Room}");
        await SendConnectedUsers(userConnection.Room);
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
        
        _userConnections.Remove(Context.ConnectionId);
        Clients.Group(userConnection.Room).SendAsync("ReceiveCard", BotUser,
            $"{userConnection.User} has left the room {userConnection.Room}");
        SendConnectedUsers(userConnection.Room);

        return base.OnDisconnectedAsync(exception);
    }
}