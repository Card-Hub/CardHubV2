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
  public List<StandardCard> Board {get; set;}
  public int TotalPot {get; set;}
  public string LastPlayerWhoRaised {get; set;}
  [JsonIgnore]
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

  public bool DrawCard(string playerName)
  {
    throw new NotImplementedException();
  }

  public void EndGame()
  {
    throw new NotImplementedException();
  }

  public List<StandardCard> GetPlayerHand(string playerName)
  {
    throw new NotImplementedException();
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
  
  public bool Call(string playerName) {
    throw new NotImplementedException();
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
    // assign the last player who raised to be the big blind
    LastPlayerWhoRaised = BigBlindPlayer;
    // IF WE WANT TO DO BIG BLIND'S CHOICE, CODE WOULD GO HERE.
    // position the current player to be able to play
    Players[PlayerOrder.GetCurrentPlayer()].CanCall = true;
    Players[PlayerOrder.GetCurrentPlayer()].CanRaise = true;
    Players[PlayerOrder.GetCurrentPlayer()].CanFold = true;
      Messenger.Log(ButtonPlayer);
      Messenger.Log(LittleBlindPlayer);
      Messenger.Log(BigBlindPlayer);
    Messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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
}