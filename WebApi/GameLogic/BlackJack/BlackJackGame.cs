using System.Runtime.CompilerServices;
using System.Xml;
using WebApi.Common.LyssiePlayerOrder;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Models;
using Newtonsoft.Json;
namespace WebApi.GameLogic;


public class BlackJackGame : IBaseGame<StandardCard>
{// make black jack player
// ask lyssie for her poker player
    public LyssiePlayerOrder PlayerOrder;
    public Dictionary<string, BlackJackPlayer> Players;
    public BlackJackJsonState BlackJackJsonState;
    public BlackJackGame() {
        PlayerOrder = new();
        Players = new();
        BlackJackJsonState = new();
    }
    public void StartGame()
    {
        throw new NotImplementedException();
    }
    public bool InitDeck()
    {
        throw new NotImplementedException();
    }
    // public List<StandardCard> GetPlayerHand(string playerName)
    // {
    //     throw new NotImplementedException();
    // }

    public bool TakeBet(string player, int amt){
        Players[player].CurrentBet = amt;
        return true;
    }
    public List<string> GetPlayerList()
    {
        return PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
    }
    public bool AddPlayer(string playerName)
    {
        PlayerOrder.AddPlayer(playerName);
        Players[playerName] = new BlackJackPlayer(playerName);
        Players[playerName].Name = playerName;
        return true;
    }  
    public bool RemovePlayer(string playerName)
    {
        PlayerOrder.RemovePlayer(playerName);
        Players.Remove(playerName);
        return true;
    }  
    public List<StandardCard> GetPlayerHand(string playerName)
    {
        throw new NotImplementedException();
    }  
    public List<string> GetPlayersInOrder()
    {
        throw new NotImplementedException();
    }
    public bool DrawCard(string playerName)
    {
        throw new NotImplementedException();
    }

    public bool ResetForNextRound()
    {
        throw new NotImplementedException();
    }

    public void EndGame()
    {
        throw new NotImplementedException();
    }
    public string GetGameState() {
        BlackJackJsonState.Update(this);
        return JsonConvert.SerializeObject(BlackJackJsonState, Newtonsoft.Json.Formatting.Indented);
    }
}