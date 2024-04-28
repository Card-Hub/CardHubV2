namespace WebApi.Common;

public class CardPlayer<TCard> : ICardPlayer<TCard>
{
    public string Name { get; init; }
    private List<TCard> _hand;
    
    public CardPlayer(string name)
    {
        Name = name;
        _hand = new List<TCard>();
    }
    
    public CardPlayer(string name, IEnumerable<TCard> cards)
    {
        Name = name;
        _hand = new List<TCard>(cards);
    }
    
    public void AddCard(TCard card)
    {
        _hand.Add(card);
    }
    
    public void AddCards(IEnumerable<TCard> cards)
    {
        _hand.AddRange(cards);
    }
    
    public List<TCard> GetHand()
    {
        return _hand;
    }

    public bool RemoveCard(TCard card)
    {
        if (!_hand.Contains(card)) return false;
        
        _hand.Remove(card);
        return true;
    }
    
    public bool RemoveCard(int index)
    {
        if (index < 0 || index >= _hand.Count) return false;
        
        _hand.RemoveAt(index);
        return true;
    }
    
    public bool RemoveCards(IEnumerable<TCard> cards)
    {
        var success = true;
        foreach (var card in cards)
        {
            if (!RemoveCard(card)) success = false;
        }
        return success;
    }
    
    public int HandCount()
    {
        return _hand.Count;
    }
    
    public void ClearHand()
    {
        _hand.Clear();
    }
    
    public TCard PickRandomCard()
    {
        var random = new Random();
        var index = random.Next(0, _hand.Count);
        var card = _hand[index];
        RemoveCard(index);
        return card;
    }
}