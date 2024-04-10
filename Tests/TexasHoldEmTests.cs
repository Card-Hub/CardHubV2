namespace Tests;
using WebApi.Models;
using WebApi.GameLogic;
using WebApi.Common.LyssiePlayerOrder;
using WebApi.GameLogic.TexasHoldEmStates;
using Xunit.Abstractions; // for output
using System.ComponentModel;
using Newtonsoft.Json;

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
    //setUp();
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
  [Fact]
  public void TestGetGameState() {

  }
  [Fact]
  public void TestTEHGameState() {
    //setUp();
    TexasHoldEmGame game = new();
    TEHJsonState tehJsonState= new TEHJsonState();
    string tehJson = JsonConvert.SerializeObject(tehJsonState);
    //output.WriteLine(tehJson);
    //string cardJson = JsonConvert.SerializeObject(new StandardCard(4, "Spades", "7"));
    //output.WriteLine(cardJson);
    //output.WriteLine(game.GetGameState());
  }
  [Fact]
  public void TestPokerPlayerJsonDeserialize() {
    PokerPlayer lyssie = new PokerPlayer("Lyssie");
    lyssie.Pot = 20;
    lyssie.Folded = true;
    string lyssieStr = JsonConvert.SerializeObject(lyssie, Formatting.Indented);
    Assert.Equivalent(lyssie, JsonConvert.DeserializeObject<PokerPlayer>(lyssieStr), strict:true);
    lyssie.Folded = false;
  }
  [Fact]
  public void TestGameSetup() {
    //setUp();
    this.game = new();
    this.game.AddPlayer("Lyssie");
    this.game.AddPlayer("Juno");
    //output.WriteLine(game.GetGameState());
  }
  [Fact]
  public void TestStartGame() {
    setUp();
    this.game = new();
    this.game.AddPlayer("Lyssie");
    this.game.AddPlayer("Juno");
    this.game.StartGame();
    //output.WriteLine(game.GetGameState());
  }
  [Fact]
  [Trait("testing", "rn")]
  public void TestRunGame() {
    setUp();
    output.WriteLine("Testing a game.");
    this.game = new();
    this.game.AddPlayer("A");
    this.game.AddPlayer("B");
    this.game.AddPlayer("C");
    this.game.AddPlayer("D");
    this.game.AddPlayer("E");
    this.game.AddPlayer("F");
    this.game.SetPlayerStatus("E", LyssiePlayerStatus.Spectator);
    output.WriteLine(this.game.GetGameState());
    output.WriteLine("GAME STARTED");
    this.game.StartGame();
    output.WriteLine(this.game.GetGameState());
    // Invalid calls at first
    bool bool1 = this.game.Call("A");
    Assert.False(bool1);
    bool bool2 = this.game.Call("B");
    Assert.False(bool2);
    bool bool3 = this.game.Call("C");
    Assert.False(bool3);
    // Valid calls
    bool bool4 = this.game.Call("D");
    Assert.True(bool4);
    output.WriteLine(this.game.GetGameState());
    bool bool5 = this.game.Call("F");
    Assert.True(bool5);
    bool bool6 = this.game.Call("A");
    Assert.True(bool6);
    bool bool7 = this.game.Call("B");
    Assert.True(bool7);
    // Invalid call in the middle
    bool bool8 = this.game.Call("B");
    output.WriteLine(bool7.ToString());
    Assert.False(bool8);
    // Test that all the bets are the same
    List<string> activePlayers1 = game.PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
    string name = "";
    bool allBetsAre2 = true;
    for (int i = 0; i < game.GetPlayerList().Count(); i++) {
      name = activePlayers1[i];
      allBetsAre2 = allBetsAre2 && game.Players[name].CurrentBet == 2;
    }
    Assert.True(allBetsAre2);
    output.WriteLine("NEW PHASE?");
    output.WriteLine(this.game.GetGameState());

  }
}