using WebApi.Models;
using WebApi.Models.CardConverter;
using System.Linq;
using System; // Console

namespace WebApi.GameLogic {

public class UnoGame : IBaseGame<UnoCard> {
  // constructor
  private List<string> playerList = new List<string>();
  private List<UnoCard> Deck = new List<UnoCard>();
  private List<UnoCard> DiscardPile = new List<UnoCard>();
  private int DirectionInt = 1;
  private int CurrentPlayerIndex = 0;
  private bool someoneHasWon = false;
  private Dictionary<string, List<UnoCard>> playerHands = new Dictionary<string, List<UnoCard>>();
  private string WildColor = "";
  private string PlayerWhoNeedsToPickWildColor = "";
  public UnoGame() {
    playerList = [];
    this.InitDeck();
  }
  /// <summary>
  /// Initialize the deck. Resets it to the 100 something cards.
  /// </summary>
  public List<string> GetPlayersInOrder() {
    throw new NotImplementedException();
  }
  private void InitDeck() {
    Deck.Clear();
    // Add cards to deck
    string[] colors = ["red", "blue", "yellow", "green"];
    string[] coloredValues = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Draw Two", "Draw Two", "Reverse", "Reverse", "Skip", "Skip", "Skip All", "Skip All"];
    string[] wildCardValues = ["Wild", "Wild Draw Four"];
    int cardId = 0;
    // Add colored cards
    foreach (string color in colors) {
      foreach (string value in coloredValues) {
        UnoCard newCard = new UnoCard();
        newCard.Id = cardId;
        newCard.Value = value;
        newCard.Color = color;
        Deck.Add(newCard);
        cardId++;
      }
    }
    foreach (string wildCardValue in wildCardValues) {
      for (int i = 0; i < 4; i++) { // 4 times each
        UnoCard newCard = new UnoCard();
        newCard.Id = cardId;
        newCard.Value = wildCardValue;
        newCard.Color = "black";
        Deck.Add(newCard);
        cardId++;
      }
    }
  }

  public void StartGame() {
    // Randomize list of players
    ShufflePlayers();
    // Player whose turn it is, is first player
    Console.WriteLine("Hi");
    CurrentPlayerIndex = 0;
    foreach (string playerName in playerList) {
      playerHands[playerName] = new List<UnoCard>();
      Assign7CardsToPlayer(playerName);
    }
    // put first card on discard pile until there's no wild card
    bool wildOnTop = true;
    while (wildOnTop) {
      DiscardPile.Insert(0, PopTopCard());
      wildOnTop = DiscardPile.ElementAt(0).Color == "black";
    }
    Console.WriteLine("Card on top of discard pile is " + DiscardPile.ElementAt(0).ToString());
  }
  public List<string> GetPlayerList() {
    return playerList;
    throw new NotImplementedException();
  }
  public List<UnoCard> GetDeck() {
    return Deck;
  }

  /// <summary>
  /// If a player is not in the list of players, remove them.
  /// <para /> 
  /// <c>playerName</c>: string; player name to add 
  /// </summary>
  public bool AddPlayer(string playerName) {
    if (playerList.IndexOf(playerName) == -1) {
      playerList.Add(playerName);
      return true;
    }
    else {
      return false;
    }
  }


  /// <summary>
  /// NOT DONE
  /// If a player exists in the list of players, remove them.
  /// <para /> 
  /// <c>playerName</c>: string; player name to remove 
  /// </summary>
  public bool RemovePlayer(string playerName) {
    if (playerList.IndexOf(playerName) != -1) {
      // playerName in playerList
      playerList.Remove(playerName);
      return true; // successful
    }
    return false; // failed due to not having playerName in list of players
    throw new NotImplementedException();
  }
  public void EndGame(string playerName) {
    throw new NotImplementedException();
  }
/// <summary>
/// Get the string associated with an Uno card.
/// </summary>
/// <para />
/// <returns><c>"Uno Card with id: 4, value: Skip, color: blue"</c></returns>
  public void ShuffleDeck() {
    Random rng = new Random();
    Deck = Deck.OrderBy(_ => rng.Next()).ToList();
  }
  public void ShufflePlayers() {
    Random rng = new Random();
    playerList = playerList.OrderBy(_ => rng.Next()).ToList();
  }
  private UnoCard PopTopCard() {
    UnoCard card = Deck.ElementAt(0);
    Deck.RemoveAt(0);
    return card;
  }
  public bool Draw(string playerName) {
    if (playerName == GetCurrentPlayer()) {
      playerHands[playerName].Add(PopTopCard());
      return true;
    }
    return false;
  }
  public bool ReverseDirection() {
    DirectionInt /= -1; // -1 -> 1, or 1 -> -1
    return true;
  }
  public UnoCard GetTopCard() {
    return Deck.ElementAt(0);
  }
  public void Assign7CardsToPlayer(string playerName) {
    CurrentPlayerIndex = playerList.IndexOf(playerName);
    for (int j = 0; j < 7; j++) {
      Draw(playerName);
    }
  }
  private void SetUpHands() {
    for (int i = 0; i < playerList.Count; i++) {
      Console.WriteLine("Assigning card to " + playerList.ElementAt(i));
      Assign7CardsToPlayer(playerList.ElementAt(i));
    }
  }
  public List<UnoCard> GetPlayerHand(string playerName) {
    return playerHands[playerName]; 
  }
  public string GetCurrentPlayer() {
    return playerList[CurrentPlayerIndex];
  }
  public bool PlayerHasPlayableCard(string playerName) {
    foreach (UnoCard card in GetPlayerHand(playerName)) {
      if (CardCanBePlayed(card)) {return true;}
    }
    return false;
  }
  public void NextTurn() {
    Console.WriteLine("Next turn!");
    CurrentPlayerIndex += DirectionInt;
    if (CurrentPlayerIndex >= playerList.Count) {
      CurrentPlayerIndex = 0;
    }
    if (CurrentPlayerIndex < 0) {
      CurrentPlayerIndex = playerList.Count - 1;
    }
  }
  public List<UnoCard> GetDiscardPile() {
    return DiscardPile;
  }
  public UnoCard GetTopCardInDiscardPile() {
    return DiscardPile.ElementAt(0);
  }
  
  public bool CardCanBePlayed(UnoCard cardToPlay) {
    bool cardCanBePlayed = false;
    bool cardMatchesValue = GetTopCardInDiscardPile().Value == cardToPlay.Value;
    bool cardMatchesColor = GetTopCardInDiscardPile().Color == cardToPlay.Color;
    // handle if there's a wild on the top card
    bool cardMatchesWildColor = false;
    if (GetTopCardInDiscardPile().Value == "Wild" || GetTopCardInDiscardPile().Value == "Wild Draw Four") {
      if (cardToPlay.Color == WildColor) {
        cardMatchesWildColor = true;
      }
    }
    // handle if cardToPlay is a wild card itself
    bool cardToPlayIsWild = cardToPlay.Value == "Wild" || cardToPlay.Value == "Wild Draw 4";

    cardCanBePlayed = cardMatchesValue || cardMatchesColor || cardMatchesWildColor || cardToPlayIsWild;
    return cardCanBePlayed;
  }
  public bool PlayCard(string playerName, UnoCard card) {
    string[] numericalVals = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
    string[] actionCards = {"Skip", "Draw Two", "Reverse"};
    string[] wildCards = {"Wild", "Wild Draw Four"};

    if (playerName == GetCurrentPlayer() || CardCanBePlayed(card)) {
      // check what type of card it is
      bool cardIsNumerical = Array.IndexOf(numericalVals, card.Value) != -1;
      bool cardIsAction = Array.IndexOf(actionCards, card.Value) != -1;
      bool cardIsWild = Array.IndexOf(wildCards, card.Value) != -1;


      // put card on top of the discard pile
      DiscardPile.Insert(0, card);
      // remove card from player hand
      playerHands[playerName].Remove(card);
      Console.WriteLine(playerName + " successfully played a card: " + card.ToString());
      // handle card effects
      if (cardIsNumerical) { // nothing more to do
      }
      else if (cardIsAction) {
        if (card.Value == "Skip") {
          NextTurn();
        }
        else if (card.Value == "Reverse") {
          ReverseDirection();
        }
        else if (card.Value == "Draw 2") {
          NextTurn();
          Draw(GetCurrentPlayer());
          Draw(GetCurrentPlayer());
        }
        else {
          Console.WriteLine("Played unknown action card.");
        }
      }
      else if (cardIsWild) {
          SetPlayerNeedsToPickWildColor(GetCurrentPlayer());
          Console.WriteLine(playerName + " needs to pick a wild color now. It is still their turn.");
          // handle player needs to pick wild elsewhere
      }
      foreach (string name in GetPlayerList()) {
        if (GetPlayerHand(name).Count == 0) {
          someoneHasWon = true;
          Console.WriteLine(name + " has won ~");
      }
      }
      if (!cardIsWild) {
        NextTurn();
      }
      return true;
    }
    return false;
  }
  
  public bool PlayerNeedsToPickWildColor(string playerName) {
    return playerName == PlayerWhoNeedsToPickWildColor;
  }
  public bool SetWildColor(string playerName, string color) {
    if (playerName == GetCurrentPlayer()) {
      if (playerName == PlayerWhoNeedsToPickWildColor) {
        WildColor = color;
        Console.WriteLine(playerName + " set the wild color to be " + color);
        PlayerWhoNeedsToPickWildColor = "";
        NextTurn();
        return true;
      }
      else {
        Console.WriteLine(playerName + " cannot set the wild color, it's not their job!");
        return false;
      }
    }
    else {
      Console.WriteLine("ERROR: " + playerName + " cannot set the wild color, it's not their turn!");
      return false;
    }
  }
  
  public bool IsOngoing() {
    return !someoneHasWon;
  }
  public bool DrawAndMoveOn(string playerName) {
    Draw(playerName);
    Console.WriteLine(playerName + " drew a card : " + GetPlayerHand(playerName).Last().ToString());
    NextTurn();
    return true;
  }
  private void SetPlayerNeedsToPickWildColor(string playerName) {
    PlayerWhoNeedsToPickWildColor = playerName;
  }
  private void ResetWildColor() {
    WildColor = "";
  }
  private void updateWildColor(string color) {
    WildColor = color;
    PlayerWhoNeedsToPickWildColor = "";
  }
}
}