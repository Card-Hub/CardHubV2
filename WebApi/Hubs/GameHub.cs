namespace WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;
public class UserConnection
{
    public required string User { get; set; }
    public required string Room { get; set; }
}


public class GameHub : Hub
{
    private IDictionary<string, UserConnection> _userConnections;
    
    public GameHub(IDictionary<string, UserConnection> userConnections)
    {
        _userConnections = userConnections;
    }

    public async Task SendMessage(string message)
    {
        Console.WriteLine($"C# connection id{Context.ConnectionId}");
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection) && userConnection != null)
        {
            Console.WriteLine($"C# Sending message from {userConnection.User} to group {userConnection.Room}: {message}");
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveMessage", $"from {userConnection.User}", message);
        }
        else
        {
            Console.WriteLine($"C# Unable to send message. UserConnection not found for ConnectionId: {Context.ConnectionId}");
        }
    }

    // public async Task SendMessage(string message)
    // {
    //     Console.WriteLine($"C# connection id {Context.ConnectionId}");

    //     if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection) && userConnection != null)
    //     {
    //         Console.WriteLine($"C# Sending message from {userConnection.User} to group {userConnection.Room}: {message}");

    //         // Check if Clients is not null before calling SendAsync
    //         if (Clients != null)
    //         {
    //             await Clients.Group(userConnection.Room)
    //                 .SendAsync("ReceiveMessage", $"from {userConnection.User}", message);
    //         }
    //         else
    //         {
    //             Console.WriteLine($"C# Unable to send message. Clients is null.");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine($"C# Unable to send message. UserConnection not found for ConnectionId: {Context.ConnectionId}");
    //     }
    // }

    

    public async Task SendCard(string message)
    {
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            Console.WriteLine($"C# Sending card from {userConnection.User} to group {userConnection.Room}: {message}");
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveCard", $"from {userConnection.User}", message);
        }
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        _userConnections[Context.ConnectionId] = userConnection;//fixing bug
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
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
        Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
            $"{userConnection.User} has left the room {userConnection.Room}");
        SendConnectedUsers(userConnection.Room);

        return base.OnDisconnectedAsync(exception);
    }
}