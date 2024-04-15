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
    public string state;// NotStarted, TakingBets, GiveCards, Checkingforwinners, DrawingCards, round end
    //right now we have given the cards to the players and we ened to check if there are any winners
    public StandardCardDeck Deck;
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

    public bool StartRound(){//this may need a check before bets to see if players have joined
        if (state == "NotStarted" && Players.Count() >= 2) {
            state = "TakingBets";
            return true;
        }
        Console.WriteLine("Not all players have made bets yet");
        return false;
    }

    public bool CheckWinnersOrLosers(){//will just apply attributes to players. we can check those later.
        if (state == "CheckingForWinners"){
            foreach(string playerName in PlayerOrder.GetActivePlayersInOrder()){
                if (Players[playerName].CurrentScore > 21)
                    Players[playerName].Busted = true;
                else if (Players[playerName].CurrentScore == 21)
                    Players[playerName].Winner = true;
                else
                    continue;
            }//if every single player is winner or looser restart round.
            foreach(string playerName in PlayerOrder.GetActivePlayersInOrder()){
                // if (Players[playerName].Busted == true)//if still playing is false on eery player then restart.
            }
            return true;
        }
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
            state = "CheckingForWinners";//need to check for quick winners ebfore i can go on.
            CheckWinnersOrLosers();//its possible that the game is just over right here. need to do states
            state = "Drawing Cards";// in draw cards i need to check if they are winner or busted alread.
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
            Players[playerName].HasBet = true;
            Console.WriteLine("Player:", playerName, "has bet:", amt);//send frontedn stuffs
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                if (player.Value.HasBet == false) {
                    state = "TakingBets";
                    Console.WriteLine("Not all bets are made");
                    return false;
                } else
                    state = "GivingCards";
            }
            if (state == "GivingCards")
                GivingCards();
            return true;
        }
        Console.WriteLine("we arnt in the TakingBets State");
        return false;
    }
    public void GivePlayerCard(string player, StandardCard card){//this is just for testing
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

    public bool DrawCard(string playerName)//this will check to see that players can draw cards too. also needs to check each players score.
    {
        if ((state == "GivingCards" || state == "DrawingCards") && Players[playerName].Busted == false && Players[playerName].Winner == false){
            Players[playerName].TakeCard(Deck.Draw());
            CheckWinnersOrLosers();//checks all players
            return true;
        }//calc the score now
        Console.WriteLine("Either wrong state or player cant draw");
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