using WebApi.Models;

namespace WebApi.Common;

public class UnoSettings
{
    
}

public class UnoDeckFactory : IDeckFactory<UnoCard, UnoSettings>
{
    public IDeck<UnoCard> CreateDeck(UnoSettings settings)
    {
        
    }
}