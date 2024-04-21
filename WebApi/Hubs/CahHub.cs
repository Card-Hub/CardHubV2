using Microsoft.AspNetCore.SignalR;
using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Services;

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
    private const string PlayerType = "PlayerType";

    private IDictionary<string, CahGame?> _games;
    private readonly CahFactory _factory;

    public CahHub(IDictionary<string, CahGame?> games, CahFactory factory)
    {
        _games = games;
        _factory = factory;
    }

    public async Task JoinRoom(UserConnectionMod connection)
    {
        if (TryGetGame(out var game))
        {
            if (game is null) return;
            if (!game.AddPlayer(Context.ConnectionId))
            {
                return;
            }
        }
        else
        {
            var newGame = _factory.Build();
            newGame.Gameboard = Context.ConnectionId;
            _games.Add(connection.Room, newGame);
        }

        Context.Items[Name] = connection.Name;
        Context.Items[Room] = connection.Room;

        await Groups.AddToGroupAsync(Context.ConnectionId, connection.Room);
    }

    public async Task StartGame()
    {
        if (TryGetGame(out var game))
        {
            if (game is null) return;

            await game.StartGame();
            await Clients.Group(GetRoomName()).SendAsync("GameStarted");
        }
    }

    public async Task SendCard(CahCard card)
    {
        if (TryGetGame(out var game))
        {
            if (game is null) return;
            if (!await game.PlayCard(Context.ConnectionId, card)) return;

            await Clients.Client(Context.ConnectionId).SendAsync("CardPlayed", card);
        }
    }

    public async Task SendWinner(string winner)
    {
        if (TryGetGame(out var game))
        {
            if (game is null) return;
            if (!await game.SelectWinner(Context.ConnectionId, winner)) return;

            await Clients.Group(GetRoomName()).SendAsync("ReceiveWinner", winner);
        }
    }

    private bool TryGetGame(out CahGame? game)
    {
        game = null;
        return _games.TryGetValue(GetRoomName(), out game);
    }

    private string GetRoomName()
    {
        return Context.Items.TryGetValueAs(Room, out string roomName)
            ? roomName
            : throw new InvalidOperationException("Room name not found");
    }
}

public static class DictionaryExtensions
{
    public static bool TryGetValueAs<TKey, TValue, TValueAs>(this IDictionary<TKey, TValue> dictionary, TKey key,
        out TValueAs valueAs) where TValueAs : TValue
    {
        if (dictionary.TryGetValue(key, out var value) && value is TValueAs valueAsCast)
        {
            valueAs = valueAsCast;
            return true;
        }

        valueAs = default!;
        return false;
    }
}