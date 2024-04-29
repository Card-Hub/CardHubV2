using System.Security.Cryptography.X509Certificates;
using WebApi.GameLogic;
using WebApi.Models;
using Xunit.Abstractions;

namespace Tests.RandomTests;


[Trait("Category", "RandomTests")]
[Trait("Category", "PokerHandEvaluator")]
public class PokerHandEvaluatorTests {
  public ITestOutputHelper Output;
  public PokerHandEvaluator Evaluator;
  public PokerHandEvaluatorTests(ITestOutputHelper output) {
    Output = output;
    Evaluator = new PokerHandEvaluator();
  }
  [Fact]
  public void TestValToInt() {
    PokerHandEvaluator evaluator = new PokerHandEvaluator();
    Assert.Equal(3, Evaluator.ValToInt("3"));
    Assert.Equal(2, Evaluator.ValToInt("2"));
    Assert.Equal(4, Evaluator.ValToInt("4"));
    Assert.Equal(5, Evaluator.ValToInt("5"));
    Assert.Equal(6, Evaluator.ValToInt("6"));
    Assert.Equal(7, Evaluator.ValToInt("7"));
    Assert.Equal(8, Evaluator.ValToInt("8"));
    Assert.Equal(9, Evaluator.ValToInt("9"));
    Assert.Equal(10, Evaluator.ValToInt("10"));
    Assert.Equal(11, Evaluator.ValToInt("J"));
    Assert.Equal(12, Evaluator.ValToInt("Q"));
    Assert.Equal(13, Evaluator.ValToInt("K"));
    Assert.Equal(14, Evaluator.ValToInt("A"));
    Assert.Throws<ArgumentException>( () => Evaluator.ValToInt(""));
    Assert.Throws<ArgumentException>( () => Evaluator.ValToInt("-1"));
    Assert.Throws<ArgumentException>( () => Evaluator.ValToInt("11"));
    Assert.Throws<ArgumentException>( () => Evaluator.ValToInt("B"));
  }
  // https://stackoverflow.com/questions/61658357/is-it-possible-to-add-inline-c-sharp-objects-to-a-theory-in-xunit
  // looks like it needs to be object[] and not StandardCard[]
  public static IEnumerable<object[]> RoyalFlushTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Hearts", "A"),
          new StandardCard(0, "Hearts", "K"),
          new StandardCard(0, "Hearts", "Q"),
          new StandardCard(0, "Hearts", "J"),
          new StandardCard(0, "Hearts", "10")
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "Q"),
          new StandardCard(0, "Spades", "A"),
          new StandardCard(0, "Spades", "10"),
          new StandardCard(0, "Spades", "J"),
          new StandardCard(0, "Spades", "K")
        }
      };
    }
  }
  [Theory]
  [MemberData(nameof(RoyalFlushTestCases))]
  public void TestRoyalFlush(List<StandardCard> hand) {
    Assert.Equal(PokerHand.RoyalFlush, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> StraightFlushTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "2"),
          new StandardCard(0, "Clubs", "3"),
          new StandardCard(0, "Clubs", "4"),
          new StandardCard(0, "Clubs", "5"),
          new StandardCard(0, "Clubs", "6")
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "Q"),
          new StandardCard(0, "Spades", "J"),
          new StandardCard(0, "Spades", "10"),
          new StandardCard(0, "Spades", "9"),
          new StandardCard(0, "Spades", "8")
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "10"),
          new StandardCard(0, "Spades", "J"),
          new StandardCard(0, "Spades", "9"),
          new StandardCard(0, "Spades", "8"),
          new StandardCard(0, "Spades", "Q")
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(StraightFlushTestCases))]
  public void TestStraightFlush(List<StandardCard> hand) {
    Assert.Equal(PokerHand.StraightFlush, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> FourOfAKindTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "2"),
          new StandardCard(0, "Spades", "2"),
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "Q"),
          new StandardCard(0, "Hearts", "A"),
          new StandardCard(0, "Spades", "Q"),
          new StandardCard(0, "Hearts", "Q"),
          new StandardCard(0, "Diamonds", "Q"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "Q"),
          new StandardCard(0, "Hearts", "A"),
          new StandardCard(0, "Clubs", "Q"),
          new StandardCard(0, "Clubs", "Q"),
          new StandardCard(0, "Clubs", "Q"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(FourOfAKindTestCases))]
  public void TestFourOfAKind(List<StandardCard> hand) {
    Assert.Equal(PokerHand.FourOfAKind, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> FullHouseTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "2"),
          new StandardCard(0, "Spades", "2"),
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "2"),
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Spades", "Q"),
          new StandardCard(0, "Hearts", "Q"),
          new StandardCard(0, "Diamonds", "Q"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "10"),
          new StandardCard(0, "Hearts", "A"),
          new StandardCard(0, "Clubs", "10"),
          new StandardCard(0, "Clubs", "A"),
          new StandardCard(0, "Clubs", "10"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(FullHouseTestCases))]
  public void TestFullHouse(List<StandardCard> hand) {
    Assert.Equal(PokerHand.FullHouse, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> FlushTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Diamonds", "6"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Hearts", "Q"),
          new StandardCard(0, "Hearts", "3"),
          new StandardCard(0, "Hearts", "K"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Spades", "K"),
          new StandardCard(0, "Spades", "J"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(FlushTestCases))]
  public void TestFlush(List<StandardCard> hand) {
    Assert.Equal(PokerHand.Flush, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> StraightTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Clubs", "3"),
          new StandardCard(0, "Diamonds", "4"),
          new StandardCard(0, "Diamonds", "5"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Hearts", "3"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Spades", "5"),
          new StandardCard(0, "Spades", "A"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Spades", "7"),
          new StandardCard(0, "Hearts", "8"),
          new StandardCard(0, "Diamonds", "9"),
          new StandardCard(0, "Hearts", "10"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "7"),
          new StandardCard(0, "Diamonds", "10"),
          new StandardCard(0, "Clubs", "8"),
          new StandardCard(0, "Clubs", "9"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(StraightTestCases))]
  public void TestStraight(List<StandardCard> hand) {
    Assert.Equal(PokerHand.Straight, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> ThreeOfAKindTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "4"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Clubs", "4"),
          new StandardCard(0, "Diamonds", "4"),
          new StandardCard(0, "Diamonds", "J"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Hearts", "3"),
          new StandardCard(0, "Diamonds", "3"),
          new StandardCard(0, "Spades", "2"),
          new StandardCard(0, "Spades", "3"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Spades", "7"),
          new StandardCard(0, "Hearts", "A"),
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Hearts", "A"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Clubs", "7"),
          new StandardCard(0, "Diamonds", "7"),
          new StandardCard(0, "Clubs", "7"),
          new StandardCard(0, "Clubs", "9"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(ThreeOfAKindTestCases))]
  public void TestThreeOfAKind(List<StandardCard> hand) {
    Assert.Equal(PokerHand.ThreeOfAKind, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> TwoPairTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "K"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Clubs", "4"),
          new StandardCard(0, "Diamonds", "4"),
          new StandardCard(0, "Diamonds", "2"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Hearts", "4"),
          new StandardCard(0, "Diamonds", "3"),
          new StandardCard(0, "Spades", "2"),
          new StandardCard(0, "Spades", "3"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Spades", "7"),
          new StandardCard(0, "Hearts", "A"),
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Hearts", "A"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "7"),
          new StandardCard(0, "Clubs", "9"),
          new StandardCard(0, "Clubs", "7"),
          new StandardCard(0, "Clubs", "9"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(TwoPairTestCases))]
  public void TestTwoPair(List<StandardCard> hand) {
    Assert.Equal(PokerHand.TwoPair, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> OnePairTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "K"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Clubs", "4"),
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Diamonds", "2"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Hearts", "4"),
          new StandardCard(0, "Diamonds", "5"),
          new StandardCard(0, "Spades", "2"),
          new StandardCard(0, "Spades", "3"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Spades", "7"),
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Hearts", "A"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "7"),
          new StandardCard(0, "Clubs", "9"),
          new StandardCard(0, "Clubs", "7"),
          new StandardCard(0, "Clubs", "J"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(OnePairTestCases))]
  public void TestOnePair(List<StandardCard> hand) {
    Assert.Equal(PokerHand.OnePair, Evaluator.GetHandType5(hand));
  }
   public static IEnumerable<object[]> HighCardTestCases
  {
    get
    {
    // each test case is a yield return array
    // even if it shouldn't be an array, lol
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "K"),
          new StandardCard(0, "Diamonds", "2"),
          new StandardCard(0, "Clubs", "4"),
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Diamonds", "Q"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Spades", "4"),
          new StandardCard(0, "Hearts", "Q"),
          new StandardCard(0, "Diamonds", "5"),
          new StandardCard(0, "Spades", "2"),
          new StandardCard(0, "Spades", "3"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "6"),
          new StandardCard(0, "Spades", "7"),
          new StandardCard(0, "Hearts", "2"),
          new StandardCard(0, "Diamonds", "A"),
          new StandardCard(0, "Hearts", "10"),
        }
      };
      yield return new object[]
      {
        new List<StandardCard>() {
          new StandardCard(0, "Diamonds", "7"),
          new StandardCard(0, "Clubs", "9"),
          new StandardCard(0, "Clubs", "10"),
          new StandardCard(0, "Clubs", "J"),
          new StandardCard(0, "Clubs", "6"),
        }
      };
    }
}
  [Theory]
  [MemberData(nameof(HighCardTestCases))]
  public void TestHighCard(List<StandardCard> hand) {
    Assert.Equal(PokerHand.HighCard, Evaluator.GetHandType5(hand));
  }
}