namespace Tests;
using WebApi.Models;
using WebApi.GameLogic;
using WebApi.Common.LyssiePlayerOrder;
using Xunit.Abstractions; // for output
using System.ComponentModel;

[Trait("Category", "TEH")]
public class TexasHoldEmTests {
  private readonly ITestOutputHelper output;
  private TexasHoldEmGame game;
  public TexasHoldEmTests(ITestOutputHelper output) {
    this.output = output; 
  }
  private void setUp() { // there is a way to do this properly. this is not it.
    output.WriteLine("Running Texas Hold Em Test...");
    
  }
  [Fact]
  [Trait("State", "NotStarted")]
  public void TestAddPlayersNotStarted() {
    setUp();
    TexasHoldEmGame game = new();
    game.AddPlayer("Alex");
    game.AddPlayer("Liam");
    game.AddPlayer("Lyssie");
    game.AddPlayer("Juno");
    game.AddPlayer("Rubi");

    string players = "";
    foreach (string player in game.GetPlayerList()) {
      players += player + ",";
    }
    Assert.Equal("Alex,Liam,Lyssie,Juno,Rubi,", players);
    // testing spectators
    game.PlayerOrder.SetPlayerStatus("Juno", LyssiePlayerStatus.Afk);
    string spectators = "";
    foreach (string spectator in game.GetSpectatorList()) {
      spectators += spectator + ",";
    }
    Assert.Equal("", spectators);

  }
  [Fact]
  // Test that a game can be started
  public void TestGameStart() {
    
  }
}