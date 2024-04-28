using WebApi.Models;

namespace WebApi.Common;

public class CahPlayer : CardPlayer<CahCard>
{
    public List<CahCard> WonCards { get; }
    public Queue<CahCard> PickedCards { get; }
    public int Score => WonCards.Count;
    private bool HasPlayed() => PickedCards.Count > 0;
    private CahCard? _gambleCard;

    public CahPlayer(string name) : base(name)
    {
        WonCards = new List<CahCard>();
        PickedCards = new Queue<CahCard>();
    }

    public void AddWonCard(CahCard card)
    {
        if (card.Type == CahCardType.Black) WonCards.Add(card);
    }
    
    public bool EnqueueCards(List<CahCard> cards)
    {
        if (HasPlayed()) return false;
        
        foreach (var card in cards)
        {
            if (card.Type != CahCardType.White )
                return false;
            PickedCards.Enqueue(card);
        }
        return true;
    }
    
    public bool Gamble(CahCard whiteCard, CahCard blackCard)
    {
        if (!HasPlayed()) return false;
        if (PickedCards.Contains(whiteCard)) return false;

        WonCards.Remove(blackCard);
        _gambleCard = whiteCard;
        return true;
    }
    
    public List<CahCard> DequeueCards()
    {
        var cards = new List<CahCard>(PickedCards);
        PickedCards.Clear();
        return cards;
    }
    
}