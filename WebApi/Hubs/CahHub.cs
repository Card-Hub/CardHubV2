using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;

namespace WebApi.Hubs;

public record UserConnectionMod
{
    public string Name { get; set; }
    public string Room { get; set; }
}


public class CahHub : Hub
{
    private const string Name = "Name";
    private const string Room = "Room";
    
    public CahHub()
    {
        
    }

    public async Task JoinRoom(UserConnectionMod connection)
    {
        Context.Items[Name] = connection.Name;
        Context.Items[Room] = connection.Room;

        await Groups.AddToGroupAsync(Context.ConnectionId, connection.Room);
        
    }

    public async Task SendCard(CahCard card)
    {
        
    }
    
    
}