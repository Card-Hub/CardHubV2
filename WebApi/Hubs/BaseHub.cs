using WebApi.GameLogic;
using WebApi.Models;

namespace WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public async Task SendGameType(string gameType, GameService gameService, UnoGameStorage unoGameStorage) {
      if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) {return;}

      if (userConnection.UserType == UserType.Gameboard) {
        gameService.GameTypeFromRoomCode[userConnection.Room] = gameType;
        switch (gameType.ToLower()) {
          case "une":
            unoGameStorage.BuildGame(userConnection.Room);
            var game = unoGameStorage.GetGame(userConnection.Room);
            game.GameboardConnStr = userConnection.ConnectionId;
            await Clients.Client(game.GameboardConnStr).SendAsync("ReceiveJson", game.GetGameState());
            break;
          default:
            Console.WriteLine("UNKNOWN GAME TYPE SENT??");
            break;
        }
        //break;
      }
    }

    public async Task JoinRoom(GameService gameService, UnoGameStorage unoGameStorage, UserConnection userConnection)
    {
        Console.WriteLine("Join Room");
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        userConnection.ConnectionId = Context.ConnectionId;
        _userConnections[Context.ConnectionId] = userConnection;

        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
            new UserMessage
            {
                User = userConnection.User,
                Message = $"{userConnection.User} has joined the room {userConnection.Room}"
            });
        //await SendConnectedUsers(userConnection.Room);
        switch (userConnection.UserType)
        {
            case UserType.Player:
              switch (gameService.GameTypeFromRoomCode[userConnection.Room].ToLower()) {
                case "une":
                  Console.WriteLine($"ADDING UNE PLAYER {userConnection.User}");
                  var game = unoGameStorage.GetGame(userConnection.Room);
                  game.AddPlayer(userConnection.User, userConnection.ConnectionId);
                  Console.WriteLine(game.GameboardConnStr);
                  await Clients.Groups(game.GameboardConnStr).SendAsync("ReceiveJson", game.GetGameState());
                  break;
                default:
                  Console.WriteLine("ADDING PLAYER FOR UNKNOWN GAME??");
                  break;
              }
              break;
            case UserType.Gameboard:
              break;
            default:
              Console.WriteLine("JOINING ROOM WITH INVALID USER TYPE?");
              break;
        }
        //      if ()
        //        //_game.AddPlayer(userConnection.ConnectionId);
        //        unoGameStorage.GetGame(userConnection.Room).AddPlayer(userConnection.User, userConnection.ConnectionId);
        //        break;
           
        //    default:
        //        throw new ArgumentOutOfRangeException();
        //}
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
                        User = userConnection.User,
                        Message = $"It's not your turn, {userName}"
                    });
            }
        }   
    }
public async Task SendAvatar(GameService gameService, UnoGameStorage unoGameStorage, string avatar) {
      if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
          if (gameService.GameTypeFromRoomCode[userConnection.Room].ToLower() == "une") { // game is uno
            var game = unoGameStorage.GetGame(userConnection.Room);
            string gameboardStr = game.GameboardConnStr;
            game.SetAvatar(userConnection.ConnectionId, avatar);
            //// send to the gameboard that the avatar was sent
            List<LobbyUser> lobbyUsers = new();
            game.GetActivePlayers();
            foreach (var player in game.GetActivePlayers()) {
              lobbyUsers.Add(new LobbyUser(player.Name, player.Avatar));
            }
            await Clients.Group(userConnection.Room).SendAsync("ReceiveAvatars", JsonConvert.SerializeObject(lobbyUsers));
            await Clients.Group(userConnection.Room).SendAsync("ReceiveJson", game.GetGameState());
          }
        }
    }
    public async Task DrawCard(GameService gameService, UnoGameStorage unoGameStorage) {
      if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
          if (gameService.GameTypeFromRoomCode[userConnection.Room].ToLower() == "une") { // game is uno
            var game = unoGameStorage.GetGame(userConnection.Room);
            string gameboardStr = game.GameboardConnStr;
            await game.DrawCard(userConnection.ConnectionId);
            //// send to the gameboard that the avatar was sent
            //List<LobbyUser> lobbyUsers = new();
            //game.GetActivePlayers();
            //foreach (var player in game.GetActivePlayers()) {
            //  lobbyUsers.Add(new LobbyUser(player.Name, player.Avatar));
            //}
            //await Clients.Client(gameboardStr).SendAsync("ReceiveAvatars", JsonConvert.SerializeObject(lobbyUsers));
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

    public async Task StartGame(UnoGameStorage unoGameStorage, GameService gameService)
    {
      if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
          var gameType = gameService.GameTypeFromRoomCode[userConnection.Room];
          switch (gameType.ToLower()) {
            case "une":
              Console.WriteLine("Une game started");
              var game = unoGameStorage.GetGame(userConnection.Room);
              await game.StartGame();
              break;
            default:
              Console.WriteLine("STARTED GAME BUT IT'S AN INVALID GAME TYPE");
              break;
          }
        }
    }
    public async Task PlayCard(string cardJson, UnoGameStorage unoGameStorage, GameService gameService)
    {
      if (_userConnections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
          var gameType = gameService.GameTypeFromRoomCode[userConnection.Room];
          switch (gameType.ToLower()) {
            case "une":
              Console.WriteLine("Card was played!");
              Console.WriteLine($"Card: {cardJson}");
              var game = unoGameStorage.GetGame(userConnection.Room);
              var cardJToken = JToken.Parse(cardJson);
              var cardColor = UnoColorLyssie.Black;
              var cardValue = UnoValueLyssie.One;
              // color switch
              switch (cardJToken["Color"].ToString().ToLower()) {
                case "red":
                  cardColor = UnoColorLyssie.Red;
                  break;
                case "blue":
                  cardColor = UnoColorLyssie.Blue;
                  break;
                case "yellow":
                  cardColor = UnoColorLyssie.Yellow;
                  break;
                case "green":
                  cardColor = UnoColorLyssie.Green;
                  break;
                case "black":
                  cardColor = UnoColorLyssie.Black;
                  break;
                default:
                  Console.WriteLine("INVALID CARD COLOR PLAYED???");
                  break;
              }
              switch (cardJToken["Value"].ToString().ToLower()) {
                case "0":
                  cardValue = UnoValueLyssie.Zero;
                  break;
                case "1":
                  cardValue = UnoValueLyssie.One;
                  break;
                case "2":
                  cardValue = UnoValueLyssie.Two;
                  break;
                case "3":
                  cardValue = UnoValueLyssie.Three;
                  break;
                case "4":
                  cardValue = UnoValueLyssie.Four;
                  break;
                case "5":
                  cardValue = UnoValueLyssie.Five;
                  break;
                case "6":
                  cardValue = UnoValueLyssie.Six;
                  break;
                case "7":
                  cardValue = UnoValueLyssie.Seven;
                  break;
                case "8":
                  cardValue = UnoValueLyssie.Eight;
                  break;
                case "9":
                  cardValue = UnoValueLyssie.Nine;
                  break;
                case "draw two":
                  cardValue = UnoValueLyssie.DrawTwo;
                  break;
                case "reverse":
                  cardValue = UnoValueLyssie.Reverse;
                  break;
                case "skip":
                  cardValue = UnoValueLyssie.Skip;
                  break;
                case "skip all":
                  cardValue = UnoValueLyssie.SkipAll;
                  break;
                case "wild draw four":
                  cardValue = UnoValueLyssie.WildDrawFour;
                  break;
                case "wild":
                  cardValue = UnoValueLyssie.Wild;
                  break;
                default:
                  Console.WriteLine("INVALID CARD VALUE PLAYED???");
                  break;
              }
              var card = new UnoCardModLyssie(int.Parse(cardJToken["Id"].ToString()), cardColor, cardValue);
              await game.PlayCard(userConnection.ConnectionId, card);
              break;
            default:
              Console.WriteLine("PLAYED CARD BUT IT'S AN INVALID GAME TYPE");
              break;
          }
        }
    }

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
                User = userConnection.User,
                Message = $"{userConnection.User} has left the room {userConnection.Room}"
            });
        SendConnectedUsers(userConnection.Room);

        return base.OnDisconnectedAsync(exception);
    }
}