namespace Tests;
using WebApi.GameLogic;
using WebApi.Common.LyssiePlayerOrder;
using Xunit.Abstractions; // for output
using WebApi.Models;

[Trait("Category", "LyssiePlayerOrder")]
public class LyssiePlayerOrderTests {
  private readonly ITestOutputHelper output;
  private LyssiePlayerOrder PlayerOrder;
  public LyssiePlayerOrderTests(ITestOutputHelper output) {
    this.output = output; 
    PlayerOrder = new();
  }
  private void setUp() {
    //output.WriteLine("...LPO test...");
    PlayerOrder = new();
  }
  [Fact]
  private void AddPlayerTest() {
    setUp();
    List<string> blankList = new();
    Assert.Equal(blankList, PlayerOrder.GetAllPlayers());
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Active));
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator));
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Afk));
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.NotAfk));
    List<string> AllPlayers = new List<string> {"Lyssie", "Liam"};
    PlayerOrder.AddPlayer("Lyssie");
    PlayerOrder.AddPlayer("Liam");
    Assert.Equal(AllPlayers, PlayerOrder.GetAllPlayers());
    Assert.Equal(AllPlayers, PlayerOrder.GetPlayers(LyssiePlayerStatus.Active));
    Assert.Equal(AllPlayers, PlayerOrder.GetPlayers(LyssiePlayerStatus.NotAfk));
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Afk));
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator));
  }
  [Fact]
  private void SetPlayerStatusTest() {
    setUp();
    PlayerOrder.AddPlayer("Rubi");
    PlayerOrder.AddPlayer("Alex");
    List<string> blankList = new();
    List<string> Players1 = new List<string> {"Rubi", "Alex"};
    List<string> Players2 = new List<string> {"Alex", "Rubi"};
    List<string> RubiList = new List<string> {"Rubi"};
    List<string> AlexList = new List<string> {"Alex"};

    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.Afk);
    Assert.Equal(PlayerOrder.GetPlayers(LyssiePlayerStatus.Active), Players1);
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator));
    Assert.Equal(RubiList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Afk));
    Assert.Equal(AlexList, PlayerOrder.GetPlayers(LyssiePlayerStatus.NotAfk));

    PlayerOrder.SetPlayerStatus("Alex", LyssiePlayerStatus.Active);
    Assert.Equal(PlayerOrder.GetPlayers(LyssiePlayerStatus.Active), Players1);

    PlayerOrder.SetPlayerStatus("Alex", LyssiePlayerStatus.Spectator);
    Assert.Equal(AlexList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator));
    Assert.Equal(RubiList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Active));
    Assert.Equal(RubiList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Afk));
    Assert.Equal(AlexList, PlayerOrder.GetPlayers(LyssiePlayerStatus.NotAfk));

    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.Spectator);
    Assert.Equal(blankList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Active));
    Assert.Equal(RubiList, PlayerOrder.GetPlayers(LyssiePlayerStatus.Afk));
    Assert.Equal(Players2, PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator));
    Assert.Equal(AlexList, PlayerOrder.GetPlayers(LyssiePlayerStatus.NotAfk));
  }
  [Fact]
  private void TestIsPlayerActive() {
    setUp();
    PlayerOrder.AddPlayer("Rubi");
    PlayerOrder.AddPlayer("Lyssie");
    Assert.True(PlayerOrder.IsPlayerActive("Rubi"));
    Assert.True(PlayerOrder.IsPlayerActive("Lyssie"));

    PlayerOrder.SetPlayerStatus("Lyssie", LyssiePlayerStatus.Afk);
    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.NotAfk);
    Assert.True(PlayerOrder.IsPlayerActive("Lyssie"));
    Assert.True(PlayerOrder.IsPlayerActive("Rubi"));

    PlayerOrder.SetPlayerStatus("Lyssie", LyssiePlayerStatus.Spectator);
    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.Spectator);
    Assert.False(PlayerOrder.IsPlayerActive("Lyssie"));
    Assert.False(PlayerOrder.IsPlayerActive("Rubi"));

    PlayerOrder.SetPlayerStatus("Lyssie", LyssiePlayerStatus.NotAfk);
    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.Afk);
    Assert.False(PlayerOrder.IsPlayerActive("Lyssie"));
    Assert.False(PlayerOrder.IsPlayerActive("Rubi"));
  }
  [Fact]
  private void TestIsPlayerAfk() {
    setUp();
    PlayerOrder.AddPlayer("Rubi");
    PlayerOrder.AddPlayer("Lyssie");
    Assert.False(PlayerOrder.IsPlayerAfk("Rubi"));
    Assert.False(PlayerOrder.IsPlayerAfk("Lyssie"));

    PlayerOrder.SetPlayerStatus("Lyssie", LyssiePlayerStatus.Afk);
    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.NotAfk);
    Assert.True(PlayerOrder.IsPlayerAfk("Lyssie"));
    Assert.False(PlayerOrder.IsPlayerAfk("Rubi"));

    PlayerOrder.SetPlayerStatus("Lyssie", LyssiePlayerStatus.Spectator);
    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.Spectator);
    Assert.True(PlayerOrder.IsPlayerAfk("Lyssie"));
    Assert.False(PlayerOrder.IsPlayerAfk("Rubi"));

    PlayerOrder.SetPlayerStatus("Lyssie", LyssiePlayerStatus.NotAfk);
    PlayerOrder.SetPlayerStatus("Rubi", LyssiePlayerStatus.Afk);
    Assert.False(PlayerOrder.IsPlayerAfk("Lyssie"));
    Assert.True(PlayerOrder.IsPlayerAfk("Rubi"));
  }
  [Fact]
  private void TestIsPlayerActiveIfPlayerNotInGame() {
    setUp();
    PlayerOrder.AddPlayer("Lyssie");
    Assert.Throws<ArgumentException>(() => PlayerOrder.IsPlayerActive("Juno"));
  }
  [Fact]
  private void TestIsPlayerAfkIfPlayerNotInGame() {
    setUp();
    Assert.Throws<ArgumentException>(() => PlayerOrder.IsPlayerAfk("Juno"));
  }
  [Fact]
  private void TestRemovePlayer() {
    setUp();
    PlayerOrder.AddPlayer("Alex");
    PlayerOrder.AddPlayer("Juno");
    PlayerOrder.AddPlayer("Liam");
    PlayerOrder.AddPlayer("Lyssie");
    PlayerOrder.AddPlayer("Rubi");
    // player is removed from player lists?
    Assert.Contains("Juno", PlayerOrder.GetAllPlayers());
    Assert.Contains("Juno", PlayerOrder.GetPlayers(LyssiePlayerStatus.Active));
    PlayerOrder.RemovePlayer("Juno");
    Assert.DoesNotContain("Juno", PlayerOrder.GetAllPlayers());
    Assert.DoesNotContain("Juno", PlayerOrder.GetPlayers(LyssiePlayerStatus.Active));
    // player can't be checked for afk or active?
    Assert.Throws<ArgumentException>(() => PlayerOrder.IsPlayerAfk("Juno"));
    Assert.Throws<ArgumentException>(() => PlayerOrder.IsPlayerActive("Juno"));
    // current player int goes down by 1
    // if the player's index is above the current player index?
  }
  [Fact]
  private void TestGetCurrentPlayer() {
    setUp();
    PlayerOrder.AddPlayer("Lyssie");
    PlayerOrder.AddPlayer("Liam");
    PlayerOrder.AddPlayer("Juno");
    Assert.Equal("Lyssie", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.NextTurn();
    Assert.Equal("Liam", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.NextTurn();
    Assert.Equal("Juno", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.NextTurn();
    Assert.Equal("Lyssie", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.DirectionInt = Direction.Backward;
    Assert.Equal("Lyssie", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.NextTurn();
    Assert.Equal("Juno", PlayerOrder.GetCurrentPlayer());
  }
  [Fact]
  private void TestShufflePlayers() {
    setUp();
    PlayerOrder.AddPlayer("Alex");
    PlayerOrder.AddPlayer("Andy");
    PlayerOrder.AddPlayer("Carmen");
    PlayerOrder.AddPlayer("Han");
    PlayerOrder.AddPlayer("Kirin");
    PlayerOrder.AddPlayer("Juno");
    PlayerOrder.AddPlayer("Liam");
    PlayerOrder.AddPlayer("Lyssie");
    PlayerOrder.AddPlayer("Rubi");
    List<string> PlayersExpected = new() {"Alex", "Andy", "Carmen", "Han", "Kirin", "Juno", "Liam", "Lyssie", "Rubi"};
    PlayerOrder.ShufflePlayers();
    Assert.NotEqual(PlayersExpected, PlayerOrder.GetAllPlayers());
    Assert.Equal(PlayersExpected.Count, PlayerOrder.GetAllPlayers().Count);
  }
  [Fact]
  private void TestSetNextPlayer() {
    setUp();
    PlayerOrder.AddPlayer("Player1");
    PlayerOrder.AddPlayer("Player2");
    PlayerOrder.AddPlayer("Player3");
    PlayerOrder.AddPlayer("Player4");
    PlayerOrder.AddPlayer("Player5");
    PlayerOrder.SetPlayerStatus("Player5", LyssiePlayerStatus.Spectator);
    Assert.Equal("Player1", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.SetNextPlayer("Player3");
    Assert.Equal("Player1", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.NextTurn();
    Assert.Equal("Player3", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.NextTurn();
    Assert.Equal("Player4", PlayerOrder.GetCurrentPlayer());
    PlayerOrder.SetNextPlayer("Player4");
    PlayerOrder.NextTurn();
    Assert.Equal("Player4", PlayerOrder.GetCurrentPlayer());
  }
  [Fact]
  private void TestGetActivePlayersInOrder() {
    setUp();
    PlayerOrder.AddPlayer("Player1");
    PlayerOrder.AddPlayer("Player2");
    PlayerOrder.AddPlayer("Player3");
    PlayerOrder.AddPlayer("Player4");
    PlayerOrder.AddPlayer("Player5");
    PlayerOrder.SetPlayerStatus("Player5", LyssiePlayerStatus.Spectator);
    List<string> inOrder = new() {"Player1", "Player2", "Player3", "Player4"};
    Assert.Equal(inOrder, PlayerOrder.GetActivePlayersInOrder());
    List<string> inOrder2 = new() {"Player2", "Player3", "Player4", "Player1"};
    PlayerOrder.NextTurn();
    Assert.Equal(inOrder2, PlayerOrder.GetActivePlayersInOrder());
    List<string> inOrder3 = new() {"Player3", "Player4", "Player1", "Player2"};
    PlayerOrder.NextTurn();
    Assert.Equal(inOrder3, PlayerOrder.GetActivePlayersInOrder());
    List<string> inOrder4 = new() {"Player3", "Player2", "Player1", "Player4"};
    PlayerOrder.DirectionInt = Direction.Backward;
    Assert.Equal(inOrder4, PlayerOrder.GetActivePlayersInOrder());
    // No impact from SetNextPlayer()
    PlayerOrder.SetNextPlayer("Player1");
    Assert.Equal(inOrder4, PlayerOrder.GetActivePlayersInOrder());
  }
}