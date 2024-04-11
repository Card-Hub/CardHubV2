using WebApi.Models;

namespace WebApi.Hubs.HubMessengers;

public interface iUnoMessenger {
  public Task SendFrontendJson(List<string> connStrs, string json);
  public Task SendFrontendTimerSet(int time);
  //public Task SendFrontendTimerPause();
  //public Task SendFrontendTimerStop();
  public Task SendFrontendError(List<string> connStrs, string error);
}