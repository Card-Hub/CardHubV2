namespace WebApi.Common;

public class Deck<TCard> : IDeck<TCard>
{
    private List<TCard> _cards;
    private readonly List<TCard> _drawnCards;

    public Deck()
    {
        _cards = new List<TCard>();
        _drawnCards = new List<TCard>();
    }
        
    public void Shuffle()
    {
        _cards = _cards.OrderBy(x=> Random.Shared.Next()).ToList();
    }

    public TCard Draw()
    {
        if (_cards.Count == 0)
        {
            throw new InvalidOperationException("No cards to draw from the deck.");
        }
        var drawnCard = _cards.ElementAt(0);
        _cards.RemoveAt(0);
        _drawnCards.Add(drawnCard);
        return drawnCard;
    }

    public TCard Peek()
    {
        if (_cards.Count == 0)
        {
            throw new InvalidOperationException("No cards to peek from the deck.");
        }
        return _cards.ElementAt(0);
    }

    public List<TCard> DrawUntil(int amount)
    {
        if (_cards.Count < amount)
        {
            var errorMessage = $"Attempted to draw {amount} cards, but only {_cards.Count} cards are left in the deck.";
            throw new InvalidOperationException(errorMessage);
        }
        var drawnCards = _cards.Take(amount).ToList();
        _cards.RemoveRange(0, amount);
        _drawnCards.AddRange(drawnCards);
        return drawnCards;
    }

    public void ReclaimCards()
    {
        _cards.AddRange(_drawnCards);
        _drawnCards.Clear();
    }
}