using WebApi.GameLogic;
using WebApi.Models;
using System;

// to run simulation:
// (in PowerShell)
// make sure you have csc compiler downloaded, add the location of csc to PATH, then in PowerShell:
// cd .\WebApi\GameLogic\Simulations
// csc .\UnoGameSim.cs ..\UnoGame.cs ..\..\Models\iCard.cs ..\..\Models\UnoCard.cs ..\iBaseGame.cs

namespace WebApi.GameLogic.Simulations {

public class UnoGameSim {
  public UnoGameSim() {}
  public static void Simulate() {
    UnoGame game = new UnoGame();
    game.AddPlayer("Alex");
    game.AddPlayer("Liam");
    game.AddPlayer("Lyssie");
    game.AddPlayer("Rubi");
    game.ShuffleDeck();
    game.StartGame();
    Console.WriteLine("RANDOM PLAYER ORDER:");
    foreach (string player in game.GetPlayerList()) {
      Console.WriteLine("\t" + player);
    }
    Console.WriteLine("PLAYER CARDS:");
    foreach (string name in game.GetPlayerList())
    {
      Console.WriteLine("\t" + name + "'s cards:");
      foreach (UnoCard card in game.GetPlayerHand(name))
      {
        Console.WriteLine("\t\t" + card.ToString());
      }
    }
    int playerTurnNum = 0;
    Console.WriteLine("GAME START!");
    List<UnoCard> playerHand = new List<UnoCard>();
    string playerName = "";
    while (game.IsOngoing()) {
      playerName = game.GetCurrentPlayer();
      // person is going to try and play the game.
      Console.WriteLine(playerTurnNum.ToString() + ". " + playerName + "'s turn.");
      // try and play a card
      playerHand = game.GetPlayerHand(playerName);
      if (!game.PlayerHasPlayableCard(playerName)) {Console.WriteLine("No playable cards!"); game.DrawAndMoveOn(playerName);}
      bool playedACard = false;
      for (int i = 0; i < playerHand.Count; i++) {
        if (!playedACard && game.CardCanBePlayed(playerHand.ElementAt(i))) {
          playedACard = true;
          game.PlayCard(playerName, playerHand.ElementAt(i));
          if (game.PlayerNeedsToPickWildColor(playerName)) {
            // get the first non-black color in the hand
            string newWildColor = "red";
            foreach (UnoCard card in game.GetPlayerHand(playerName)) {
              if (card.Color != "black") {
                newWildColor = card.Color;
              }
              // set that color as the new wild color
              game.SetWildColor(playerName, newWildColor);
              break;
            }
          }
        }
      }
      string endTurnStr = "Card totals: ";
      foreach (string name in game.GetPlayerList()) {
        endTurnStr += name + ": " + game.GetPlayerHand(name).Count() + ", ";
      }
      Console.WriteLine(endTurnStr);

      playerTurnNum ++;
    }
    Console.WriteLine(playerName + " won!");
  }
}

}