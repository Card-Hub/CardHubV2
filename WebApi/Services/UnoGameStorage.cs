using WebApi.GameLogic.LyssieUno;
using WebApi.GameLogic;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Common;

// Stores games and settings for specified rooms.
public class UnoGameStorage {
  private IHubContext<BaseHub> HubContext;
  public Dictionary<string, UnoGameModLyssie> GamesFromRoomCodes { get; private set; } = new();
  private Dictionary<string, UnoSettingsLyssie> SettingsFromRoomCodes = new();
  private UnoDeckBuilderLyssie deckBuilder = new UnoDeckBuilderLyssie();
  public UnoGameStorage(IHubContext<BaseHub> hubContext) {
    this.HubContext = hubContext;
  }
  public void BuildGame(string roomCode) {
    Console.WriteLine("In buildgame");
    InitializeUnoSettingsForRoom(roomCode);
    var game = new UnoGameModLyssie(new UnoMessenger(HubContext), deckBuilder);
    game.ChangeSettings(SettingsFromRoomCodes[roomCode]);
    GamesFromRoomCodes[roomCode] = game;
    Console.WriteLine("In buildgame 2");
  }
  public void SetSetting(string roomCode, string setting, bool newValue) {
    if (!SettingsFromRoomCodes.ContainsKey(roomCode)) {
      InitializeUnoSettingsForRoom(roomCode);
    }
    switch (setting) {
      case "UseSkipAll":
        SettingsFromRoomCodes[roomCode].UseSkipAll = newValue;
        GamesFromRoomCodes[roomCode].ChangeSettings(SettingsFromRoomCodes[roomCode]);
        Console.WriteLine($"Changed UseSkipAll to value {newValue}");
        break;
      default:
        Console.WriteLine($"Attempted to change unknown setting {setting} to value {newValue}");
        break;
    }
  }
  
  private void InitializeUnoSettingsForRoom(string roomCode) {
    Console.WriteLine($"Initializing uno settings for room {roomCode}");
    var settings = new UnoSettingsLyssie();
    SettingsFromRoomCodes[roomCode] = settings;
  }
  public UnoGameModLyssie GetGame(string roomCode) {
    return GamesFromRoomCodes[roomCode];
  }
}