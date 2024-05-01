//using System.ComponentModel;
//using WebApi.GameLogic;
//using WebApi.Models;
//using Xunit.Abstractions;

//// to run simulation:
//// cd to CardHubV2 (or whatever your parent folder for WebApi is)
//// dotnet test --filter "SimGame=Uno" --logger "console;verbosity=detailed"

namespace Tests.GameSims;

//[Category("Skip")]
//public class UnoGameSim {
//  private readonly ITestOutputHelper output;
//  public UnoGameSim(ITestOutputHelper output) {
//    this.output = output;
//  }
//  public void Simulate() {
//    i
//    UnoGame game = new UnoGameModLyssie();
//    game.AddPlayer("Alex");
//    game.AddPlayer("Liam");
//    game.AddPlayer("Lyssie");
//    game.AddPlayer("Rubi");
//    game.ShuffleDeck();
//    game.StartGame();
//    output.WriteLine("RANDOM PLAYER ORDER:");
//    foreach (string player in game.GetPlayerList()) {
//      output.WriteLine("\t" + player);
//    }
//    output.WriteLine("PLAYER CARDS:");
//    foreach (string name in game.GetPlayerList())
//    {
//      output.WriteLine("\t" + name + "'s cards:");
//      foreach (UnoCard card in game.GetPlayerHand(name))
//      {
//        output.WriteLine("\t\t" + card.ToString());
//      }
//    }
//    int playerTurnNum = 0;
//    output.WriteLine("GAME START!");
//    List<UnoCard> playerHand = new List<UnoCard>();
//    string playerName = "";
//    while (game.IsOngoing()) {
//      playerName = game.GetCurrentPlayer();
//      // person is going to try and play the game.
//      output.WriteLine(playerTurnNum.ToString() + ". " + playerName + "'s turn.");
//      // try and play a card
//      playerHand = game.GetPlayerHand(playerName);
//      if (!game.PlayerHasPlayableCard(playerName)) {
//        output.WriteLine("No playable cards!");
//        bool drewAndMovedOn = game.DrawAndMoveOn(playerName);
//        if (drewAndMovedOn) {
//          output.WriteLine(playerName + " drew a card : " + game.GetPlayerHand(playerName).Last().ToString());
//        }
//      }
//      else {
//          bool playedACard = false;
//      for (int i = 0; i < playerHand.Count; i++) {
//        if (!playedACard && game.CardCanBePlayed(playerHand.ElementAt(i))) {
//          playedACard = true;
//          output.WriteLine(playerName + " played a card: " + playerHand.ElementAt(i).ToString());
//          game.PlayCard(playerName, playerHand.ElementAt(i));
//          if (game.PlayerNeedsToPickWildColor(playerName)) {
//            // get the first non-black color in the hand
//            string newWildColor = "red";
//            foreach (UnoCard card in game.GetPlayerHand(playerName)) {
//              if (card.Color != "black") {
//                newWildColor = card.Color;
//              }
//              // set that color as the new wild color
//              bool wildColorSet = game.SetWildColor(playerName, newWildColor);
//              if (wildColorSet) {
//                output.WriteLine(playerName + " set the wild color to be " + newWildColor);
//              }
//              else {
//                output.WriteLine(playerName + " could not set the wild color to " + newWildColor);
//              }
//              break;
//            }
//          }
//        }
//      }
//      }
      
//      string endTurnStr = "Card totals: ";
//      foreach (string name in game.GetPlayerList()) {
//        endTurnStr += name + ": " + game.GetPlayerHand(name).Count() + ", ";
//      }
//      output.WriteLine(endTurnStr);

//      playerTurnNum ++;
//    }
//    output.WriteLine(playerName + " won!");
//  }
//}