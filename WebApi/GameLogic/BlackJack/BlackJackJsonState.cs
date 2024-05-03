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
    public List<string> Winners;
    public List<string> Losers;
    public List<string> Stalemates;
    public string State;
    public bool AllPlayersHaveBet;
    public bool GameStarted;


    // add lot of button code here
    public BlackJackJsonState() {
        GameType = "Black Jack";
        PlayerNames = new();
        Players = new();
        CurrentPlayer = "";
        Winners = new();
        Losers = new();
        Stalemates = new();
        State = "";
        AllPlayersHaveBet = false;
        GameStarted = false;
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