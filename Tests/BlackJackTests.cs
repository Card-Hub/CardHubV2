using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output
using WebApi.Common.LyssiePlayerOrder;
using System.Security.AccessControl;
using WebApi.Hubs.HubMessengers;
using Tests.Messengers;

namespace Tests;

public class BlackJackTests {
  private readonly ITestOutputHelper output;
  public BlackJackTests(ITestOutputHelper output) {
    this.output = output;
  }
  [Trait("unit-test", "test1")]
  [Fact]
  public void TestAddRemovePlayer()//works for remove and add
  {
    var messenger = new UnoTestMessenger(output);
    List<string> controlNames = new List<string>();
    // controlNames.Add("Liam");
    controlNames.Add("1234");
    controlNames.Add("12345");
    controlNames.Add("123456");    
    BlackJackGame game = new BlackJackGame(messenger);
    game.AddPlayer("Liam", "123");
    game.AddPlayer("Lyssie","1234");
    game.AddPlayer("Rubi","12345");
    game.AddPlayer("Alex","123456");
    game.RemovePlayer("123");
    game.TakeBet("1234", 1);
    game.TakeBet("12345", 2);
    game.TakeBet("123456", 3);
    Assert.Equal(controlNames, game.GetPlayerList());
    output.WriteLine(game.GetGameState());
  }
[Trait("unit-test", "test2")]
[Fact]
public void TestInitDeck()
{
  var messenger = new UnoTestMessenger(output);
  BlackJackGame game = new BlackJackGame(messenger);
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
  var messenger = new UnoTestMessenger(output);
  BlackJackGame game = new BlackJackGame(messenger);
  game.AddPlayer("Liam","123");
  game.InitDeck();
  game.state="DrawingCards";
  game.DrawCard("123");
  Assert.Equal(51, game.Deck.GetCards().Count());
  output.WriteLine(game.GetGameState());
}

[Trait("unit-test", "test4")]
[Fact]
public void TestPlayerScoring()
{
  var messenger = new UnoTestMessenger(output);
  BlackJackGame game = new BlackJackGame(messenger);
  game.AddPlayer("Liam","123");
  StandardCard ace = new StandardCard(0, "Clubs", "A");
  StandardCard king = new StandardCard(0, "Clubs", "K");
  game.GivePlayerCard("123", ace);
  game.GivePlayerCard("123", ace);
  game.GivePlayerCard("123", ace);
  game.GivePlayerCard("123", ace);
  Assert.Equal(14, game.GetPlayerScoreFromGame("123"));
  game.GivePlayerCard("123", king);
  Assert.Equal(14, game.GetPlayerScoreFromGame("123"));
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