using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Common;
using Newtonsoft.Json;
using Xunit.Abstractions;
using WebApi.GameLogic.LyssieUno;
using Tests.Messengers;

namespace Tests.RandomTests;

[Trait("Category", "RandomTests")]
[Trait("Output", "Required")]
public class UnoGameModLyssieTests {
  public ITestOutputHelper Output;
  public UnoGameModLyssieTests(ITestOutputHelper output) {
    Output = output;
  }
  [Fact]
  [Trait("Testing", "rn")]
  public async void TestCreateGame() {
    var messenger = new UnoTestMessenger(Output);
    var deckBuilder = new UnoDeckBuilderLyssie();
    var game = new UnoGameModLyssie(messenger, deckBuilder); 
    game.GameboardConnStr = "GGGG";
    game.AddPlayer("Alex", "01");
    game.AddPlayer("Liam", "02");
    game.AddPlayer("Lyssie", "03");
    game.AddPlayer("Rubi", "04");
    //game.AddPlayer("Juno", "69");
    //game.AddPlayer("Han", "0000");
    game.SetAvatar("01", "Lyssie.png");
    game.SetAvatar("03", "Fairy.png");
    //Output.WriteLine(game.GetGameState());
    Output.WriteLine("!!!GAME STARTING!!!!");
    await game.StartGame();
    Output.WriteLine(game.GetGameState());
    // game sim lmao
    string currentPlayerConnStr = "";
    string playerName = "";
    int turnNum = 0;
    var random = new Random();
    double thresholdToTryToPlayUnplayableCard = .95;
    int maxTurns = 150;
    while (game.Winner == "" && turnNum < maxTurns) {
      currentPlayerConnStr = game.GetCurrentPlayer();
      if (!game.SomeoneNeedsToSelectWildColor) {
        playerName = game._players[currentPlayerConnStr].Name;
        Output.WriteLine($"\t\t*TURN {turnNum}: {playerName}'S TURN*");
        Output.WriteLine($"\t\t[connStr: {currentPlayerConnStr}]");
        Output.WriteLine($"## Current game state: {game._CurrentColor} {game._discardPile.Peek().ValueEnum.ToString()}");
        Output.WriteLine($"[their hand:]");
        for (int i = 0; i < game.GetPlayerHand(currentPlayerConnStr).Count; i++) {
          Output.WriteLine($"\t{game.GetPlayerHand(currentPlayerConnStr)[i].Color} {game.GetPlayerHand(currentPlayerConnStr)[i].Value}");
        } 
        // \n {JsonConvert.SerializeObject(game.GetPlayerHand(currentPlayerConnStr), Formatting.None)}");
      
        // try to play a card
        bool playedACard = false;
        foreach (UnoCardModLyssie card in game.GetPlayerHand(currentPlayerConnStr)) {
          // if card can be played, play it, else try and play it if the boolean is big enough, to test them not doing things right, else draw a card
          if (game.CanCardBePlayed(card)) {
            await game.PlayCard(game.GetCurrentPlayer(), card);
            playedACard = true;
            break;
          }
          else {
            // randomly try to play a card when you can't
            if (random.NextDouble() > thresholdToTryToPlayUnplayableCard) {
              await game.PlayCard(game.GetCurrentPlayer(), card);
            }
          }
        }
        if (!playedACard) {
          await game.DrawCard(game.GetCurrentPlayer());
        }
        turnNum++;
      }
      else {
        if (game._players[currentPlayerConnStr].GetHand().Count > 0) {
          Output.WriteLine("Picking wild...");
          UnoColorLyssie colorToPick = game._players[currentPlayerConnStr].GetHand()[0].ColorEnum;
          await game.SelectWild(currentPlayerConnStr, colorToPick);
        }
      }
      //Output.WriteLine(game.GetGameState());
    }
    Output.WriteLine("GAME TEST END");
    Output.WriteLine($"Game took {turnNum} turns.");
    Output.WriteLine($"Winner: {game.Winner}");
  }
  [Fact]
  [Trait("ye", "ye")]
  public async void TestCanCardBePlayed() {
    var messenger = new UnoTestMessenger(Output);
    var deckBuilder = new UnoDeckBuilderLyssie();
    UnoGameModLyssie game = new(messenger, deckBuilder);
    game.AddPlayer("Lyssie", "123");
    game.AddPlayer("Juno", "69");
    game.AddPlayer("Han", "0000");
    await game.StartGame();
    UnoCardModLyssie blue2 = new UnoCardModLyssie(0, UnoColorLyssie.Blue, UnoValueLyssie.Two);
    UnoCardModLyssie blue5 = new UnoCardModLyssie(0, UnoColorLyssie.Blue, UnoValueLyssie.Five);
    UnoCardModLyssie red2 = new UnoCardModLyssie(0, UnoColorLyssie.Red, UnoValueLyssie.Two);
    UnoCardModLyssie red9 = new UnoCardModLyssie(0, UnoColorLyssie.Red, UnoValueLyssie.Nine);
    UnoCardModLyssie greenSkip = new UnoCardModLyssie(0, UnoColorLyssie.Green, UnoValueLyssie.Skip);
    UnoCardModLyssie wild = new UnoCardModLyssie(0, UnoColorLyssie.Black, UnoValueLyssie.Wild);

    game._discardPile.Push(blue2);
    game._CurrentColor = UnoColorLyssie.Blue;
    Assert.True(game.CanCardBePlayed(blue2));
    Assert.True(game.CanCardBePlayed(blue5));
    Assert.True(game.CanCardBePlayed(red2));
    Assert.False(game.CanCardBePlayed(red9));
    Assert.False(game.CanCardBePlayed(greenSkip));
    Assert.True(game.CanCardBePlayed(wild));
  }
}