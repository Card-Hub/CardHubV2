using WebApi.GameLogic;
using WebApi.Models;

namespace WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebApi.GameLogic.LyssieUno;

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

class LobbyUser {
  public string Name { get; set; }
  public string Avatar { get; set; }
  public LobbyUser(string name, string avatar) {
    Name = name;
    Avatar = avatar;
  }
};

public partial class BaseHub : Hub
{
    private const string BotUser = "Bot";
    private IDictionary<string, UserConnection> _userConnections;
    private UnoGameMod _game;

    public BaseHub(IDictionary<string, UserConnection> userConnections, UnoGameMod game)
    {
        _userConnections = userConnections;
        _game = game;
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Connected");
        return base.OnConnectedAsync();
    }

    public async Task JoinRoom(UnoGameStorage unoGameStorage, UserConnection userConnection)
    {
        Console.WriteLine("Join Room");
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        userConnection.ConnectionId = Context.ConnectionId;
        _userConnections[Context.ConnectionId] = userConnection;
        Console.WriteLine(userConnection.ToString());
        // lyssie
        Console.WriteLine("!!");

        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
            new UserMessage
            {
                User = BotUser,
                Message = $"{userConnection.User} has joined the room {userConnection.Room}"
            });
        await SendConnectedUsers(userConnection.Room);

        switch (userConnection.UserType)
        {
            case UserType.Player:
                //_game.AddPlayer(userConnection.ConnectionId);
                unoGameStorage.GetGame(userConnection.Room).AddPlayer(userConnection.User, userConnection.ConnectionId);
                break;
            case UserType.Gameboard:
                unoGameStorage.BuildGame(userConnection.Room);
                unoGameStorage.GetGame(userConnection.Room).GameboardConnStr = userConnection.ConnectionId;
                break;
            default:
                throw new ArgumentOutOfRangeException();
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

    public async Task SendCard(UnoCardMod card)
    {
        if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            var userName = userConnection.ConnectionId;
            if (await _game.PlayCard(userConnection.ConnectionId!, card))
            {
                await Clients.Client(userConnection.ConnectionId!).SendAsync("PopCard", card.ExtractValue());
                await Clients.Client(_game.Gameboard).SendAsync("ReceiveCard", userConnection.User, card.ExtractValue());
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage",
                    new UserMessage
                    {
                        User = BotUser,
                        Message = $"It's not your turn, {userName}"
                    });
            }
        }   
    }
    public async Task SendAvatar(UnoGameStorage unoGameStorage, string avatar) {
      if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
          if (true) { // game is uno
            var game = unoGameStorage.GetGame(userConnection.Room);
            string gameboardStr = game.GameboardConnStr;
            game.SetAvatar(userConnection.ConnectionId, avatar);
          //// send to the gameboard that the avatar was sent
          List<LobbyUser> lobbyUsers = new();
          game.GetActivePlayers();
          foreach (var player in game.GetActivePlayers()) {
            lobbyUsers.Add(new LobbyUser(player.Name, player.Avatar));
          }
          await Clients.Client(gameboardStr).SendAsync("ReceiveAvatars", JsonConvert.SerializeObject(lobbyUsers));
          //await Clients.Client(gameboardStr).SendAsync("Log", game.GetGameState());
          }
        }
    }

    //public async Task DrawCard()
    //{
    //    if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;

    //    var userName = userConnection.ConnectionId!;
    //    var card = _game.DrawCard(userName);

    //    if (card is not null)
    //    {
    //        await Clients.Caller.SendAsync("ReceiveCard", "Gameboard", card.ExtractValue());
    //    }
    //}

    //public async Task StartGame()
    //{
    //    await _game.StartGame();
        
    //    var roomConnections = _userConnections.Values.Where(x => x.Room == _userConnections[Context.ConnectionId].Room);

    //    foreach (var conn in roomConnections.Where(x => x.UserType == UserType.Player))
    //    {
    //        var userName = conn.ConnectionId!;
    //        var hand = _game.GetPlayerHand(userName);
    //        await Clients.Client(conn.ConnectionId!).SendAsync("StartedGame", hand);
    //    }
    //}

    public Task SendConnectedUsers(string room)
    {
        var users = _userConnections.Values.Where(x => x.Room == room && x.UserType == UserType.Player)
            .Select(x => x.User);
        //Console.WriteLine("in send connected: ", users);
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