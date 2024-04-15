namespace WebApi.Tests;
using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output
using WebApi.Common.LyssiePlayerOrder;
using System.Security.AccessControl;

public class BlackJackTests {
  private readonly ITestOutputHelper output;
  public BlackJackTests(ITestOutputHelper output) {
    this.output = output; 
  }
  [Trait("unit-test", "test1")]
  [Fact]
  public void TestAddRemovePlayer()//works for remove and add
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
    game.TakeBet("Lyssie", 1);
    game.TakeBet("Rubi", 2);
    game.TakeBet("Alex", 3);
    Assert.Equal(controlNames, game.GetPlayerList());
    output.WriteLine(game.GetGameState());
  }
[Trait("unit-test", "test2")]
[Fact]
public void TestInitDeck()
{
  BlackJackGame game = new BlackJackGame();
  game.InitDeck();
  foreach (var card in game.Deck.GetCards())
  {
      Console.WriteLine(card.Value); // Assuming Card class overrides ToString() method
  }
  Assert.Equal(52, game.Deck.GetCards().Count());
}

[Trait("unit-test", "test3")]
[Fact]
public void TestPlayerDrawCard(){
  BlackJackGame game = new BlackJackGame();
  game.AddPlayer("Liam");
  game.InitDeck();
  game.DrawCard("Liam");
  Assert.Equal(51, game.Deck.GetCards().Count());
  output.WriteLine(game.GetGameState());
}

[Trait("unit-test", "test4")]
[Fact]
public void TestPlayerScoring()
{
  BlackJackGame game = new BlackJackGame();
  game.AddPlayer("Liam");
  StandardCard ace = new StandardCard(0, "Clubs", "A");
  StandardCard king = new StandardCard(0, "Clubs", "K");
  game.GivePlayerCard("Liam", ace);
  game.GivePlayerCard("Liam", ace);
  game.GivePlayerCard("Liam", ace);
  game.GivePlayerCard("Liam", ace);
  Assert.Equal(14, game.GetPlayerScoreFromGame("Liam"));
  game.GivePlayerCard("Liam", king);
  Assert.Equal(14, game.GetPlayerScoreFromGame("Liam"));
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