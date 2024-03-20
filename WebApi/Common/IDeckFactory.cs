using WebApi.Models;

namespace WebApi.Common;

public interface IDeckFactory<TCard, in TSettings>
{
    IDeck<TCard> CreateDeck(TSettings settings);
}