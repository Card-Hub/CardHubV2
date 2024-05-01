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
[Trait("Game", "Texas")]
public class TexasHoldEmTests {
  public ITestOutputHelper Output;
  public TexasHoldEmTests(ITestOutputHelper output) {
    Output = output;
  }
  [Fact]
  public void TestNormalizeInt() {
    UnoTestMessenger messenger = new UnoTestMessenger(Output);
    TexasHoldEmGame game = new TexasHoldEmGame(messenger);
    Assert.Equal(0,game.NormalizeInt(5, 5));
    Assert.Equal(1,game.NormalizeInt(6, 5));
    Assert.Equal(4,game.NormalizeInt(-1, 5));
  }
}