namespace WebApi.Common;

public class DeckLyssie<TCard> : IDeckLyssie<TCard>
{
    protected List<TCard> _cards;
    protected List<TCard> _drawnCards;

    // for json purposes only
    public List<TCard> Cards { get { return _cards; } }
    public List<TCard> DrawnCards { get { return _drawnCards; } }
    
    public DeckLyssie()
    {
        _cards = new List<TCard>();
        _drawnCards = new List<TCard>();
    }
    
    public DeckLyssie(IEnumerable<TCard> cards)
    {
        _cards = new List<TCard>(cards);
        _drawnCards = new List<TCard>();
    }
    
    public void Add(TCard card)
    {
        _cards.Add(card);
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

    public List<TCard> Draw(int amount)
    {
        if (_cards.Count < amount)
        {
            var errorMessage = $"Attempted to draw {amount} cards, but only {_cards.Count} cards are left in the deck.";
            throw new InvalidOperationException(errorMessage);
        }
        if (amount < 1)
        {
            throw new InvalidOperationException("Attempted to draw less than 1 card from the deck.");
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

  public List<TCard> GetCards()
  {
    return _cards;
  }
}