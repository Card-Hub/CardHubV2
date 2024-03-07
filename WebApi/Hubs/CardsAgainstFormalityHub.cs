using WebApi.Common;

namespace WebApi.Hubs;


public partial class BaseHub
{
    public async Task ProcessCard(string message)
    {
        var state = new CardsAgainstFormalityState(new PlayerOrder(new List<string>()));
    }
}