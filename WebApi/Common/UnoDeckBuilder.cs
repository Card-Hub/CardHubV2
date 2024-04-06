using WebApi.Models;

namespace WebApi.Common;

public class UnoSettings
{
    
}

public class UnoDeckBuilder : IDeckBuilder<UnoCard, UnoSettings>
{
    public IDeck<UnoCard> Build(UnoSettings settings)
    {
        var deck = new Deck<UnoCard>();
        
        string[] colors = ["red", "blue", "yellow", "green"];
        string[] coloredValues = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Draw Two", "Draw Two", "Reverse", "Reverse", "Skip", "Skip", "Skip All", "Skip All"];
        string[] wildCardValues = ["Wild", "Wild Draw Four"];
        var cardId = 0;
        
        foreach (var color in colors) {
            foreach (var value in coloredValues) {
                var newCard = new UnoCard
                {
                    Id = cardId,
                    Value = value,
                    Color = color
                };
                deck.Add(newCard);
                cardId++;
            }
        }
        
        foreach (var wildCardValue in wildCardValues) {
            for (var i = 0; i < 4; i++) {
                var newCard = new UnoCard
                {
                    Id = cardId,
                    Value = wildCardValue,
                    Color = "black"
                };
                deck.Add(newCard);
                cardId++;
            }
        }
        
        return deck;
    }
}