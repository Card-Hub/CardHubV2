using WebApi.Models;

namespace WebApi.Common;

public class UnoPlayer : CardPlayer<UnoCard>
{
    public UnoPlayer(string name, List<UnoCard> cards) : base(name, cards)
    {
    }
}