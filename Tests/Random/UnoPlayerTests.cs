using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Common;


using WebApi.Models;
using Xunit.Abstractions;

namespace Tests.Random;

[Trait("Category", "Random")]

public class UnoPlayerTests {
  private ITestOutputHelper Output;
  public UnoPlayerTests(ITestOutputHelper output) {
    this.Output = output;
  }
  [Fact]
  public void TestCardPlayerInit() {
    UnoPlayerLyssie lyssie = new("Lyssie", "12345");
    Assert.Equal("Lyssie", lyssie.Name);
    Assert.Equal("", lyssie.Avatar);
    Assert.Equal(new List<UnoCardModLyssie>(), lyssie.Hand);
    Assert.False(lyssie.Afk);
    // compare Jtokens
    JToken blankPlayer = JToken.FromObject(new {Name = "Lyssie", Avatar = "", Afk = false, Hand = new List<UnoCard>(), PickingWildColor = false});
    JToken lyssieJtoken = JToken.FromObject(lyssie);
    //Output.WriteLine(JsonConvert.SerializeObject(lyssie, Formatting.Indented));
    Assert.True(JToken.DeepEquals(lyssieJtoken, blankPlayer));
    
    // ensure that if one thing is changed, it's no longer the same
    lyssie.PickingWildColor = true;
    lyssieJtoken = JToken.FromObject(lyssie);
    Assert.False(JToken.DeepEquals(lyssieJtoken, blankPlayer));
    
  }
}