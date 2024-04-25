using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output
using WebApi.Common.LyssiePlayerOrder;
using System.Security.AccessControl;
using WebApi.Hubs.HubMessengers;
using Tests.Messengers;
using WebApi.Hubs;

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
    BlackJackGame game = new BlackJackGame(messenger, "abcd");
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
    BlackJackGame game = new BlackJackGame(messenger, "abcd");
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
    BlackJackGame game = new BlackJackGame(messenger, "abcd");
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
    BlackJackGame game = new BlackJackGame(messenger, "abcd");
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
  [Fact]
  [Trait("Integration_test", "test_1")]
  public async void TestCreateGame() {
    var messenger = new BlackJackTestMessenger(output);
    var game = new BlackJackGame(messenger, "abcd");
    game.StartGame();
    game.AddPlayer("Alex", "01");
    game.AddPlayer("Liam", "02");
    game.AddPlayer("Lyssie", "03");
    game.AddPlayer("Rubi", "04");
    game.StartRound();
    game.TakeBet("01", 42);
    game.TakeBet("02", 43);
    game.TakeBet("03", 44);
    game.TakeBet("04", 45);

    output.WriteLine(game.GetGameState());
  }
}