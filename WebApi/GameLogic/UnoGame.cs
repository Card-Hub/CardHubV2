using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.GameLogic;

public class UnoGame : IBaseGame {
  // constructor
  private List<String> playerList = new List<String>();
  private List<UnoCard> deck = new List<UnoCard>();
  private int directionInt = 1;
  private int CurrentPlayerIndex = 0;
  //private Dictionary<String, List<UnoCard>> playerHands;
  public UnoGame() {
    playerList = [];
    this.InitDeck();
    //this.SetUpHands();
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
        cardId++;
      }
    }
  }

  public void StartGame() {
    // Randomize list of players
    // Player whose turn it is, is first player
    Console.WriteLine("Hi");
    //throw new NotImplementedException();
  }
  public List<String> GetPlayerList() {
    return playerList;
    throw new NotImplementedException();
  }
   public List<UnoCard> GetDeck() {
    return deck;
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
  public String GetUnoCardString(UnoCard card) {
    String str =  "Uno Card with id: " + card.id.ToString() + ", value: " + card.value + ", color: " + card.color;
    return str;
  }
  public void shuffleDeck() {
    Random rng = new Random();
    deck = deck.OrderBy(_ => rng.Next()).ToList();
  }
  public void shufflePlayers() {
    Random rng = new Random();
    playerList = playerList.OrderBy(_ => rng.Next()).ToList();
  }
  public void EndGame() {
    throw new NotImplementedException();
  }
  public UnoCard popTopCard() {
    UnoCard card = deck.ElementAt(0);
    deck.RemoveAt(0);
    return card;
  }
  //public bool drawCard(String playerName) {
  //  if (playerName == GetCurrentPlayer()) {
  //    player
  //  }
  //}
  public bool TryPlayCard(UnoCard card) {
    return true;
  }
  public bool ReverseDirection() {
    directionInt /= -1; // -1 -> 1, or 1 -> -1
    return true;
  }
  public UnoCard GetTopCard() {
    return deck.ElementAt(0);
  }
  //public UnoCard Assign7CardsToPlayer(playerName) {
  //  for (int j = 0; j < 7; j++) {
  //    currentPlayer = playerName;

  //  }
  //}
  //public 
  //public void SetUpHands() {
  //  for (int i = 0; i < playerList.Count; i++) {
  //    Console.WriteLine("Assigning card to " + playerList.ElementAt(i));

  //  }
  //}
}