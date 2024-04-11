using WebApi.Common;
using WebApi.Models;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests.Random;

[Trait("Category", "Random")]
public class UnoCardModLyssieTests {
  ITestOutputHelper Output;
  public UnoCardModLyssieTests(ITestOutputHelper output) {
    Output = output;
  }
  [Fact]
  public void TestCardInit() {
    // some random card
    var cardBlue1 = new UnoCardModLyssie(1, UnoColorLyssie.Blue, UnoValueLyssie.One);
    JToken jtokenBlue1 = JToken.FromObject(cardBlue1);
    JToken jtokenBlue1Expected = JToken.FromObject(new {Id=1, Color="Blue", Value="One"});
    Assert.True(JToken.DeepEquals(jtokenBlue1, jtokenBlue1Expected));
    // a special card
    var cardWildDrawFour = new UnoCardModLyssie(100, UnoColorLyssie.Black, UnoValueLyssie.WildDrawFour);
    JToken jtokenWildDrawFour = JToken.FromObject(cardWildDrawFour);
    JToken jtokenWildDrawFourExpected = JToken.FromObject(new {Id=100, Color="Black", Value="Wild Draw Four"});
    Assert.True(JToken.DeepEquals(jtokenWildDrawFour, jtokenWildDrawFourExpected));
  }
}