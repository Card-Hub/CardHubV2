using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Hubs.HubMessengers;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

// stubbed out

public class UnoMessenger : iUnoMessenger {
  IHubContext<BaseHub> Context;
  public UnoMessenger(IHubContext<BaseHub> context) {
    this.Context = context;
  }

// potentially lots of people will need to know this
  public async Task SendFrontendJson(List<string> connStrs, string json)
  {
    Console.WriteLine($"UnoMessenger sent to the frontend: \n {json}");
    foreach (var connStr in connStrs) {
      Context.Clients.Client(connStr).SendAsync("ReceiveJson", json);
    }
  }

// only the gameboard needs to know this
  public async Task SendFrontendTimerSet(int time)
  {
    Console.WriteLine($"UnoMessenger told the frontend the timer reset to {time} seconds");
  }

// potentially lots of people will need to know this
  public async Task SendFrontendError(List<string> connStrs, string error)
  {
    Console.WriteLine($"UnoMessenger told the frontend there's an error: {error}");
    foreach (var connStr in connStrs) {
      await Context.Clients.Client(connStr).SendAsync("ReceiveError", error);
    }
  }
    public void Log(string message) {
      Console.WriteLine(message);
    }
}