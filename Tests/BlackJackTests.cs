namespace WebApi.Tests;
using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output
using WebApi.Common.LyssiePlayerOrder;

public class BlackJackTests {
  private readonly ITestOutputHelper output;
  public BlackJackTests(ITestOutputHelper output) {
    this.output = output; 
  }
  [Trait("testing", "rn")]
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
[Trait("testing", "rn1")]
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

    // public bool TakeBet(string player, int amt){
    //     Players[player].CurrentBet = amt;
    //     return true;
    // }
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