using Xunit.Abstractions;
using WebApi.Hubs.HubMessengers;
using WebApi.Models;

namespace Tests.Messengers;

public class UnoTestMessenger : iUnoMessenger {
  public ITestOutputHelper Output;
  public UnoTestMessenger(ITestOutputHelper outputHelper) { this.Output = outputHelper; }

  public async Task SendFrontendJson(List<string> connStrs, string json)
  {
    Output.WriteLine($"* UnoTestMessenger sent the frontend: \n {json}");
  }
  public async Task SendFrontendError(List<string> connStrs, string error)
  {
    Output.WriteLine($"UnoTestMessenger sent the frontend: \n  {error}");
  }



  public async Task SendFrontendTimerSet(int time)
  {
    Output.WriteLine($"UnoTestMessenger told the frontend the timer reset to {time} seconds");
  }
}