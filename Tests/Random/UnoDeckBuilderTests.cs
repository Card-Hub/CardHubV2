using WebApi.Common;
using WebApi.Models;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Tests.Random;

[Trait ("Category", "Random")]
public class UnoDeckBuilderTests {
  public ITestOutputHelper Output;
  public UnoDeckBuilderTests(ITestOutputHelper output) {
    Output = output;
  }
  [Fact]
  public void TestDeckBuildDefault() {
    var builder = new UnoDeckBuilderLyssie();
    // no skip alls
    var unoSettings = new UnoSettingsLyssie();
    IDeckLyssie<UnoCardModLyssie> unoDeck = builder.Build(unoSettings);
    string deckJson = JsonConvert.SerializeObject(unoDeck, Formatting.Indented);
    // it worked when i looked at the json output
    // typing the expected value of that would be awful tho i'm not doing that
    // so just check the count
    Assert.Equal(108, unoDeck.GetCards().Count);
  }
  [Fact]
  public void TestBuildWithSkipAlls() {
    var builder = new UnoDeckBuilderLyssie();
    // with skip alls
    var unoSettings2 = new UnoSettingsLyssie();
    unoSettings2.UseSkipAll = true;
    IDeckLyssie<UnoCardModLyssie> unoDeckWithSkipAlls = builder.Build(unoSettings2);
    string deckJsonWithSkipAlls = JsonConvert.SerializeObject(unoDeckWithSkipAlls, Formatting.Indented);
    Assert.Equal(116, unoDeckWithSkipAlls.GetCards().Count);
  }
}