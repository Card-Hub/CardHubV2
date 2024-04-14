using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Common;


using WebApi.Models;
using Xunit.Abstractions;

namespace Tests.RandomTests;


[Trait("Category", "RandomTests")]

public class CardPlayerLyssieTests {
  private ITestOutputHelper Output;
  public CardPlayerLyssieTests(ITestOutputHelper output) {
    this.Output = output;
  }
  [Fact]
  public void TestCardPlayerInit() {
    CardPlayerLyssie<UnoCard> lyssie = new("Lyssie", "123");
    Assert.Equal("Lyssie", lyssie.Name);
    Assert.Equal("", lyssie.Avatar);
    Assert.Equal(new List<UnoCard>(), lyssie.Hand);
    Assert.False(lyssie.Afk);
    //Output.WriteLine(JsonConvert.SerializeObject(lyssie));
    // compare Jtokens
    JToken blankPlayer = JToken.FromObject(new {Name = "Lyssie", Avatar = "", Afk = false, Hand = new List<UnoCard>()});
    JToken lyssieJtoken = JToken.FromObject(lyssie);
    Assert.True(JToken.DeepEquals(lyssieJtoken, blankPlayer));
  }
}