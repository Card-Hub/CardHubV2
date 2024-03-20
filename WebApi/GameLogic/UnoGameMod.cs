using WebApi.Common;
using WebApi.Models;

namespace WebApi.GameLogic;

public class UnoGameMod
{
    private Deck<UnoCard> _deck;
    private PlayerOrder<UnoPlayer, UnoCard> _playerOrder;

    public UnoGameMod()
    {
        
    }
}