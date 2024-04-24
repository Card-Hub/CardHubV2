using WebApi.GameLogic.LyssieUno;
using WebApi.GameLogic;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Common;

// Stores games and settings for specified rooms.
public class BlackJackGameStorage {
  private IHubContext<BaseHub> HubContext;
  public Dictionary<string, BlackJackGame> GamesFromRoomCodes { get; private set; } = new();
  public BlackJackGameStorage(IHubContext<BaseHub> hubContext) {
    this.HubContext = hubContext;
  }
  public void BuildGame(string roomCode, string gameboardConnStr) {
    Console.WriteLine("In buildgame");
    var game = new BlackJackGame(new UnoMessenger(HubContext), gameboardConnStr);
    GamesFromRoomCodes[roomCode] = game;
  }


  public BlackJackGame GetGame(string roomCode) {
    return GamesFromRoomCodes[roomCode];
  }
}