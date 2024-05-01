using WebApi.Models;

namespace WebApi.Common;

public class PokerPlayer : CardPlayer<StandardCard>
{
    public bool IsDealer { get; set; }
    public int Chips { get; set; }

    public PokerPlayer(string name, int chips) : base(name)
    {
        Chips = chips;
    }
    
    public PokerPlayer(string name, int chips, IEnumerable<StandardCard> cards) : base(name, cards)
    {
        Chips = chips;
    }
    
    
}