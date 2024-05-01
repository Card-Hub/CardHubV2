using Microsoft.AspNetCore.SignalR;
using WebApi.Common;

namespace WebApi.Hubs;


public record PlayerMessage
{
    public required string Name { get; set; }
    public required string Message { get; set; }
}

public record BaseConnection
{
    public required string Room { get; set; }
    public string? Name { get; set; }

    public BaseConnection() { }
}

public record BasePlayer
{
    public required string ConnectionId { get; set; }
    public required string Name { get; set; }
    public required string Avatar { get; set; }

    public BasePlayer() { }
}

public class BaseRoom
{
    public List<BasePlayer> Users { get; } = [];
    public string Gameboard { get; }
    private static readonly List<string> AvatarNames = ["lyssie", "ruby", "oli", "femaleJuno", "alex", "andy", "liam", "juno", "pocky", "star", "fairy", "dinoNugget1", "dinoNugget2", "dinoNugget3", "dinoNugget4", "amongusNugget"];

    public BaseRoom(string gameboard)
    {
        Gameboard = gameboard;
    }
    
    public IEnumerable<string> GetAllIds()
    {
        var all = Users.Select(user => user.ConnectionId).ToList();
        all.Add(Gameboard);

        return all;
    }
    
    public IEnumerable<BasePlayer> GetAvatars() => Users;
    
    public bool SetAvatar(string connectionId, string avatar)
    {
        var user = Users.FirstOrDefault(user => user.ConnectionId == connectionId);
        if (user is null) return false;
        
        user.Avatar = avatar;
        return true;
    }
    
    public string GetRandomAvailableAvatar()
    {
        var random = new Random();
        var availableAvatars = AvatarNames.Except(Users.Select(user => user.Avatar)).ToArray();
        var index = random.Next(availableAvatars.Length);
        return availableAvatars.ElementAt(index);
    }
}

public interface IBaseClient
{
    Task ReceiveMessage(PlayerMessage message);
    Task ReceiveAvatars(IEnumerable<BasePlayer> avatars);
}

public class BaseHub : Hub<IBaseClient>
{
    private const string Name = "Name";
    private const string Room = "Room";
    
    private IDictionary<string, BaseRoom> _rooms;
    
    public BaseHub(IDictionary<string, BaseRoom> rooms)
    {
        _rooms = rooms;
    }
    
    public async Task JoinRoom(BaseConnection connection)
    {
        // Joinable by both gameboard and player
        // Both require a room id to join, only player requires a name
        
        var roomId = connection.Room;
        var name = connection.Name;
        
        if (TryGetRoom(out var baseRoom, roomId))
        {
            if (name is null) return;

            baseRoom.Users.Add(new BasePlayer
            {
                ConnectionId = ContextId,
                Name = name,
                Avatar = baseRoom.GetRandomAvailableAvatar()
            });
            
            Context.Items[Name] = name;
        }
        else
        {
            _rooms.Add(roomId, new BaseRoom(ContextId));
        }
        
        Context.Items[Room] = roomId;

        await Groups.AddToGroupAsync(ContextId, roomId);
        await SendAvatar();
    }
    
    public async Task SendMessage(string message)
    {
        await Clients.Group(ContextRoomId).ReceiveMessage(
            new PlayerMessage
            {
                Name = ContextUserName,
                Message = message
            });
    }
    
    public async Task SendAvatar(string? avatar = null)
    {
        if (!TryGetRoom(out var baseRoom)) return;
        if (avatar != null && !baseRoom.SetAvatar(ContextId, avatar)) return;
        
        await Clients.Group(ContextRoomId).ReceiveAvatars(baseRoom.GetAvatars());
    }

    
    #region Helpers
    
    private bool TryGetRoom(out BaseRoom baseRoom, string? roomId = null)
    {
        roomId ??= ContextRoomId;
        return _rooms.TryGetValueAs(roomId, out baseRoom);
    }
    
    private string ContextId => Context.ConnectionId;
    
    private string ContextUserName => Context.Items.TryGetValueAs(Name, out string name)
            ? name
            : throw new InvalidOperationException("Name not found");
    
    private string ContextRoomId =>
        Context.Items.TryGetValueAs(Room, out string roomId)
            ? roomId
            : throw new InvalidOperationException("Room id not found");

    #endregion
}