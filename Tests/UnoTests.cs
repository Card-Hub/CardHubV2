namespace WebApi.Tests;
using WebApi.Models;
//using WebApi.GameLogic;
using Xunit.Abstractions; // for output

// Setup/Teardown not implemented
// Notes on output:
// Use output.WriteLine instead of Console.WriteLine for output
  // to run via cli, use:
  // cd CardHubV2 (or folder just above WebApi)
  //   dotnet test --logger "console;verbosity=detailed"
  // or, if you don't care about any console output:
  //   dotnet test

public class UnitTest1
{
    // for output
    private readonly ITestOutputHelper output;
    public UnitTest1(ITestOutputHelper output)
    {
      this.output = output;
    }
    // tests need either [Fact] or [Theory] above them
    // google [Theory] or just use [Fact]

    [Fact]
    public void TestBlankUnoCardValues()
    {
      UnoCard uc = new UnoCard();
      var blankDict = new Dictionary<string, string>
        {
            { "Id", "0" },
            { "Value", "" },
            { "Color", "" }
        };
      Assert.Equal(blankDict, uc.GetAttributes());
    }
    [Fact]
    public void TestICardUnoConstructor() {
      iCard uc = new UnoCard();
      var blankDict = new Dictionary<string, string>
        {
            { "Id", "0" },
            { "Value", "" },
            { "Color", "" }
        };
      Assert.Equal(blankDict, uc.GetAttributes());
      uc.SetAttribute("Id", "1");
      Assert.Equal("1", uc.GetAttribute("Id"));
    }
    [Fact]
    public void TestUnoCardString() {
      UnoCard uc = new UnoCard();
      Assert.Equal("{ id: 0, value: , color:  }", uc.ToString());
      UnoCard uc2 = new UnoCard(15, "Skip", "blue");
      Assert.Equal("{ id: 15, value: Skip, color: blue }", uc2.ToString());
    } 
    [Fact]
    public void TestHalfFinishedUnoCardValues() {
      UnoCard uc = new UnoCard();
    }
}