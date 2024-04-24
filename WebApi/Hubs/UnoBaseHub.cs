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
  
  public async Task SelectUno(UnoGameStorage unoGameStorage, string a1, string a2)
    {
        if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
        Console.WriteLine(a1 + a2);
        // store settings
        unoGameStorage.BuildGame(userConnection.Room);
        //unoGameStorage.SetSetting(userConnection.Room, "UseSkipAll", true);
        var game = unoGameStorage.GetGame(userConnection.Room);
        Console.WriteLine("in select uno ");
        // reguster gameboard to game - do this first!
        game.GameboardConnStr = userConnection.ConnectionId;
        Console.WriteLine(unoGameStorage.GetGame(userConnection.Room).GameboardConnStr);
        Console.WriteLine("Uno selected");
        Clients.Client(userConnection.ConnectionId!).SendAsync("BELog","Uno selected");
        //Context.Clients.Client(game.GameboardConnStr).SendAsync("BELog", "BE Uno selected");
        Console.WriteLine("end select uno");
    }
    public async Task UnoStartGame(UnoGameStorage unoGameStorage)
    {
        if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
        //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage")
        var game = unoGameStorage.GetGame(userConnection.Room);
        await game.StartGame();
    }
    public async Task UnoDrawCard(UnoGameStorage unoGameStorage, string connStr) {
      if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
      string roomCode = userConnection.Room;
      UnoGameModLyssie game = unoGameStorage.GetGame(roomCode);
      await game.DrawCard(connStr);
    }
    public async Task UnoPlayCard(UnoGameStorage unoGameStorage, string connStr, string cardJson) {
      throw new NotImplementedException("Have to convert to card obj in backend");
      if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
      string roomCode = userConnection.Room;
      UnoGameModLyssie game = unoGameStorage.GetGame(roomCode);
      //await game.PlayCard(connStr, cardJson);
    }
    public async Task SelectColor(UnoGameStorage unoGameStorage, string color) {
      if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
      string roomCode = userConnection.Room;
      UnoGameModLyssie game = unoGameStorage.GetGame(roomCode);
      var colorEnum = UnoColorLyssie.Black;
      switch (color.ToLower()) {
        case "blue":
          colorEnum = UnoColorLyssie.Blue;
          break;
        case "green":
          colorEnum = UnoColorLyssie.Green;
          break;
        case "red":
          colorEnum = UnoColorLyssie.Red;
          break;
        case "yellow":
          colorEnum = UnoColorLyssie.Yellow;
          break;
        default:
          throw new ArgumentException("Invalid wild color selected");
      }
      await game.SelectWild(userConnection.ConnectionId, colorEnum);
    }
}