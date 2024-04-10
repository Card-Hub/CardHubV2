using WebApi.Common;
using WebApi.Models;
using WebApi.GameLogic;
using System.Text.Json.Serialization;

namespace WebApi.GameLogic;

public class BlackJackJsonState {
    [JsonIgnore]
    public string GameType;
    [JsonIgnore]
    public List<string> PlayerNames;
    public List<Player> Players;
    public BlackJackJsonState() {
        GameType = "Black Jack";
        PlayerNames = new();
        Players = new();
    }
    public void Update(BlackJackGame game) {
        PlayerNames = game.GetPlayerList();
        Players.Clear();
        for (int i = 0; i < game.GetPlayerList().Count; i++) {
            Players.Add(game.Players[PlayerNames[i]]);
        }
    }
}