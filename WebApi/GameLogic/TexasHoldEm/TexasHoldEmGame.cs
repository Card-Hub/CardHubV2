// using System.Runtime.CompilerServices;
// using WebApi.Models;
// using WebApi.GameLogic.TexasHoldEmStates;
// using WebApi.Common;
// using WebApi.Common.LyssiePlayerOrder;
// using Newtonsoft.Json;
// using System.Text.Json;
// namespace WebApi.GameLogic;

// public class TexasHoldEmGame : IBaseGame<StandardCard>
// {
//   public TexasHoldEmState State {get; set;}
//   private TEHJsonState tehJsonState {get; set;}
//   //public string StateString = "";
//   public string GetStateString() {
//     return State.ToString();
//   }
//   public LyssiePlayerOrder PlayerOrder;
//   public Dictionary<string, PokerPlayer> Players;
//   public bool SpectatorsOnly {get; set;}
//   public StandardCardDeck Deck {get; set;}
//   public int InitialPlayerPot {get; set;}
//   public int LittleBlindAmt {get; set;}
//   public int BigBlindAmt {get; private set;}
//   public string LittleBlindPlayer {get; set;}
//   public string BigBlindPlayer {get; set;}
//   public int CurrentBet {get; set;}
//   public string Dealer {get; set;}
//   public List<StandardCard> Board {get; set;}
//   public int TotalPot {get; set;}
//   public string LastPersonWhoRaised {get; set;}


//   public TexasHoldEmGame() {
//     this.PlayerOrder = new();
//     this.Players = new();
//     this.State = new TexasHoldEmStateNotStarted(this);
//     this.tehJsonState = new();
//     this.Deck = new();
//     this.Deck.Init52();
//     this.LittleBlindAmt = 1;
//     this.BigBlindAmt = 2;
//     this.LittleBlindPlayer = "";
//     this.BigBlindPlayer = "";
//     this.TotalPot = 0;
//     this.Dealer = "";
//     this.Board = new();
//     this.InitialPlayerPot = 100;
//   }
//   public void SetState(TexasHoldEmState newState) {
//     this.State = newState;
//   }
//   //private
//   public bool AddPlayer(string playerName)
//   {
//     return this.State.AddPlayer(playerName);
//   }

//   public bool DrawCard(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public void EndGame()
//   {
//     throw new NotImplementedException();
//   }

//   public List<StandardCard> GetPlayerHand(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public List<string> GetPlayerList()
//   {
//     return PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
//   }
//   public List<string> GetSpectatorList() {
//     return this.PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator);
//     //throw new NotImplementedException();
//   }

//   public List<string> GetPlayersInOrder()
//   {
//     throw new NotImplementedException();
//   }

//   public bool RemovePlayer(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public void StartGame()
//   {
//     if (this.State.ToString() == "Not Started" && PlayerOrder.GetPlayers(LyssiePlayerStatus.Active).Count >= 2) {
//       List<string> playerNames = PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
//       for (int i = 0; i < playerNames.Count; i++) {
//         Players[playerNames[i]].Pot = InitialPlayerPot;
//       }
//       this.State.RoundStart();
//     }
//     else {
//       Console.WriteLine("Cannot start game.");
//     }
//   }

//   public bool InitDeck()
//   {
//     throw new NotImplementedException();
//   }

//   public bool ResetForNextRound()
//   {
//     throw new NotImplementedException();
//   }
//   public string GetGameState() {
//     tehJsonState.Update(this);
//     return JsonConvert.SerializeObject(tehJsonState, Formatting.Indented);
//   }
// }