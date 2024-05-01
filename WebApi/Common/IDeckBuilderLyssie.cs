using WebApi.Models;

namespace WebApi.Common;

public interface IDeckBuilderLyssie<TCard, in TSettings>
{
    IDeckLyssie<TCard> Build(TSettings settings);
}