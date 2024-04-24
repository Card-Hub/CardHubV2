using WebApi.Models;

namespace WebApi.Common;

public class UnoPlayer : CardPlayer<UnoCardMod>
{
    public UnoPlayer(string name) : base(name)
    {
    }
}