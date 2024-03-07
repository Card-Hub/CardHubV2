us

namespace Tests;

public class UnitTest1
{
    public UnoGame unoGame;
    
    [Fact]
    public void UnoTest() {
        unoGame = new UnoGame();
        unoGame.AddPlayer("Lyssie");
        unoGame.AddPlayer("Rubi");
        unoGame.AddPlayer("Liam");
        unoGame.AddPlayer("Alex");
    }
    //[Fact]
    //public void Test1()
    //{
    //  List<string> players = new List<string>();
    //  players.Add("Lyssie");
    //  players.Add("Rubi");
    //  players.Add("Liam");
    //  players.Add("Alex");
    //  Xunit.Assert.Equal(players, unoGame.GetPlayerList());
    //  Xunit.Assert.True(true);
    //}
    //[Fact]
    //public void Test2() {
    //  Xunit.Assert.Equal(1,1);
    //}
}