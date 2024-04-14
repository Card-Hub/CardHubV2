using WebApi.Common;
using Newtonsoft.Json;
using Xunit.Abstractions;
using WebApi.Models;
using Newtonsoft.Json.Linq;

namespace Tests.RandomTests;

[Trait("Category", "RandomTests")]
public class UnoCardTests {
  public ITestOutputHelper Output;
  public UnoCardTests(ITestOutputHelper output) {
    Output = output;
  }
  [Fact]
  public void UnoCardJsonTests() {
    UnoCard unoCard = new UnoCard();
    // before changes
    JToken unoCardJtoken = JToken.FromObject(unoCard);
    JToken blankUnoCardJToken = JToken.FromObject(new {Id = 0, Value = "", Color = ""});
    Assert.True(JToken.DeepEquals(unoCardJtoken, blankUnoCardJToken));
    
    // changes occur
    unoCard.Id = 1;
    unoCard.Color = "Red";
    unoCard.Value = "Skip";
    unoCardJtoken = JToken.FromObject(unoCard);
    JToken redSkipUnoCardJToken = JToken.FromObject(new {Id = 1, Value = "Skip", Color = "Red"});
    Assert.True(JToken.DeepEquals(unoCardJtoken, redSkipUnoCardJToken));
    Assert.False(JToken.DeepEquals(unoCardJtoken, blankUnoCardJToken));
    
    // can even be in a different order
    JToken differentOrderRedSkipToken = JToken.FromObject(new {Color = "Red", Value = "Skip", Id = 1});
    Assert.True(JToken.DeepEquals(unoCardJtoken, differentOrderRedSkipToken));
  }
}