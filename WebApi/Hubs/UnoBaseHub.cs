using WebApi.GameLogic;
using WebApi.Models;

namespace WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.GameLogic.LyssieUno;


public partial class BaseHub : Hub
{
  private Dictionary <string, UnoGameModLyssie> unoRoomToGames = new();
  private List<string> unoRoomCodes = new();
  
  public async Task SelectUno(UnoGameModLyssieBuilder unoGameBuilderService)
    {
        if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",
            new UserMessage
            {
                User = "TestBot",
                Message = $"{userConnection.User} selected to play {userConnection.Room}"
            });
          await Clients.User(userConnection.ConnectionId).SendAsync("ReceiveMessage",
            new UserMessage
            {
                User = "TestBot 2",
                Message = $"{userConnection.ConnectionId} selected to play {userConnection.Room} 2"
            });
        // create game
        UnoDeckBuilderLyssie deckBuilderLyssie = new UnoDeckBuilderLyssie();
        // create game
        unoRoomToGames[userConnection.Room] = unoGameBuilderService.BuildGame(deckBuilderLyssie);
        // reguster gameboard to game - do this first!
        unoRoomToGames[userConnection.Room].GameboardConnStr = userConnection.ConnectionId;
        Console.WriteLine($"Gameboard conn str: {userConnection.ConnectionId}");
        // add player; should send to frontend that a test player was added
        unoRoomToGames[userConnection.Room].AddPlayer("TESTPLAYER", userConnection.ConnectionId);

    }
}