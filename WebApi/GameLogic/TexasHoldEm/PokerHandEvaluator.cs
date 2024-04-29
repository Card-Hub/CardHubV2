using System.Collections;
using WebApi.Models;

namespace WebApi.GameLogic;

public enum PokerHand {
  HighCard = 0,
  OnePair = 1,
  TwoPair = 2,
  ThreeOfAKind = 3,
  Straight = 4,
  Flush = 5,
  FullHouse = 6,
  FourOfAKind = 7,
  StraightFlush = 8,
  RoyalFlush = 9
}

public class PokerHandEvaluator() {
  public int ValToInt(string val) {
    switch (val.ToUpper()) {
      case "2": return 2;
      case "3": return 3;
      case "4": return 4;
      case "5": return 5;
      case "6": return 6;
      case "7": return 7;
      case "8": return 8;
      case "9": return 9;
      case "10": return 10;
      case "J": return 11;
      case "Q": return 12;
      case "K": return 13;
      case "A": return 14;
      default: throw new ArgumentException($"Invalid val {val}");
    }
  }
  public PokerHand GetHandType5(List<StandardCard> fiveCardHand) {
    List<StandardCard> sortedHand = fiveCardHand.OrderByDescending(card => ValToInt(card.Value)).ToList();
    // calculate number of duplicates
    // will only ever be up to 2 unique duplicates
    string val = "";
    Dictionary<string, int> duplicates = new();
    for (int i = 1; i < 5; i++) {
      val = sortedHand[i].Value;
      // found a duplicate
      if (sortedHand[i].Value == sortedHand[i-1].Value) {
        // previous found a duplicate
        if (duplicates.ContainsKey(sortedHand[i].Value)) {
          duplicates[val]++;
        }
        else { // found a new duplicate
          duplicates[val] = 2;
        }
      }
    }
    // calculate hands
    
    // hand type 5: flush
    bool isFlush = AllSameSuit(sortedHand);
    // hand type 4: straight
    bool isStraight = IsStraight(sortedHand);
    // hand type 8: straight flush
    bool isStraightFlush = isStraight && isFlush;
    // hand type 9: royal flush
    bool isRoyalFlush = isStraightFlush && sortedHand[0].Value == "A" && sortedHand[4].Value == "10";
    // hand type 7: four of a kind
    bool isFourOfAKind = false;
    if (duplicates.Keys.Count == 1) {
      if (duplicates [duplicates.Keys.ToArray()[0]] == 4) {
        isFourOfAKind = true;
      }
    }
    // hand type 6: full house
    bool isFullHouse = false;
    if (duplicates.Keys.Count == 2) {
      string[] duplicatesVals = duplicates.Keys.OrderDescending().ToArray();
      int biggerValCount = duplicates[duplicatesVals[0]]; 
      int smallerValCount = duplicates[duplicatesVals[1]]; 
      if ( (biggerValCount == 3 || smallerValCount == 3)
           && (biggerValCount == 2 || smallerValCount == 2) ) {
        isFullHouse = true;
      }
    }
    // hand type 3: three of a kind
    bool isThreeOfAKind = false;
    if (duplicates.Keys.Count == 1) {
      if (duplicates [ duplicates.Keys.ToArray()[0]] == 3) {
        isThreeOfAKind = true;
      }
    }
    // hand type 2: two pair
    bool isTwoPair = false;
    if (duplicates.Keys.Count == 2) {
      if ( duplicates [duplicates.Keys.ToArray()[0]] == 2 && duplicates [duplicates.Keys.ToArray()[1]] == 2) {
        isTwoPair = true;
      }
    }
    // hand type 1: one pair
    bool isOnePair = false;
    if (duplicates.Keys.Count == 1) {
      if ( duplicates [duplicates.Keys.ToArray()[0]] == 2) {
        isOnePair = true;
      }
    }
    if (isRoyalFlush) {return PokerHand.RoyalFlush;} // 9
    else if (isStraightFlush) {return PokerHand.StraightFlush;} // 8
    else if (isFourOfAKind) {return PokerHand.FourOfAKind;} // 7
    else if (isFullHouse) {return PokerHand.FullHouse;} // 6
    else if (isFlush) {return PokerHand.Flush;} // 5
    else if (isStraight) {return PokerHand.Straight;} // 4
    else if (isThreeOfAKind) {return PokerHand.ThreeOfAKind;} // 3
    else if (isTwoPair) {return PokerHand.TwoPair;} // 2
    else if (isOnePair) {return PokerHand.OnePair;} // 3
    else {return PokerHand.HighCard;}
  }
  public bool AllSameSuit(List<StandardCard> cards) {
    bool allSameSuit = true;
    for (int i = 1; i < 5; i++) {
      allSameSuit = allSameSuit && cards[i].Suit == cards[i-1].Suit;
    }
    return allSameSuit;
  }
  public bool IsStraight(List<StandardCard> sortedCards) {
    bool isStraight = true;
    int firstValInt = 0;
    int secondValInt = 0;
    for (int i = 1; i < 5; i++) {
      firstValInt = ValToInt(sortedCards[i-1].Value);
      secondValInt = ValToInt(sortedCards[i].Value);
      isStraight = isStraight && (firstValInt == 1 + secondValInt);
    }
    // calculate edge case for if it's A 2 3 4 5
    bool isEdgeCase = true;
    string[] edgeCaseVals = ["A", "5", "4", "3", "2"];
    for (int i = 0; i < 5; i++) {
      isEdgeCase = isEdgeCase && (edgeCaseVals[i] == sortedCards[i].Value);
    }
    return isStraight || isEdgeCase;
  }
}