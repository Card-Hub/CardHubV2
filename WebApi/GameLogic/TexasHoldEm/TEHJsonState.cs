using WebApi.Common;
using WebApi.Models;


namespace WebApi.GameLogic.TexasHoldEmStates;
public class TEHJsonState {
  // the order of these things are important as it directly corresponds to the order of the stuff in the serialiezed json
  public string GameType { get; set; }
  public string State { get; set; }
  public string LittleBlindPlayer { get; set; }
  public string BigBlindPlayer { get; set; }
  public int LittleBlindAmt { get; set; }
  public int BigBlindAmt { get; set; }
  public int CurrentBet { get; set; }
  public int TotalPot { get; set; }
  public string Dealer { get; set; }
  public string CurrentPlayer { get; set; }
  public string LastPlayerWhoRaised { get; set; }
  public List<StandardCard> Board { get; set; }
  public List<PokerPlayer> Players { get; set; }
  public void Update(TexasHoldEmGame game) {
    //GameType = "Texas Hold Em";
    State = game.State.ToString();
    LittleBlindPlayer = game.LittleBlindPlayer;
    BigBlindPlayer = game.BigBlindPlayer;
    LittleBlindAmt = game.LittleBlindAmt;
    BigBlindAmt = game.BigBlindAmt;
    CurrentBet = game.CurrentBet;
    Dealer = game.Dealer;
    if (State != "Not Started") {
      CurrentPlayer = game.PlayerOrder.GetCurrentPlayer();
    }
    else {
      CurrentPlayer = "n/a";
    }
    Board = game.Board;
    // players
    Players.Clear();
    List<PokerPlayer> pokerPlayers = new List<PokerPlayer>();
    List<string> activePlayers = game.PlayerOrder.GetPlayers(Common.LyssiePlayerOrder.LyssiePlayerStatus.Active);
    string name = "";
    for (int i = 0; i < game.PlayerOrder.GetPlayers(Common.LyssiePlayerOrder.LyssiePlayerStatus.Active).Count; i++) {
      name = activePlayers[i];
      Players.Add(game.Players[name]);
    }
    this.LastPlayerWhoRaised = game.LastPlayerWhoRaised;
    this.TotalPot = game.TotalPot;
  }
  public TEHJsonState() {
    GameType = "Texas Hold Em";
    State = "";
    LittleBlindPlayer = "";
    BigBlindPlayer = "";
    LittleBlindAmt = 0;
    BigBlindAmt = 0;
    CurrentBet = 0;
    Dealer = "";
    CurrentPlayer = "";
    LastPlayerWhoRaised = "";
    Board = new();
    Players = new();
    TotalPot = 0;
  }
}
