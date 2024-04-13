using System.Runtime.CompilerServices;
using System.Xml;
using WebApi.Common.LyssiePlayerOrder;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Models;
using Newtonsoft.Json;
using WebApi.Common;
namespace WebApi.GameLogic;


public class BlackJackGame : IBaseGame<StandardCard>
{// make black jack player
// ask lyssie for her poker player
    public LyssiePlayerOrder PlayerOrder;
    public Dictionary<string, BlackJackPlayer> Players;
    public BlackJackJsonState BlackJackJsonState;
    public StandardCardDeck Deck;
    public BlackJackGame() {
        PlayerOrder = new();
        Players = new();
        BlackJackJsonState = new();
        Deck = new();
    }
    public void StartGame()//need to ask for bets before we can init
    {
        throw new NotImplementedException();
    }
    // IBaseGame<StandardCard>.EndGame()
    public bool InitDeck()
    {
        Deck.Init52();
        Deck.Shuffle();
        return true;
    }

    public void EndGame() {
        throw new NotImplementedException();
    }

    public List<string> GetPlayersInOrder() {
        throw new NotImplementedException();
    }
    public bool ResetForNextRound()
    {
        throw new NotImplementedException();
    }
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
        return Players[playerName].ShowHand();
    }  

    public bool DrawCard(string playerName)
    {
        Players[playerName].TakeCard(Deck.Draw());
        return true;
    }

    public int GetPlayerScore(string playerName){//need to allow for splits, double downs. only one or the other.
        int score = 0;
        List<StandardCard> hand = Players[playerName].ShowHand();
        foreach(StandardCard card in hand){
            score += 0;
        }
        return score;
    }
    public string GetGameState() {
        BlackJackJsonState.Update(this);
        return JsonConvert.SerializeObject(BlackJackJsonState, Newtonsoft.Json.Formatting.Indented);
    }
}