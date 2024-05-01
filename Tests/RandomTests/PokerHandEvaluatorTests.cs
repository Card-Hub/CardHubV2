using System.Security.Cryptography.X509Certificates;
using WebApi.GameLogic;
using WebApi.Models;
using Xunit.Abstractions;

using Newtonsoft.Json;
using System.ComponentModel;
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

	//
	//
	//
	// SCORE
	//
	//
	//

	public static IEnumerable<object[]> RoyalFlushScoreTestCases
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
				},
				0xA00000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "Q"),
					new StandardCard(0, "Spades", "A"),
					new StandardCard(0, "Spades", "10"),
					new StandardCard(0, "Spades", "J"),
					new StandardCard(0, "Spades", "K")
				},
				0xA00000
			};
		}
	}
	[Theory]
	[MemberData(nameof(RoyalFlushScoreTestCases))]
	public void TestRoyalFlushScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
	public static IEnumerable<object[]> StraightFlushScoreTestCases
	{
		get
		{
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "2"),
					new StandardCard(0, "Clubs", "3"),
					new StandardCard(0, "Clubs", "4"),
					new StandardCard(0, "Clubs", "5"),
					new StandardCard(0, "Clubs", "6")
				},
				0x960000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "Q"),
					new StandardCard(0, "Spades", "J"),
					new StandardCard(0, "Spades", "10"),
					new StandardCard(0, "Spades", "9"),
					new StandardCard(0, "Spades", "8")
				},
				0x9c0000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "10"),
					new StandardCard(0, "Spades", "J"),
					new StandardCard(0, "Spades", "9"),
					new StandardCard(0, "Spades", "8"),
					new StandardCard(0, "Spades", "Q")
				},
				0x9c0000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "A"),
					new StandardCard(0, "Spades", "5"),
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Spades", "3"),
					new StandardCard(0, "Spades", "2")
				},
				0x950000
			};
		}
	}
	[Theory]
	[MemberData(nameof(StraightFlushScoreTestCases))]
	public void TestStraightFlushScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
	public static IEnumerable<object[]> FourOfAKindScoreTestCases
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
				},
				0x826000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "Q"),
					new StandardCard(0, "Hearts", "A"),
					new StandardCard(0, "Spades", "Q"),
					new StandardCard(0, "Hearts", "Q"),
					new StandardCard(0, "Diamonds", "Q"),
				},
				0x8ce000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "A"),
					new StandardCard(0, "Hearts", "A"),
					new StandardCard(0, "Clubs", "5"),
					new StandardCard(0, "Clubs", "A"),
					new StandardCard(0, "Clubs", "A"),
				},
				0x8e5000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "3"),
					new StandardCard(0, "Hearts", "2"),
					new StandardCard(0, "Clubs", "2"),
					new StandardCard(0, "Clubs", "2"),
					new StandardCard(0, "Clubs", "2"),
				},
				0x823000
			};
		}
	}
	[Theory]
	[MemberData(nameof(FourOfAKindScoreTestCases))]
	public void TestFourOfAKindScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
	public static IEnumerable<object[]> FullHouseScoreTestCases
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
				},
				0x726000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "2"),
					new StandardCard(0, "Hearts", "2"),
					new StandardCard(0, "Spades", "Q"),
					new StandardCard(0, "Hearts", "Q"),
					new StandardCard(0, "Diamonds", "Q"),
				},
				0x7c2000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "10"),
					new StandardCard(0, "Hearts", "A"),
					new StandardCard(0, "Clubs", "10"),
					new StandardCard(0, "Clubs", "A"),
					new StandardCard(0, "Clubs", "10"),
				},
				0x7ae000
			};
		}
	}
	[Theory]
  [MemberData(nameof(FullHouseScoreTestCases))]
  public void TestFullHouseScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
	public static IEnumerable<object[]> FlushScoreTestCases
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
				},
				0x6e6622
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Hearts", "2"),
					new StandardCard(0, "Hearts", "2"),
					new StandardCard(0, "Hearts", "Q"),
					new StandardCard(0, "Hearts", "3"),
					new StandardCard(0, "Hearts", "K"),
				},
				0x6dc322
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Spades", "K"),
					new StandardCard(0, "Spades", "J"),
				},
				0x6db444
			};
		}
	}
	[Theory]
  [MemberData(nameof(FlushScoreTestCases))]
  public void TestFlushScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
	public static IEnumerable<object[]> StraightScoreTestCases
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
				},
				0x550000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Hearts", "3"),
					new StandardCard(0, "Diamonds", "2"),
					new StandardCard(0, "Spades", "5"),
					new StandardCard(0, "Spades", "A"),
				},
				0x550000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "6"),
					new StandardCard(0, "Spades", "7"),
					new StandardCard(0, "Hearts", "8"),
					new StandardCard(0, "Diamonds", "9"),
					new StandardCard(0, "Hearts", "10"),
				},
				0x5a0000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "7"),
					new StandardCard(0, "Diamonds", "10"),
					new StandardCard(0, "Clubs", "8"),
					new StandardCard(0, "Clubs", "9"),
					new StandardCard(0, "Clubs", "6"),
				},
				0x5a0000
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "3"),
					new StandardCard(0, "Diamonds", "4"),
					new StandardCard(0, "Clubs", "7"),
					new StandardCard(0, "Clubs", "5"),
					new StandardCard(0, "Clubs", "6"),
				},
				0x570000
			};
		}
	}
	[Theory]
  [MemberData(nameof(StraightScoreTestCases))]
  public void TestStraightScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
  public static IEnumerable<object[]> ThreeOfAKindScoreTestCases
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
				},
        0x44b200
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Hearts", "3"),
					new StandardCard(0, "Diamonds", "3"),
					new StandardCard(0, "Spades", "2"),
					new StandardCard(0, "Spades", "3"),
				},
        0x434200
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "6"),
					new StandardCard(0, "Spades", "7"),
					new StandardCard(0, "Hearts", "A"),
					new StandardCard(0, "Diamonds", "A"),
					new StandardCard(0, "Hearts", "A"),
				},
        0x4e7600
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Clubs", "7"),
					new StandardCard(0, "Diamonds", "7"),
					new StandardCard(0, "Clubs", "7"),
					new StandardCard(0, "Clubs", "9"),
					new StandardCard(0, "Clubs", "6"),
				},
        0x479600
			};
		}
	}
  [Theory]
  [MemberData(nameof(ThreeOfAKindScoreTestCases))]
  public void TestThreeOfAKindScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
  public static IEnumerable<object[]> TwoPairScoreTestCases
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
				},
        0x342d00
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Hearts", "4"),
					new StandardCard(0, "Diamonds", "3"),
					new StandardCard(0, "Spades", "2"),
					new StandardCard(0, "Spades", "3"),
				},
        0x343200
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "6"),
					new StandardCard(0, "Spades", "7"),
					new StandardCard(0, "Hearts", "A"),
					new StandardCard(0, "Diamonds", "6"),
					new StandardCard(0, "Hearts", "A"),
				},
        0x3e6700
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "7"),
					new StandardCard(0, "Clubs", "9"),
					new StandardCard(0, "Clubs", "7"),
					new StandardCard(0, "Clubs", "9"),
					new StandardCard(0, "Clubs", "6"),
				},
        0x397600
			};
		}
	}
  [Theory]
  [MemberData(nameof(TwoPairScoreTestCases))]
  public void TestTwoPairScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
  public static IEnumerable<object[]> OnePairScoreTestCases
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
				},
        0x22ed40
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Hearts", "4"),
					new StandardCard(0, "Diamonds", "5"),
					new StandardCard(0, "Spades", "2"),
					new StandardCard(0, "Spades", "3"),
				},
        0x245320
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "6"),
					new StandardCard(0, "Spades", "7"),
					new StandardCard(0, "Hearts", "2"),
					new StandardCard(0, "Diamonds", "A"),
					new StandardCard(0, "Hearts", "A"),
				},
        0x2e7620
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "7"),
					new StandardCard(0, "Clubs", "9"),
					new StandardCard(0, "Clubs", "7"),
					new StandardCard(0, "Clubs", "J"),
					new StandardCard(0, "Clubs", "6"),
				},
        0x27b960
			};
		}
	}
  [Theory]
  [MemberData(nameof(OnePairScoreTestCases))]
  public void TestOnePairScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}
  public static IEnumerable<object[]> HighCardScoreTestCases
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
				},
        0x1edc42
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Spades", "4"),
					new StandardCard(0, "Hearts", "Q"),
					new StandardCard(0, "Diamonds", "5"),
					new StandardCard(0, "Spades", "2"),
					new StandardCard(0, "Spades", "3"),
				},
        0x1c5432
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "6"),
					new StandardCard(0, "Spades", "7"),
					new StandardCard(0, "Hearts", "2"),
					new StandardCard(0, "Diamonds", "A"),
					new StandardCard(0, "Hearts", "10"),
				},
        0x1ea762
			};
			yield return new object[]
			{
				new List<StandardCard>() {
					new StandardCard(0, "Diamonds", "7"),
					new StandardCard(0, "Clubs", "9"),
					new StandardCard(0, "Clubs", "10"),
					new StandardCard(0, "Clubs", "J"),
					new StandardCard(0, "Clubs", "6"),
				},
        0x1ba976
			};
		}
	}
	[Theory]
	[MemberData(nameof(HighCardScoreTestCases))]
  public void TestHighCardScores(List<StandardCard> hand, int expectedValue) {
		Assert.Equal(expectedValue, Evaluator.GetHandScore5(hand));
	}

  [Fact]
  public void TestGetAll5ItemCombosOf7() {
    List<string> strings7 = new List<string> { "A", "B", "C", "D", "E", "F", "G"};
    List<List<string>> expectedListOf5s = new List<List<string>> {
      new List<string>() {"C", "D", "E", "F", "G"}, // 1 2 
      new List<string>() {"B", "D", "E", "F", "G"}, // 1 3
      new List<string>() {"B", "C", "E", "F", "G"}, // 1 4
      new List<string>() {"B", "C", "D", "F", "G"}, // 1 5
      new List<string>() {"B", "C", "D", "E", "G"}, // 1 6
      new List<string>() {"B", "C", "D", "E", "F"}, // 1 7
      new List<string>() {"A", "D", "E", "F", "G"}, // 2 3
      new List<string>() {"A", "C", "E", "F", "G"}, // 2 4
      new List<string>() {"A", "C", "D", "F", "G"}, // 2 5
      new List<string>() {"A", "C", "D", "E", "G"}, // 2 6
      new List<string>() {"A", "C", "D", "E", "F"}, // 2 7
      new List<string>() {"A", "B", "E", "F", "G"}, // 3 4
      new List<string>() {"A", "B", "D", "F", "G"}, // 3 5
      new List<string>() {"A", "B", "D", "E", "G"}, // 3 6
      new List<string>() {"A", "B", "D", "E", "F"}, // 3 7
      new List<string>() {"A", "B", "C", "F", "G"}, // 4 5
      new List<string>() {"A", "B", "C", "E", "G"}, // 4 6
      new List<string>() {"A", "B", "C", "E", "F"}, // 4 7
      new List<string>() {"A", "B", "C", "D", "G"}, // 5 6
      new List<string>() {"A", "B", "C", "D", "F"}, // 5 7
      new List<string>() {"A", "B", "C", "D", "E"} // 6 7
    };
    List<List<string>> allCombosOf5 = Evaluator.GetAll5ItemCombinationsOf7Items(strings7);
    // 7c5 = 21
    Assert.Equal(21, allCombosOf5.Count); 
    Assert.Equal(21, expectedListOf5s.Count);
    for (int i = 0; i < 21; i++) {
      Assert.Equal(expectedListOf5s[i], allCombosOf5[i]);
    }
  }
  public static IEnumerable<object[]> BestHandAndScoreTestCases
	{
		get
		{
			yield return new object[] // High card
			{
				new List<StandardCard>() {
					new StandardCard(1, "Diamonds", "K"),
					new StandardCard(2, "Diamonds", "2"),
					new StandardCard(3, "Clubs", "4"),
					new StandardCard(4, "Diamonds", "A"),
					new StandardCard(5, "Diamonds", "Q"),
          new StandardCard(6, "Hearts", "6"),
          new StandardCard(7, "Hearts", "J")
				},
        new List<StandardCard>() {
          new StandardCard(1, "Diamonds", "K"),
          new StandardCard(4, "Diamonds", "A"),
          new StandardCard(5, "Diamonds", "Q"),
          new StandardCard(6, "Hearts", "6"),
          new StandardCard(7, "Hearts", "J"),
        },
        0x1edcb6
			};
      yield return new object[] // one pair
			{
				new List<StandardCard>() {
					new StandardCard(1, "Clubs", "K"),
					new StandardCard(2, "Diamonds", "2"),
					new StandardCard(3, "Clubs", "4"),
					new StandardCard(4, "Diamonds", "A"),
					new StandardCard(5, "Hearts", "6"),
					new StandardCard(6, "Diamonds", "2"),
					new StandardCard(7, "Hearts", "8"),
				},
				new List<StandardCard>() {
					new StandardCard(1, "Clubs", "K"),
					new StandardCard(2, "Diamonds", "2"),
					new StandardCard(4, "Diamonds", "A"),
					new StandardCard(6, "Diamonds", "2"),
					new StandardCard(7, "Hearts", "8"),
				},
        0x22ed80
			};
		}
	}
  [Theory]
  [MemberData(nameof(BestHandAndScoreTestCases))]
  [Trait("testing", "rnrn")]
  public void TestBestHand(List<StandardCard> cards7, List<StandardCard> bestHand, int expectedScore) {

    List<StandardCard> actualBestHand = Evaluator.GetBestHand7(cards7).OrderByDescending(x=>Evaluator.ValToInt(x.Value)).ToList();
    Output.WriteLine("Best hand:");

    for (int i = 0; i < 5; i++) {
      Output.WriteLine(JsonConvert.SerializeObject(bestHand[i]));
    }
    Output.WriteLine("Actual best hand:");
    Output.WriteLine($"{actualBestHand.Count}");

    for (int i = 0; i < 5; i++) {
      Output.WriteLine(JsonConvert.SerializeObject(actualBestHand[i]));
    }
    Assert.Equivalent(bestHand.OrderByDescending(x=>Evaluator.ValToInt(x.Value)).ToList(), actualBestHand);
    Assert.Equal(expectedScore, Evaluator.GetHandScore5(Evaluator.GetBestHand7(cards7)));  
  }
}