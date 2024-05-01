using WebApi.Models;

namespace WebApi.Common;

public interface IDeckBuilder<TCard, in TSettings>
{
    IDeck<TCard> Build(TSettings settings);
}