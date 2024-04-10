namespace WebApi.Tests;
using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output

public class BlackJackTests {
  private readonly ITestOutputHelper output;
  private void setUp() { // there is a way to do this properly. this is not it.
    
  }
  public BlackJackTests(ITestOutputHelper output) {
    this.output = output; 
  }
  [Trait("testing", "rn")]
  [Fact]
  public void TestGetPlayerList()
  {
    List<string> controlNames = new List<string>();
    // controlNames.Add("Liam");
    controlNames.Add("Lyssie");
    controlNames.Add("Rubi");
    controlNames.Add("Alex");    
    BlackJackGame game = new BlackJackGame();
    game.AddPlayer("Liam");
    game.AddPlayer("Lyssie");
    game.AddPlayer("Rubi");
    game.AddPlayer("Alex");
    game.RemovePlayer("Liam");
    Assert.Equal(controlNames, game.GetPlayerList());
    output.WriteLine(game.GetGameState());
  }


  // Test that a game can be started
  // [Fact]
  // public void TestStartGame() {
  //   setUp();
  //   this.game = new();
  //   this.game.AddPlayer("Lyssie");
  //   this.game.AddPlayer("Juno");
  //   this.game.StartGame();
  //   output.WriteLine(game.GetGameState());
  // }
}