using WebApi.Models;

namespace WebApi.Common;

public class UnoPlayer : CardPlayer<UnoCard>
{
    public UnoPlayer(string name) : base(name)
    {
    }
}