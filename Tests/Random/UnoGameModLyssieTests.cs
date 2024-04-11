using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Common;
using Newtonsoft.Json;
using Xunit.Abstractions;
using WebApi.GameLogic.LyssieUno;
using Tests.Messengers;

namespace Tests.Random;

[Trait("Category", "Random")]
[Trait("Output", "Required")]
[Trait("Testing", "rn")]
public class UnoGameModLyssieTests {
  public ITestOutputHelper Output;
  public UnoGameModLyssieTests(ITestOutputHelper output) {
    Output = output;
  }
  [Fact]
  public async void TestCreateGame() {
    var messenger = new UnoTestMessenger(Output);

    var deckBuilder = new UnoDeckBuilderLyssie();
    var game = new UnoGameModLyssie(messenger, deckBuilder); 
    game.AddPlayer("Lyssie", "123");
    game.AddPlayer("Alex", "456");
    game.AddPlayer("Rubi", "789");
    game.AddPlayer("Liam", "1111");
    game.AddPlayer("Juno", "69");
    game.AddPlayer("Han", "0000");
    game.SetAvatar("123", "Lyssie.png");
    game.SetAvatar("69", "Fairy.png");
    Output.WriteLine(game.GetGameState());
    await game.StartGame();
    Output.WriteLine(game.GetGameState());
  }
}