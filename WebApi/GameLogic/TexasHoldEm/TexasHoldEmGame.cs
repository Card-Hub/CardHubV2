using System.Runtime.CompilerServices;
using WebApi.Models;
using WebApi.GameLogic.TexasHoldEmStates;
using WebApi.Common;
using WebApi.Common.LyssiePlayerOrder;
using Newtonsoft.Json;
using System.Text.Json;
using WebApi.Hubs.HubMessengers;
using NetTopologySuite.Operation.Overlay;
namespace WebApi.GameLogic;

public class TexasHoldEmGame
{
  private TEHJsonState tehJsonState {get; set;}
  public string State {get; private set;}
  public LyssiePlayerOrder PlayerOrder;
  public Dictionary<string, PokerPlayer> Players;
  public bool SpectatorsOnly {get; set;}
  public StandardCardDeck Deck {get; set;}
  public int InitialPlayerPot {get; set;}
  public int LittleBlindAmt {get; set;}
  public int BigBlindAmt {get; private set;}
  public string LittleBlindPlayer {get; set;}
  public string BigBlindPlayer {get; set;}
  public string ButtonPlayer {get; set;}
  private int ButtonIndex {get; set;}
  public int CurrentBet {get; set;}
  public int MinIncrease {get; set;}
  public List<StandardCard> Board {get; set;}
  public int TotalPot {get; set;}
  public string LastPlayerWhoRaised {get; set;}
  public bool LastPlayerWhoRaisedCanStillGo = true;
  public string GameboardConnStr {get; set;}
  [JsonIgnore]
  private iUnoMessenger Messenger {get;set;}


  public TexasHoldEmGame(iUnoMessenger messenger) {
    this.PlayerOrder = new();
    this.Players = new();
    this.tehJsonState = new();
    this.Deck = new();
    this.Deck.Init52();
    this.State = "Not Started";
    this.LittleBlindAmt = 1;
    this.BigBlindAmt = 2;
    this.LittleBlindPlayer = "";
    this.BigBlindPlayer = "";
    this.ButtonPlayer = "";
    this.ButtonIndex = -1; // set to -1 so that we can += 1 it to be 0 in RoundStart
    this.TotalPot = 0;
    this.MinIncrease = LittleBlindAmt;
    this.Board = new();
    this.InitialPlayerPot = 100;
    this.LastPlayerWhoRaised = "";
    this.Messenger = messenger;
  }
  public bool AddPlayer(string playerName, string connStr)
  {
    if (!Players.ContainsKey(connStr)) {
        PlayerOrder.AddPlayer(connStr);
        Players[connStr] = new PokerPlayer(playerName, connStr);
        Messenger.SendFrontendJson(new List<string>() {GameboardConnStr}, GetGameState());
        return true;
      }
      return false;
  }

  

  public bool SetPlayerStatus(string connStr, LyssiePlayerStatus status) {
    switch (status) {
      case (LyssiePlayerStatus.Active):
        return PlayerOrder.SetPlayerStatus(connStr, LyssiePlayerStatus.Active);
      case (LyssiePlayerStatus.Spectator):
        return PlayerOrder.SetPlayerStatus(connStr, LyssiePlayerStatus.Spectator);
      case (LyssiePlayerStatus.Afk):
        return PlayerOrder.SetPlayerStatus(connStr, LyssiePlayerStatus.Afk);
      case (LyssiePlayerStatus.NotAfk):
        return PlayerOrder.SetPlayerStatus(connStr, LyssiePlayerStatus.NotAfk);
      default:
        throw new ArgumentException("Invalid status in SetPlayerStatus in TEHGame");
    }
  }
  public List<string> GetPlayerList()
  {
    return PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
  }
  public List<string> GetSpectatorList() {
    return this.PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator);
  }

  public async Task RemovePlayer(string connStr)
  {
    if (this.State == "Not Started") {
      // remove player from playerOrder
      this.PlayerOrder.RemovePlayer(connStr);
      // remove player from dict
      this.Players.Remove(connStr);
      await Messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
      //return true;
    }
    else {
      Messenger.Log("Removing player during this state isn't implemented yet!");
      throw new NotImplementedException();
    }
  }

  public void StartGame()
  { 
    if (this.State == "Not Started" && PlayerOrder.GetPlayers(LyssiePlayerStatus.Active).Count >= 2) {
      List<string> playerConnStrs = PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
      for (int i = 0; i < playerConnStrs.Count; i++) {
        // give people their money
        Players[playerConnStrs[i]].AmountOfMoneyLeft = InitialPlayerPot;
        Players[playerConnStrs[i]].CurrentlyPlaying = true;
      }
      Messenger.Log("Here in startgame");
      this.RoundStart();
    }
    else {
      Console.WriteLine("Cannot start game.");
    }
  }


  public bool ResetDeck()
  {
    this.Deck.ReclaimCards();
    return true;
  }

    public void RoundStart() {
    // reset, assign new button
    this.State = "Pre-Flop";
    do { // modify button until we find a player who is playing
      this.ButtonIndex += 1;
      // correct button index
      this.ButtonIndex = NormalizeInt(ButtonIndex, PlayerOrder.GetPlayers(LyssiePlayerStatus.Active).Count);
      // get buttonplayer
      this.ButtonPlayer = PlayerOrder.GetPlayers(LyssiePlayerStatus.Active)[ButtonIndex];
      // set current player
      PlayerOrder.SetNextPlayer(this.ButtonPlayer);
      PlayerOrder.NextTurn(); // button's turn
      Messenger.Log($"BUTTON Who's turn is it? {this.ButtonPlayer}");
    } while (Players[ButtonPlayer].CurrentlyPlaying == false); 
    // set the blinds
    // if there are more than 2 players, then the little blind is after button
    if (HowManyPlayersPlayingThisRound() > 2) {
      int LBPlayerIndex = this.ButtonIndex;
      do { // modify LBPlayer until we find a player who is playing
        LBPlayerIndex += 1;
        // correct button index
        LBPlayerIndex = NormalizeInt(LBPlayerIndex, PlayerOrder.GetPlayers(LyssiePlayerStatus.Active).Count);
        // get LBPlayer
        this.LittleBlindPlayer = PlayerOrder.GetPlayers(LyssiePlayerStatus.Active)[LBPlayerIndex];
      } while (Players[PlayerOrder.GetPlayers(LyssiePlayerStatus.Active)[LBPlayerIndex]].CurrentlyPlaying == false); 
      // get next player who is playing, and set them to
      this.LittleBlindPlayer = PlayerOrder.GetActivePlayersInOrder()[1];
      this.BigBlindPlayer = PlayerOrder.GetActivePlayersInOrder()[2];
      // current player needs to be after big blind, 3 turn changes
      PlayerOrder.NextTurn(); // LB's turn
      Messenger.Log($"LB Who's turn is it? {this.PlayerOrder.GetCurrentPlayer()}");
      PlayerOrder.NextTurn(); // BB's turn
      Messenger.Log($"BB Who's turn is it? {this.PlayerOrder.GetCurrentPlayer()}");
      PlayerOrder.NextTurn(); // UTG's turn
      Messenger.Log($"UTG Who's turn is it? {this.PlayerOrder.GetCurrentPlayer()}");
    }
    else { // little blind is the button, big blind is after button
      this.LittleBlindPlayer = PlayerOrder.GetActivePlayersInOrder()[0];
      this.BigBlindPlayer = PlayerOrder.GetActivePlayersInOrder()[1];
      // current player needs to be little blind, no turn changes
      // NEEDS TO BE FIXED SO THAT IT ONLY CONSIDERS THE PLAYERS WHO ARE INDEED PLAYING THIS ROUND
      Messenger.Log($"LB Who's turn is it? {this.PlayerOrder.GetCurrentPlayer()}");
    }
    // the blinds put in the blind
    Players[LittleBlindPlayer].CurrentBet = LittleBlindAmt;
    Players[LittleBlindPlayer].AmountOfMoneyLeft -= LittleBlindAmt;
    Players[BigBlindPlayer].CurrentBet = BigBlindAmt;
    Players[BigBlindPlayer].AmountOfMoneyLeft -= BigBlindAmt;
    // set game's currentbet
    CurrentBet = BigBlindAmt;
    // assign the last player who raised to be the big blind
    LastPlayerWhoRaised = BigBlindPlayer;
    // IF WE WANT TO DO BIG BLIND'S CHOICE, CODE WOULD GO HERE.
    // position the current player to be able to play
    Players[PlayerOrder.GetCurrentPlayer()].CanCall = true;
    Players[PlayerOrder.GetCurrentPlayer()].CanRaise = true;
    Players[PlayerOrder.GetCurrentPlayer()].CanFold = true;
    Messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
  }

  public bool ResetForNextRound()
  {
    List<string> playerNames = PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
    for (int i = 0; i < playerNames.Count; i++) {
      Players[playerNames[i]].CurrentBet = 0;
      Players[playerNames[i]].Folded = false;
      Players[playerNames[i]].CanFold = false;
      Players[playerNames[i]].CanRaise = false;
      Players[playerNames[i]].CanCheck = false;
      Players[playerNames[i]].CanCall = false;
    }
    return true;
  }
  
  public string GetGameState() {
    tehJsonState.Update(this);
    return JsonConvert.SerializeObject(tehJsonState, Formatting.Indented);
  }
  
  private void DeclareWantToPlay(string connStr, bool wantToPlay) {
    if (State == "DeclaringWantToPlay") {
      if (Players.ContainsKey(connStr)) {
        Players[connStr].CurrentlyPlaying = wantToPlay;
      }
      // if all players have declared that they want to play
      int totalActivePlayers = PlayerOrder.GetPlayers(LyssiePlayerStatus.Active).Count;
      int totalDeclaredPlayers = 0;
      foreach (string cs in PlayerOrder.GetPlayers(LyssiePlayerStatus.Active)) {
        if (Players[cs].HasDeclaredWhetherPlaying) {
          totalDeclaredPlayers ++;
        }
      }
      if (totalDeclaredPlayers == totalActivePlayers) {
        Messenger.SendFrontendError(new List<string>() {GameboardConnStr}, "Not enough players want to play this round.");
        RoundStart();
      }
    }
  }

  public bool Call(string connStr) {
    PokerPlayer player = Players[connStr];
    int amtExtraToPutIn = CurrentBet - player.CurrentBet;
    if (player.CanCall) {
      if (player.AmountOfMoneyLeft >= amtExtraToPutIn) {
        // player can afford to call
        player.AmountOfMoneyLeft -= amtExtraToPutIn;
        player.CurrentBet = CurrentBet;
        TotalPot += amtExtraToPutIn;

        // move onto new player
        SetUpOldPlayer(); // clears their bools
        Messenger.Log("IN CALL, NEXT PLAYER SHOULD GO");
        Messenger.Log($"OLD PLAYER WAS: {Players[PlayerOrder.GetCurrentPlayer()].Name}");
        GoToNextNotFoldedPlayingPlayer();
        Messenger.Log($"NEW PLAYER IS: {Players[PlayerOrder.GetCurrentPlayer()].Name}");

        var newCurrentPlayer = PlayerOrder.GetCurrentPlayer();
        // if new player was the last to raise, the street ends
        if (newCurrentPlayer == LastPlayerWhoRaised) {
          if (LastPlayerWhoRaisedCanStillGo) {
            // set up the new current player
            SetUpNewPlayer(true); // next player can't check
            LastPlayerWhoRaisedCanStillGo = false;
          }
          else {
            NextStreet();
          }
        }
        else {
          // set up the new current player
          SetUpNewPlayer(false); // next player can't check
          
        }
        Messenger.SendFrontendJson(new List<string>() {GameboardConnStr}, GetGameState());
        return true;
      }
      else { // can't afford to call
        Messenger.SendFrontendError(new List<string>() {connStr}, "Can't call, you can't afford the current bet!");
        return false;
      }
    }
    else {
        Messenger.SendFrontendError(new List<string>() {connStr}, "Can't call right now!");
        return false;
    }
  }
  public bool Fold(string connStr) {
    if (Players[connStr].CanFold) {
      Players[connStr].Folded = true;
    }
    // store whether a player could check beforehand
    bool foldedPlayerCouldCheck = Players[connStr].CanCheck;
    Players[connStr].CanFold = false;
    Players[connStr].CanCall = false;
    Players[connStr].CanCheck = false;
    Players[connStr].CanRaise = false;

    // either the player isn't the only player left, or there's
    if (HowManyPlayersArePlayingAndHaventFolded() > 1) {
      //
      GoToNextNotFoldedPlayingPlayer();
      var newCurrentPlayer = PlayerOrder.GetCurrentPlayer();
      if (PlayerOrder.GetCurrentPlayer() == LastPlayerWhoRaised) {
        // if new player was the last to raise, the street ends
        if (newCurrentPlayer == LastPlayerWhoRaised) {
          if (LastPlayerWhoRaisedCanStillGo) {
            // set up the new current player
            SetUpNewPlayer(true); // next player can't check
            LastPlayerWhoRaisedCanStillGo = false;
          }
          else {
            Messenger.Log("IN FOLD, RAN OUT OF PLAYERS...");
            Messenger.Log("SHOULD GO TO THE NEXT STREET NOW");
            NextStreet();
          }
        NextStreet();
        }
      }
      else {
        // move onto new player
        var prevCurrentPlayer = PlayerOrder.GetCurrentPlayer();
        Messenger.Log("IN FOLD, NEXT PLAYER SHOULD GO");
        GoToNextNotFoldedPlayingPlayer();
        var newCurrentPlayer2 = PlayerOrder.GetCurrentPlayer();
        // if new player was the last to raise, the street ends
        if (newCurrentPlayer2 == LastPlayerWhoRaised) {
          NextStreet();
        }
        else {
          // set up the new current player
          SetUpNewPlayer(foldedPlayerCouldCheck);
        }
      }
    }
    else { // not enough players

    }
    return true;
  }

  public bool Raise(string connStr, int amtToRaiseBy) {
    // your turn, you can afford it, you aren't the last to raise
    PokerPlayer thisPlayer = Players[connStr];
    if (thisPlayer.CanRaise) {
      int amtExtraToPutIn = CurrentBet + amtToRaiseBy - thisPlayer.CurrentBet;
      if (amtExtraToPutIn > thisPlayer.AmountOfMoneyLeft) {
        Messenger.SendFrontendError(new List<string>() {connStr}, "You can't afford to raise that much!");
        return false;
      }
      else { // can afford to raise by that amount
        thisPlayer.CurrentBet += amtExtraToPutIn;
        CurrentBet = thisPlayer.CurrentBet;
        thisPlayer.AmountOfMoneyLeft -= amtExtraToPutIn;
        return true;
      }
    }
    // for some reason, you can't raise at all
    else {
      if (connStr != PlayerOrder.GetCurrentPlayer()) {
        // it's not your turn!
        Messenger.SendFrontendError(new List<string>() {connStr}, "You can only raise on your turn!");
      }
      else if (thisPlayer.Name == LastPlayerWhoRaised) {
        Messenger.SendFrontendError(new List<string>() {connStr}, "You aren't elegible to raise!");
      }
      else if (thisPlayer.AmountOfMoneyLeft < (MinIncrease + CurrentBet - thisPlayer.CurrentBet)) {
        Messenger.SendFrontendError(new List<string>() {connStr}, "You can't afford to raise!");
      }
      else {
        Messenger.SendFrontendError(new List<string>() {connStr},"You can't raise right now!");
      }
      return false;
    }
  }

  private List<string> GetAllConnStrsIncGameboard() {
      List<string> allConnStrs = new List<string>(PlayerOrder.GetAllPlayers())
      {
        GameboardConnStr // adds this to the end of all the others
      }; 
      // shallow copy - the list is different but the objects are references and can be messed with
      return allConnStrs;
    }

    // Misc
    public bool SetAvatar(string connStr, string avatarStr) {
      if (Players.ContainsKey(connStr)) {
        Players[connStr].Avatar = avatarStr;
        return true;
      }
      else {
        Console.WriteLine($"Can't set avatar {avatarStr} for {connStr}, they aren't a player");
        return false;
      }
    }
    /// <summary>
    /// Normalize an integer. Returns a value between 0 (inclusive) and max - 1 (inclusive). So, it returns an int that is a valid index within a [max]-long array.
    /// </summary>
    /// <param name="valToNormalize"></param>
    /// <param name="max"></param>
    /// <returns></returns>
  public int NormalizeInt(int valToNormalize, int max) {
    int val = valToNormalize % max; // handle too large
    while (val < 0) { // handle too big
      val += max;
    }
    return val;
  }
  public int HowManyPlayersPlayingThisRound() {
    int count = 0;
    foreach (string connStr in PlayerOrder.GetPlayers(LyssiePlayerStatus.Active)) {
      if (Players[connStr].CurrentlyPlaying == true) {
        count += 1;
      }
    }
    return count;
  }

  public int HowManyPlayersArePlayingAndHaventFolded() {
    int count = 0;
    foreach (string connStr in PlayerOrder.GetPlayers(LyssiePlayerStatus.Active)) {
      if (Players[connStr].CurrentlyPlaying == true && Players[connStr].Folded == false) {
        count += 1;
      }
    }
    return count;
  }

  public void GoToNextNotFoldedPlayingPlayer() {
    do {
      PlayerOrder.NextTurn();
    } while (
        Players[PlayerOrder.GetCurrentPlayer()].Folded == true ||
        Players[PlayerOrder.GetCurrentPlayer()].CurrentlyPlaying == false);
  }

  public void NextStreet() {
    Messenger.Log("Next street!");
    LastPlayerWhoRaisedCanStillGo = true;
    // river is the final state of the game before winnerscreen
    if (State == "Pre-Flop" || State == "Flop" || State == "Turn") {
      // go to little blind player
      // if little blind is out, go to the first not-folded player
      PlayerOrder.SetNextPlayer(LittleBlindPlayer);
      PlayerOrder.NextTurn(); // current player == little blind
      if (Players[LittleBlindPlayer].Folded == true) {
        GoToNextNotFoldedPlayingPlayer();
      }
      SetUpNewPlayer(true);
    }
    if (State == "Pre-Flop") {
      State = "Flop";
      // give the 3 community cards
      Board.Add(Deck.Draw());
      Board.Add(Deck.Draw());
      Board.Add(Deck.Draw());
      // set the next canCheck
      Players[PlayerOrder.GetCurrentPlayer()].CanCheck = true;
    }
    else if (State == "Flop") {
      State = "Turn";
      // give the turn cards
      Board.Add(Deck.Draw());
      // set the next canCheck
      Players[PlayerOrder.GetCurrentPlayer()].CanCheck = true;
    }
    else if (State == "Turn") {
      State = "River";
      // give the turn cards
      Board.Add(Deck.Draw());
      // set the next canCheck
      Players[PlayerOrder.GetCurrentPlayer()].CanCheck = true;
    }
    else if (State == "River") {
      // end everything!
      State = "RoundEnd";
      Messenger.Log("Round has ended!");
    }
  }

  private void SetUpNewPlayer(bool canCheck) {
    PokerPlayer currentPlayer = Players[PlayerOrder.GetCurrentPlayer()];
    currentPlayer.CanFold = true;
    currentPlayer.CanCheck = canCheck;
    // player can call whenever they can afford to
    currentPlayer.CanCall = currentPlayer.AmountOfMoneyLeft >= (CurrentBet - currentPlayer.CurrentBet);
    // player can raise if they can afford to
    // but they also can't have raised last
    var playerCanAffordToRaise = currentPlayer.AmountOfMoneyLeft >= (MinIncrease + CurrentBet - currentPlayer.CurrentBet);
    currentPlayer.CanRaise = currentPlayer.Name != LastPlayerWhoRaised && playerCanAffordToRaise;
  }
  private void SetUpOldPlayer() {
    PokerPlayer currentPlayer = Players[PlayerOrder.GetCurrentPlayer()];
    currentPlayer.CanFold = false;
    currentPlayer.CanCheck = false;
    currentPlayer.CanCall = false;
    currentPlayer.CanCheck = false;
  }

  public void EndGame()
  {
    Messenger.Log("Game ended.");
  }
}