using WebApi.GameLogic;
using WebApi.Models;
using Xunit.Abstractions;

// to run simulation:
// cd to CardHubV2 (or whatever your parent folder for WebApi is)
// dotnet test --filter "SimGame=Uno" --logger "console;verbosity=detailed"

namespace Tests.GameSims;

public class TexasGameSim {
  private readonly ITestOutputHelper output;
  public TexasGameSim(ITestOutputHelper output) {
    this.output = output;
  }
  public void Simulate() {
    TexasHoldEmGame game = new TexasHoldEmGame();
    
  }
}