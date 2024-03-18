namespace Tests;
using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output
using System.ComponentModel;

using Tests.GameSims;
public class GameSimTests {
  public readonly ITestOutputHelper output;
  public GameSimTests(ITestOutputHelper output) {
    this.output = output;
  }
  [Fact]
  [Category ("Sim")]
  [Trait ("SimGame", "Uno")]
  public void RunUnoSim() {
    output.WriteLine("12345");
    UnoGameSim UnoSim = new UnoGameSim(this.output);
    UnoSim.Simulate();
    Assert.True(true);
  }
}