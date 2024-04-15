using WebApi.Models;

namespace WebApi.Common;

public class UnoSettings
{
}

public class UnoDeckBuilder : IDeckBuilder<UnoCardMod, UnoSettings>
{
    public IDeck<UnoCardMod> Build(UnoSettings settings)
    {
        var deck = new Deck<UnoCardMod>();
        var cardId = 0;

        var nonWildValues = Enum.GetValues(typeof(UnoValue)).Cast<UnoValue>()
            .Where(v => v != UnoValue.Wild && v != UnoValue.WildDrawFour).ToArray();
        List<UnoColor> normalColors = [UnoColor.Blue, UnoColor.Green, UnoColor.Red, UnoColor.Yellow];
        foreach (var color in normalColors)
        {
            foreach (var value in nonWildValues)
            {
                deck.Add(new UnoCardMod(cardId, color, value));
                cardId++;
            }
        }

        List<UnoValue> wildCardValues = [UnoValue.Wild, UnoValue.WildDrawFour];
        foreach (var wildCardValue in wildCardValues)
        {
            for (var i = 0; i < 4; i++)
            {
                deck.Add(new UnoCardMod(cardId, UnoColor.Black, wildCardValue));
                cardId++;
            }
        }

        return deck;
    }
}