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

        var normalCardValues = Enum.GetValues(typeof(UnoValue)).Cast<UnoValue>()
            .Where(v => v is not UnoValue.Wild and UnoValue.WildDrawFour).ToArray();
        foreach (var color in Enum.GetValues(typeof(UnoColor)).Cast<UnoColor>())
        {
            foreach (var value in normalCardValues)
            {
                deck.Add(new UnoCardMod(cardId, color, value));
                cardId++;
            }
        }

        var wildCardValues = Enum.GetValues(typeof(UnoValue)).Cast<UnoValue>()
            .Where(v => v is UnoValue.Wild or UnoValue.WildDrawFour).ToArray();
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