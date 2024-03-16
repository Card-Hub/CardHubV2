using WebApi.Models;

namespace WebApi.GameLogic;

interface iPokerEvaluation {
  /// <summary>
  /// Evaluate 2 hands. Generally used to compare each player's best hand.
  /// </summary>
  /// <param name="hand1"></param>
  /// <param name="hand2"></param>
  /// <returns><c>1</c> if hand1 is better, <c>-1</c> if hand2 is better, <c>0</c> if they are equal.</returns>
  int CompareHands(List<StandardCard> hand1, List<StandardCard> hand2);
  int GetBestHand(List<StandardCard> fiveOrMoreCards);

}