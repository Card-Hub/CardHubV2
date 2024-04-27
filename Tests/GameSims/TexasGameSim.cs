using Tests.Messengers;
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
    output.WriteLine("Started Texas Hold Em Game Sim.");
    UnoTestMessenger messenger = new UnoTestMessenger(output);
    TexasHoldEmGame game = new TexasHoldEmGame(messenger);
    output.WriteLine("Game state upon a fresh game is: " + game.GetGameState());
    game.AddPlayer("A", "111");
    game.AddPlayer("B", "222");
    game.AddPlayer("C", "333");
    //game.AddPlayer("D", "444");
    output.WriteLine("Game state upon adding A, B, C, D is: " + game.GetGameState());
    game.StartGame();
  }
}