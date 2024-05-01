namespace Tests;
using WebApi.Models;
using WebApi.GameLogic;
using Xunit.Abstractions; // for output
using System.ComponentModel;

using Tests.GameSims;
using Tests.Messengers;

public class GameSimTests {
  public readonly ITestOutputHelper output;
  public GameSimTests(ITestOutputHelper output) {
    this.output = output;
  }
  //[Fact]
  //[Category ("Sim")]
  //[Trait ("SimGame", "Uno")]
  //public void RunUnoSim() {
  //  UnoGameSim UnoSim = new UnoGameSim(this.output);
  //  UnoSim.Simulate();
  //}
  [Fact]
  [Category ("Sim")]
  [Trait ("SimGame", "Texas")]
  public void RunTexasSim() {
    TexasGameSim texasGameSim= new TexasGameSim(output);
    texasGameSim.Simulate();
  }
}