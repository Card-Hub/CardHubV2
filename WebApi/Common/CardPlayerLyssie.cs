using Newtonsoft.Json;

namespace WebApi.Common;

public class CardPlayerLyssie<TCard> : ICardPlayer<TCard>
{
  // DON'T TOUCH THESE pls
  // retaining _hand and making Hand to avoid modifying this too much.
  // these attributes will go before before child attributes:
    [JsonProperty(Order = -10)]
    public string Name { get; init; }
    [JsonProperty(Order = -10)]
    public string Avatar { get; set; }
    [JsonProperty(Order = -10)]
    public bool Afk { get; set;}
    // these attributes will go after child attributes:
    [JsonProperty(Order = 10)]
    public List<TCard> Hand { get { return _hand; } }

    // these attributes are private and won't be seen
    private string ConnectionString;
    private List<TCard> _hand;
    
    public CardPlayerLyssie(string name, string connectionString)
    {
        Name = name;
        Avatar = "";
        Afk = false;
        _hand = new List<TCard>();
        ConnectionString = connectionString;
    }
    
    public CardPlayerLyssie(string name, string connectionString, IEnumerable<TCard> cards)
    {
        Name = name;
        Avatar = "";
        _hand = new List<TCard>(cards);
        ConnectionString = connectionString;
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
        if (!_hand.Contains(card)) {
          Console.WriteLine("HAND DOESN'T CONTAIN CARD???");
          return false;
        }
        
        _hand.Remove(card);
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
    
    public void ClearHand()
    {
        _hand.Clear();
    }
}