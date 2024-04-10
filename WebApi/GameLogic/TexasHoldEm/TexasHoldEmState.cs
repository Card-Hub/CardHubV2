// using WebApi.Models;
// using WebApi.GameLogic;
// using WebApi.Common;
// namespace WebApi.GameLogic.TexasHoldEmStates;

// class TexasHoldEmStateSetup : TexasHoldEmState
// {
//   public TexasHoldEmStateSetup(TexasHoldEmGame game) : base(game) {}

//   public override bool SetSpectatorsOnly(bool spectatorsOnly)
//   {
//     throw new NotImplementedException();
//   }

//   public override bool SetPlayerLimit(int limit)
//   {
//     throw new NotImplementedException();
//   }

//   public override bool AddPlayer(string playerName)
//   {
//     // add spectator if possible
//     if (game.PlayerOrder.AddPlayer(playerName)) {
//       game.Players[playerName] = new PokerPlayer(playerName);
//       game.PlayerOrder.SetPlayerStatus(playerName, Common.LyssiePlayerOrder.LyssiePlayerStatus.Spectator);
//       return true;
//     }
//     else {
//       return false;
//     }
//   }

//   public override bool RemovePlayer(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public override bool KickPlayer(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public override bool SetPlayerToActive(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public override bool SetPlayerToAFK(string playerName)
//   {
//     return game.PlayerOrder.SetPlayerStatus(playerName, Common.LyssiePlayerOrder.LyssiePlayerStatus.Afk);
//   }

//   public override bool SetPlayerToSpectator(string playerName)
//   {
//     throw new NotImplementedException();
//   }

//   public override bool DrawCard(string playerName)
//   {
//     StandardCard card = game.Deck.Draw();
//     game.Players[playerName].Hand.Add(card);
//     return true;
//   }

//   public override bool Call(string playerName)
//   {
//     throw new InvalidOperationException("Can't call during setup");
//   }

//   public override bool Check(string playerName)
//   {
//     throw new InvalidOperationException("Can't check during setup");
//   }


//   public override bool Fold(string playerName)
//   {
//     throw new InvalidOperationException("Can't fold during setup");
//   }

//   public override bool Raise(string playerName)
//   {
//     Console.WriteLine($"{playerName} can't raise during setup.");
//     return false;
//   }

//   public override void NextTurn()
//   {
//     Console.WriteLine("Can't go to the next turn during setup.");
//   }

//   public override void RoundStart()
//   {
//     Console.WriteLine("Can't start a round during setup");
//   }

//   public override void RoundEnd()
//   {
//     Console.WriteLine("Can't end a round during setup");
//   }

//   public override void Setup()
//   {
//     // resetting of things will be in the RoundEnd() of BetweenHands
//     // shuffle deck, give each player 2 cards
//     game.Deck.ReclaimCards();
//     game.Deck.Shuffle();
//     StandardCard card;
//     List<string> playerNames = game.PlayerOrder.GetPlayers(Common.LyssiePlayerOrder.LyssiePlayerStatus.Active);
//     // (technically, you do it by passing everyone 1 card, then pass everyone a second card...)
//     for (int i = 0; i < playerNames.Count; i++) {
//       card = game.Deck.Draw();
//       game.Players[playerNames[i]].Hand.Add(card);
//     }
//     for (int i = 0; i < playerNames.Count; i++) {
//       card = game.Deck.Draw();
//       game.Players[playerNames[i]].Hand.Add(card);
//     }
//     // set current player, dealer, big blind, little blind
//     // in a two player game, the dealer is the little blind, and the other player is big blind
//     // otherwise, dealer is the first player, little blind is the second player, and third player is big blind
//     if (playerNames.Count == 2) {
//       game.Dealer = playerNames[0];
//       game.LittleBlindPlayer = playerNames[0];
//       game.BigBlindPlayer = playerNames[1];
//     }
//     else {
//       game.Dealer = playerNames[0];
//       game.LittleBlindPlayer = playerNames[1];
//       game.BigBlindPlayer = playerNames[2];
//       // the player after the big blind should go first
//       game.PlayerOrder.SetNextPlayer(3);
//     }
//     // set bets, take away from pots, add to total pot
//     // TODO ADD CHECKS FOR THIS!!!
//     // MAYBE A METHOD? IN POKERPLAYER?
//     game.Players[game.LittleBlindPlayer].Pot -= game.LittleBlindAmt;
//     game.Players[game.LittleBlindPlayer].CurrentBet += game.LittleBlindAmt;
//     game.Players[game.BigBlindPlayer].Pot -= game.BigBlindAmt;
//     game.Players[game.BigBlindPlayer].CurrentBet += game.BigBlindAmt;
//     game.LastPersonWhoRaised = game.BigBlindPlayer;
//     // total pot and current bet
//     game.TotalPot += game.LittleBlindAmt + game.BigBlindAmt;
//     game.CurrentBet = game.BigBlindAmt;
//     // sets the current player in PlayerOrder
//     game.PlayerOrder.NextTurn();
//     game.State = new TexasHoldEmStatePreFlop(game);
//   }

//   public override string ToString()
//   {
//     return "Setup";
//   }
// }