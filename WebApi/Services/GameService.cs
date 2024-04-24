using WebApi.GameLogic.LyssieUno;
using WebApi.GameLogic;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Common;
public class GameService
{
  private IHubContext<BaseHub> HubContext;
  public Dictionary<string, string> GameTypeFromRoomCode { get; set; } = new();
  public GameService(IHubContext<BaseHub> hubContext) {
    this.HubContext = hubContext;
  }
}