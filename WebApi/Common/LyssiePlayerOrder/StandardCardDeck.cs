using System.ComponentModel;
using WebApi.Models;
namespace WebApi.Common;

public class StandardCardDeck : Deck<StandardCard> {
  public StandardCardDeck() : base() {}
  // Initializes the Deck to have 52 cards.
  public void Init52() {
    this._cards.Clear();
    this._drawnCards.Clear();
    int id = 0;
    List<string> suits = new List<string>() {"Clubs", "Diamonds", "Hearts", "Spades"};
    List<string> ranks = new List<string>() {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    foreach (string suit in suits) {
      foreach (string rank in ranks) {
        this._cards.Add(new StandardCard(id, suit, rank));
        id += 1;
      }
    }
  }
  /// <summary>
  /// Initializes the Deck to have Jokers and the standard 52 cards of a deck.
  /// </summary>
  public void Init54() {
    this._cards.Clear();
    this._drawnCards.Clear();
    int id = 0;
    this._cards.Add(new StandardCard(id, "", "Joker"));
    id += 1;
    this._cards.Add(new StandardCard(id, "", "Joker"));
    id += 1;
    List<string> suits = new List<string>() {"Clubs", "Diamonds", "Hearts", "Spades"};
    List<string> ranks = new List<string>() {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    foreach (string suit in suits) {
      foreach (string rank in ranks) {
        this._cards.Add(new StandardCard(id, suit, rank));
        id += 1;
      }
    }
  }
}