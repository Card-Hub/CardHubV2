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
    public string state;// NotStarted, TakingBets, GiveCards, in turns, round end
    public StandardCardDeck Deck;
    // public bool BetsTaken;
    public BlackJackGame() {//dealer will jsut be a player i control.
        PlayerOrder = new();
        Players = new();
        BlackJackJsonState = new();
        Deck = new();
        state = "NotStarted";
    }


    public void StartGame()
    {
        AddPlayer("Dealer");//keep deal in first pos and just pretend like they are last
        StartRound();
    }

    public bool StartRound(){//starting a round of blackjack is everyone getss two cards
        if (state == "NotStarted") {
            state = "TakingBets";
            return true;
        }
        Console.WriteLine("Not all players have made bets yet");
        return false;
    }

    public bool GivingCards(){
        if (state == "GivingCards"){
            foreach(string playerName in GetPlayersInOrder().Skip(1))//skip 1 is for the dealer
                DrawCard(playerName);
            DrawCard("Dealer");//dealer gets first card
            foreach(string playerName in GetPlayersInOrder().Skip(1))//second card for dealer will not be shown.
                DrawCard(playerName);
            DrawCard("Dealer");
            return true;
        }
        Console.WriteLine("The state is not in \"GivingCards\"");
        return false;
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

    public List<string> GetPlayersInOrder() {//curent
        return PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
    }
    public bool ResetForNextRound()// must be at thee end of the rounda
    {
        throw new NotImplementedException();
    }
    public bool TakeBet(string playerName, int amt){
        if (state == "TakingBets") { 
            Players[playerName].CurrentBet = amt;
            Players[playerName].hasBet = true;
            Console.WriteLine("Player:", playerName, "has bet:", amt);//send frontedn stuffs
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                if (player.Value.hasBet == false)
                    break;
                else
                    state = "GivingCards";
            }
            return true;
        }
        return false;
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
        if (state == "NotStarted") {
            PlayerOrder.AddPlayer(playerName);
            Players[playerName] = new BlackJackPlayer(playerName);
            Players[playerName].Name = playerName;
            return true;
        }
        return false;
    }
    public bool RemovePlayer(string playerName)
    {
        if (state == "NotStarted") {
            PlayerOrder.RemovePlayer(playerName);
            Players.Remove(playerName);
            return true;
        }
        return false;
    }  
    public List<StandardCard> GetPlayerHand(string playerName)
    {
        return Players[playerName].ShowHand();
    }  

    public bool DrawCard(string playerName)//this will be done before drawing and during drawing
    {
        if (state == "GivingCards"){
            Players[playerName].TakeCard(Deck.Draw());
            return true;
        }
        return false;
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