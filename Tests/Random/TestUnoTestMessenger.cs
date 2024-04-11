using System.ComponentModel;
using Tests.Messengers;
using Xunit.Abstractions;

namespace Tests.Random;

[Trait("Category", "Random")]
public class TestUnoTestMessenger {
  public ITestOutputHelper Output;
  public TestUnoTestMessenger(ITestOutputHelper output) {
    this.Output = output;
  }
  [Fact]
  [Trait("Output", "Required")]
  public async void TestMessenger() {
    UnoTestMessenger messenger = new UnoTestMessenger(Output);
    await messenger.SendFrontendJson( new List<string>() {"123", "456"}, "test json...");
  }
}