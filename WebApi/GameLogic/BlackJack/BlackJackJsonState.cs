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
    public string CurrentPlayer;
    public string Winners;
    public string Losers;
    public string State;


    // add lot of button code here
    public BlackJackJsonState() {
        GameType = "Black Jack";
        PlayerNames = new();
        Players = new();
        CurrentPlayer = "";
        Winners = "";
        Losers = "";
        State = "";
    }
    public void Update(BlackJackGame game) {
        PlayerNames = game.GetPlayerList();
        CurrentPlayer = game.PlayerOrder.GetCurrentPlayer();
        Players.Clear();
        for (int i = 0; i < game.GetPlayerList().Count; i++) {
            Players.Add(game.Players[PlayerNames[i]]);
        }
        State = game.state;
    }
}