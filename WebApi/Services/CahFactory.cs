using Microsoft.AspNetCore.SignalR;
using WebApi.GameLogic;
using WebApi.Hubs;

namespace WebApi.Services;

public class CahFactory
{
    private IServiceProvider _serviceProvider;
    public CahFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public CahGame Build()
    {
        return _serviceProvider.GetRequiredService<CahGame>();
    }
}