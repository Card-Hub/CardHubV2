using WebApi.Models;

namespace WebApi.Common;

public class UnoSettingsLyssie
{
  public bool UseSkipAll {get;set;} = false;
}

public class UnoDeckBuilderLyssie : IDeckBuilderLyssie<UnoCardModLyssie, UnoSettingsLyssie>
{
  /*
  A standard deck of Uno cards consists of 108 cards:

25 cards of each color (red, blue, green, and yellow)
19 number cards (1 zero and 2 each of one through nine)
2 Draw 2 cards
2 Reverse cards
2 Skip cards
4 Wild cards
4 Wild Draw 4 cards
*/
  public IDeckLyssie<UnoCardModLyssie> Build(UnoSettingsLyssie settings)
  {
    var deck = new DeckLyssie<UnoCardModLyssie>();
    var cardId = 0;

    // add regular cards
    var normalCardValues = Enum.GetValues(typeof(UnoValueLyssie)).Cast<UnoValueLyssie>()
      .Where(v => (v is not UnoValueLyssie.Wild) && (v is not UnoValueLyssie.WildDrawFour)).ToArray();
    var normalCardColors = Enum.GetValues(typeof(UnoColorLyssie)).Cast<UnoColorLyssie>().Where(v => v is not UnoColorLyssie.Black);
    foreach (var color in normalCardColors)
    {
      foreach (var value in normalCardValues)
      {
        // handle Skip All config; if true, then add 2 (per color), else don't add any to the deck
        if (value is not UnoValueLyssie.SkipAll ||
           (value is UnoValueLyssie.SkipAll && settings.UseSkipAll)) {

          // if 0, only add it once
          if (value == UnoValueLyssie.Zero) {
            deck.Add(new UnoCardModLyssie(cardId, color, value));
            cardId++;
          }
          // otherwise, add it twice
          else {
            for (int i = 0; i < 2; i++) {
              deck.Add(new UnoCardModLyssie(cardId, color, value));
              cardId++;
            }
          }
        }
      }
    }
    // add wild card vals
    var wildCardValues = Enum.GetValues(typeof(UnoValueLyssie)).Cast<UnoValueLyssie>()
      .Where(v => v is UnoValueLyssie.Wild or UnoValueLyssie.WildDrawFour).ToArray();
    foreach (var wildCardValue in wildCardValues)
    {
      for (var i = 0; i < 4; i++)
      {
        deck.Add(new UnoCardModLyssie(cardId, UnoColorLyssie.Black, wildCardValue));
        cardId++;
      }
    }
    return deck;
  }
}