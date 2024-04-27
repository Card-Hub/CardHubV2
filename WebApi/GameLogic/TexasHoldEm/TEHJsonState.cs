using WebApi.Common;
using WebApi.Models;


namespace WebApi.GameLogic.TexasHoldEmStates;
public class TEHJsonState {
  // the order of these things are important as it directly corresponds to the order of the stuff in the serialiezed json
  public string GameType { get; set; }
  public string State { get; set; }
  public string ButtonPlayer { get; set; }
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
    if (State == "Not Started") {
      ButtonPlayer = "";
      LittleBlindPlayer = "";
      BigBlindPlayer = "";
      CurrentPlayer = "";
    }
    else {
      ButtonPlayer = game.Players[game.ButtonPlayer].Name; // 
      LittleBlindPlayer = game.Players[game.LittleBlindPlayer].Name;
      BigBlindPlayer = game.Players[game.BigBlindPlayer].Name;
      CurrentPlayer = game.Players[game.PlayerOrder.GetCurrentPlayer()].Name;
    }
    LittleBlindAmt = game.LittleBlindAmt;
    BigBlindAmt = game.BigBlindAmt;
    CurrentBet = game.CurrentBet;
    Board = game.Board;
    // players
    Players.Clear();
    List<string> activePlayers = game.PlayerOrder.GetPlayers(Common.LyssiePlayerOrder.LyssiePlayerStatus.Active);
    string name = "ghghg";
    for (int i = 0; i < activePlayers.Count; i++) {
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
