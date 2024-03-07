using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.GameLogic;

public class UnoGame : IBaseGame {
  // constructor
  private List<String> playerList = new List<String>();
  private List<UnoCard> deck = new List<UnoCard>();
  public UnoGame() {
    playerList = [];
    this.InitDeck();
  }
  public void InitDeck() {
    // Add cards to deck
    String[] colors = ["red", "blue", "yellow", "green"];
    String[] coloredValues = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Draw 2", "Draw 2", "Reverse", "Reverse", "Skip", "Skip"];
    String[] wildCardValues = ["Wild", "Wild Draw 4"];
    int cardId = 0;
    // Add colored cards
    foreach (String color in colors) {
      foreach (String value in coloredValues) {
        UnoCard newCard = new UnoCard();
        newCard.id = cardId;
        newCard.value = value;
        newCard.color = color;
        deck.Add(newCard);
        cardId++;
      }
    }
    foreach (String wildCardValue in wildCardValues) {
      for (int i = 0; i < 4; i++) { // 4 times each
        UnoCard newCard = new UnoCard();
        newCard.id = cardId;
        newCard.value = wildCardValue;
        newCard.color = "wild";
        deck.Add(newCard);
      }
    }
  }

  public void StartGame() {
    throw new NotImplementedException();
  }
  public List<String> GetPlayerList() {
    return playerList;
    throw new NotImplementedException();
  }

  /// <summary>
  /// If a player is not in the list of players, remove them.
  /// <para /> 
  /// <c>playerName</c>: String; player name to add 
  /// </summary>
  public Boolean AddPlayer(String playerName) {
    if (playerList.IndexOf(playerName) == -1) {
      playerList.Add(playerName);
      return true;
    }
    else {
      return false;
    }
  }

  /// <summary>
  /// If a player exists in the list of players, remove them.
  /// <para /> 
  /// <c>playerName</c>: String; player name to remove 
  /// </summary>
  public Boolean RemovePlayer(String playerName) {
    if (playerList.IndexOf(playerName) != -1) {
      // playerName in playerList
      playerList.Remove(playerName);
      return true; // successful
    }
    return false; // failed due to not having playerName in list of players
    throw new NotImplementedException();
  }
  public void EndGame(String playerName) {
    throw new NotImplementedException();
  }
  public static String GetUnoCardString(UnoCard card) {
    String str =  "Uno Card with id: " + card.id.ToString() + ", value: " + card.value + ", color: " + card.color;
    return str;
  }
  public void shuffleDeck() {
    Random rng = new Random();
    deck = deck.OrderBy(_ => rng.Next()).ToList();
  }
  public void EndGame() {
    throw new NotImplementedException();
  }
}