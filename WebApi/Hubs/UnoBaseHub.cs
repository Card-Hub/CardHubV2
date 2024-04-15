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
  
  public async Task SelectUno(UnoGameStorage unoGameStorage)
    {
        if (!_userConnections.TryGetValue(Context.ConnectionId, out var userConnection)) return;
        // store settings
        //unoGameStorage.InitializeUnoSettingsForRoom();
        unoGameStorage.SetSetting(userConnection.Room, "UseSkipAll", true);
        unoGameStorage.BuildGame(userConnection.Room);
        var game = unoGameStorage.GetGame(userConnection.Room);
        // reguster gameboard to game - do this first!
        unoRoomToGames[userConnection.Room].GameboardConnStr = userConnection.ConnectionId;
        Console.WriteLine("Uno selected");
        await Clients.User(userConnection.ConnectionId).SendAsync("Log", "Uno selected");
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
    public async Task SelectWild(UnoGameStorage unoGameStorage, string connStr, string color) {
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
      await game.SelectWild(connStr, colorEnum);
    }
}