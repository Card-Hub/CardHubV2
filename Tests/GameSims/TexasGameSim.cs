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
    List<Tuple<string, string>> testPlayers = new();
    testPlayers.Add(new Tuple<string, string>("A", "111"));
    testPlayers.Add(new Tuple<string, string>("B", "222"));
    testPlayers.Add(new Tuple<string, string>("C", "333"));
    //game.AddPlayer("D", "444");
    foreach (Tuple<string, string> playerTuple in testPlayers) {
      game.AddPlayer(playerTuple.Item1, playerTuple.Item2);
    }
    output.WriteLine("Game state upon adding A, B, C, D is: " + game.GetGameState());
    game.StartGame();
    foreach (Tuple<string, string> playerTuple in testPlayers) { // playerTuple is (name, connStr)
      output.WriteLine($"Turn starting for {playerTuple.Item1}");
      game.Call(playerTuple.Item2);
    }
  }
}