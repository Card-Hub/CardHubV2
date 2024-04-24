using Xunit.Abstractions;
using WebApi.Hubs.HubMessengers;
using WebApi.Models;

namespace Tests.Messengers;

public class BlackJackTestMessenger : iUnoMessenger {//sub for sending jsons to frontend
  public ITestOutputHelper Output;//using itestoutputhelper becuse its what we use to print in the unit test
  public BlackJackTestMessenger(ITestOutputHelper outputHelper) { this.Output = outputHelper; }

  public async Task SendFrontendJson(List<string> connStrs, string json)
  {
    Output.WriteLine($"* BlackJackTestMessenger sent the frontend: \n {json}");
  }
  public async Task SendFrontendError(List<string> connStrs, string error)
  {
    Output.WriteLine($"BlackJackTestMessenger sent the frontend: \n  {error}");
  }

  public async Task SendFrontendTimerSet(int time)
  {
    Output.WriteLine($"BlackJackTestMessenger told the frontend the timer reset to {time} seconds");
  }
    public void Log(string message) {
      Output.WriteLine(message);
    }
}