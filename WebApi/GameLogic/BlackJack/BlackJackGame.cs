using System.Runtime.CompilerServices;
using System.Xml;
using WebApi.Common.LyssiePlayerOrder;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Models;
using Newtonsoft.Json;
using WebApi.Common;
namespace WebApi.GameLogic;


public class BlackJackGame : IBaseGame<StandardCard>
{
    public LyssiePlayerOrder PlayerOrder;
    public Dictionary<string, BlackJackPlayer> Players;
    public BlackJackJsonState BlackJackJsonState;
    public StandardCardDeck Deck;
    public bool BetsTaken;
    public BlackJackGame() {
        PlayerOrder = new();
        Players = new();
        BlackJackJsonState = new();
        Deck = new();
        BetsTaken = true;
    }


    public void StartGame()
    {
        StartRound();
    }

    public void StartRound(){
        foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
            if (player.Value.hasBet == false) {
                BetsTaken = false;
            }
        }
        if (BetsTaken == true){
            
            throw new NotImplementedException();
        }
    }
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
        Players[player].hasBet = true;
        Console.WriteLine("Player:", player, "has bet:", amt);
        return true;
    }
    public void GivePlayerCard(string player, StandardCard card){
        Players[player].TakeCard(card);
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

    public int GetPlayerScoreFromGame(string playerName){//this wont work for splits. player will need more than one hand
        int score = 0, aces = 0;
        List<StandardCard> hand = Players[playerName].ShowHand();
        foreach(StandardCard card in hand){
            if (card.Value == "A"){
                aces += 1;
            }
            else if (card.Value == "10" || card.Value == "J" || card.Value == "Q" || card.Value == "K"){
                score += 10;
            }
            else {
                score += card.Value[0] - 48;
            }
        }
        if (aces > 0) {
            for (int i = 0; i < aces; i++){
                score += 1;
            }
            if (score + 10 <= 21) {
                score += 10;
            }
        }
        Players[playerName].CurrentScore = score;
        return score;
    }

    public void SetState(BlackJackJsonState newState) {
      BlackJackJsonState = newState;
    }
    public string GetGameState() {
        BlackJackJsonState.Update(this);
        return JsonConvert.SerializeObject(BlackJackJsonState, Newtonsoft.Json.Formatting.Indented);
    }
}