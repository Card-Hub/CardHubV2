using WebApi.Models;

namespace WebApi.Common;

public class CahPlayer : CardPlayer<CahCard>
{
    public List<CahCard> WonCards { get; }
    public Queue<CahCard> PickedCards { get; }

    public CahPlayer(string name) : base(name)
    {
    }

    public void AddWonCard(CahCard card)
    {
        if (card.Type == CahCardType.Black)
            WonCards.Add(card);
    }

    public CahCard PickRandomCard()
    {
        var random = new Random();
        var index = random.Next(0, HandCount());
        var card = GetHand()[index];
        RemoveCard(index);
        return card;
    }
}