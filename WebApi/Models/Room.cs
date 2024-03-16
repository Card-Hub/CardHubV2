using WebApi.GameLogic;
using WebApi.Hubs;

namespace WebApi.Models;

public class Room
{
    public IBaseGame<UnoCard> Game { get; set; }
    public List<UserConnection> UserConnections { get; set; }
    public string Code { get; set; }
}