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
    UnoGameSim UnoSim = new UnoGameSim(this.output);
    UnoSim.Simulate();
  }
  // [Fact]
  // [Category ("Sim")]
  // [Trait ("SimGame", "TexasHoldEm")]
  // public void RunTexasSim() {
  //   TexasGameSim TexasSim = new TexasGameSim(this.output);
  //   TexasSim.Simulate();
  // }
    [Fact]
  [Category ("Sim")]
  [Trait ("SimGame", "BlackJack")]
  public void RunBlackJackSim() {
    BlackJackGameSim BlackJackSim = new BlackJackGameSim(this.output);
    BlackJackSim.Simulate();
  }
}
