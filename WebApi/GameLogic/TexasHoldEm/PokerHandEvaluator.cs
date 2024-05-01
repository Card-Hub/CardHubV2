using System.Collections;
using System.Reflection.Metadata.Ecma335;
using NetTopologySuite.Operation.Overlay;
using WebApi.Models;


// this could be modularized and simplified, but who cares? it passes testing and speed ain't an issue rn.

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

  public int GetHandScore5(List<StandardCard> fiveCardHand) {
    List<StandardCard> sortedHand = fiveCardHand.OrderByDescending(card => ValToInt(card.Value)).ToList();

    PokerHand handType = GetHandType5(fiveCardHand);

    int primaryScore = 0;
    int tiebreaker1 = 0;
    int tiebreaker2 = 0;
    int tiebreaker3 = 0;
    int tiebreaker4 = 0;
    int tiebreaker5 = 0;
    switch (handType) {
      case PokerHand.RoyalFlush:
        primaryScore = 10;
        // tiebreaker 1-5 are 0
        break;
      case PokerHand.StraightFlush:
        primaryScore = 9;
        tiebreaker1 = ValToInt(sortedHand[0].Value);
        // edge case: if it's A 5 4 3 2, then highest value is 5
        if (sortedHand[0].Value == "A" && sortedHand[1].Value == "5") {
          tiebreaker1 = ValToInt("5");
        }
        // tiebreaker 2-5 are 0
        break;
      case PokerHand.FourOfAKind:
        primaryScore = 8;
        // get the value of the repeated card
        int howManyCardsToCheck = 3;
        string repeatedCardValue = "";
        string[] valsThatCouldBeRepeated = new string[howManyCardsToCheck-1];
        for (int i = 0; i < howManyCardsToCheck; i++) {
          if (valsThatCouldBeRepeated.Contains(sortedHand[i].Value)) {
            repeatedCardValue = sortedHand[i].Value;
            break;
          }
          else {
            valsThatCouldBeRepeated[i] = sortedHand[i].Value;
          }
        }
        // get the value of the remaining card
        string remainingCardValue = "";
        for (int i = 0; i < 5; i++) {
          if (sortedHand[i].Value != repeatedCardValue) {
            remainingCardValue = sortedHand[i].Value;
            break;
          }
        }
        // calculate score
        tiebreaker1 = ValToInt(repeatedCardValue);
        tiebreaker2 = ValToInt(remainingCardValue);
        // tiebreaker 3-5 are 0
        break;
      case PokerHand.FullHouse:
        primaryScore = 7;
        // get the value of the triplet and pair
        string tripletValue = "";
        string pairValue = "";
        Dictionary<string, int> valCounts = new();
        for (int i = 0; i < 5; i++) {
          if (valCounts.ContainsKey(sortedHand[i].Value)) {
            valCounts[sortedHand[i].Value] ++;
          }
          else {
            valCounts[sortedHand[i].Value] = 1;
          }
        }
        string firstVal = valCounts.Keys.ToArray()[0];
        string secondVal = valCounts.Keys.ToArray()[1];
        if (valCounts[firstVal] == 3) {
          tripletValue = firstVal;
          pairValue =  secondVal;
        }
        else {
          tripletValue = secondVal;
          pairValue =  firstVal;
        }
        tiebreaker1 = ValToInt(tripletValue);
        tiebreaker2 = ValToInt(pairValue);
        // tiebreaker 3-5 are 0
        break;
      case PokerHand.Flush: //NOT DONE
        primaryScore = 6;
        tiebreaker1 = ValToInt(sortedHand[0].Value);
        tiebreaker2 = ValToInt(sortedHand[1].Value);
        tiebreaker3 = ValToInt(sortedHand[2].Value);
        tiebreaker4 = ValToInt(sortedHand[3].Value);
        tiebreaker5 = ValToInt(sortedHand[4].Value);
        break;
      case PokerHand.Straight:
        primaryScore = 5;
        // tiebreaker1 is the highest value in the straight
        tiebreaker1 = ValToInt(sortedHand[0].Value);
        // edge case: if it's A 5 4 3 2, then highest value is 5
        if (sortedHand[0].Value == "A" && sortedHand[1].Value == "5") {
          tiebreaker1 = ValToInt("5");
        }
        break;
      case PokerHand.ThreeOfAKind:
        primaryScore = 4;
        // get the value of the triplet and pair
        tripletValue = "";
        valCounts = new(); // dictionary<string, int>
        for (int i = 0; i < 5; i++) {
          if (valCounts.ContainsKey(sortedHand[i].Value)) {
            valCounts[sortedHand[i].Value] ++;
          }
          else {
            valCounts[sortedHand[i].Value] = 1;
          }
        }
        firstVal = valCounts.Keys.ToArray()[0];
        secondVal = valCounts.Keys.ToArray()[1];
        string thirdVal = valCounts.Keys.ToArray()[2];
        if (valCounts[firstVal] == 3) {
          tripletValue = firstVal;
        }
        else if (valCounts[secondVal] == 3) {
          tripletValue = secondVal;
        }
        else {
          tripletValue = thirdVal;
        }
        // grab the other two values's ints
        List<int> remainingIntVals = new();
        foreach(StandardCard card in sortedHand) {
          if (card.Value != tripletValue) {
            remainingIntVals.Add(ValToInt(card.Value));
          }
        }
        remainingIntVals = remainingIntVals.OrderDescending().ToList();
        tiebreaker1 = ValToInt(tripletValue);
        tiebreaker2 = remainingIntVals[0];
        tiebreaker3 = remainingIntVals[1];
        // tiebreaker 4-5 are 0
        break;
      case PokerHand.TwoPair:
        primaryScore = 3;
        // get the values of the pairs
        string[] pairVals = new string[2];
        int numPairsFoundSoFar = 0;
        for (int i = 1; i < 5; i++) {
          if (sortedHand[i].Value == sortedHand[i-1].Value) { // found a pair
            pairVals[numPairsFoundSoFar] = sortedHand[i].Value;
            numPairsFoundSoFar++;
          }
        }
        remainingCardValue = "";
        // get value of remaining card
        foreach(StandardCard card in sortedHand) {
          if (!pairVals.Contains(card.Value)) {
            remainingCardValue = card.Value;
            break;
          }
        }
        tiebreaker1 = ValToInt(pairVals[0]);
        tiebreaker2 = ValToInt(pairVals[1]);
        tiebreaker3 = ValToInt(remainingCardValue);
        break;
      case PokerHand.OnePair:
        primaryScore = 2;
        // get the value of the pair
        pairValue = "";
        string[] cardVals = new string[4];
        for (int i=0; i<5; i++) {
          if (cardVals.Contains(sortedHand[i].Value)) {
            pairValue = sortedHand[i].Value;
            break;
          }
          else {
            cardVals[i] = sortedHand[i].Value;
          }
        }
        // get the other values
        List<string> remainingValues = new();
        foreach (StandardCard card in sortedHand) {
          if (pairValue != card.Value) {
            remainingValues.Add(card.Value);
          }
        }
        tiebreaker1 = ValToInt(pairValue);
        tiebreaker2 = ValToInt(remainingValues[0]);
        tiebreaker3 = ValToInt(remainingValues[1]);
        tiebreaker4 = ValToInt(remainingValues[2]);
        // tiebreaker 5 = 0
        break;
      case PokerHand.HighCard:
        primaryScore = 1;
        tiebreaker1 = ValToInt(sortedHand[0].Value);
        tiebreaker2 = ValToInt(sortedHand[1].Value);
        tiebreaker3 = ValToInt(sortedHand[2].Value);
        tiebreaker4 = ValToInt(sortedHand[3].Value);
        tiebreaker5 = ValToInt(sortedHand[4].Value);
        break;
      default:
        return 0;
    }
    return HexScore(primaryScore,tiebreaker1,tiebreaker2,tiebreaker3,tiebreaker4,tiebreaker5);
  }
  private int HexScore(int primaryScore, int tiebreaker1, int tiebreaker2, int tiebreaker3, int tiebreaker4, int tiebreaker5) {
    return
    primaryScore * 0x100000 +
    tiebreaker1  * 0x010000 +
    tiebreaker2  * 0x001000 +
    tiebreaker3  * 0x000100 +
    tiebreaker4  * 0x000010 +
    tiebreaker5  * 0x000001;
  }
  public List<StandardCard> GetBestHand7(List<StandardCard> sevenCardHand) {
    List<List<StandardCard>> allCombos = GetAll5ItemCombinationsOf7Items(sevenCardHand);
    List<StandardCard> bestHand = new();
    int bestScore = 0;
    int currentScore = 0;
    foreach(List<StandardCard> hand in allCombos) {
      currentScore = GetHandScore5(hand);
      if(currentScore > bestScore) {
        bestScore = currentScore;
        bestHand = new List<StandardCard>(hand);
      }
    }
    return bestHand;
  }
  public List<List<T>> GetAll5ItemCombinationsOf7Items<T>(List<T> sevenItems) {
    List<List<T>> allCombos = new();
    List<T> oneCombo = new();
    // simple way of getting each 5-card hand from the seven-card hand
    for (int exclude1 = 0; exclude1 < 7; exclude1++) {
      for (int exclude2 = 0; exclude2 < 7; exclude2++) {
        if (exclude1 < exclude2) {
          // get list excluding cards at index exclude1 and exclude2
          oneCombo.Clear();
          for (int i = 0; i < 7; i++) {
            if ((i != exclude1) && (i != exclude2)) {
              oneCombo.Add(sevenItems[i]);
            }
          }
          allCombos.Add(new List<T>(oneCombo)); // need to copy the list so that there's not just a reference to oneCombo, which results in 21 of the same list
        }
      }
    }
    return allCombos;
  }
}