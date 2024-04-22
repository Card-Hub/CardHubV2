using System.Runtime.CompilerServices;
using System.Xml;
using WebApi.Common.LyssiePlayerOrder;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Models;
// using WebApi.Hubs.HubMessengers;
// using Tests.RandomTests.UnoTestMessenger;
using Newtonsoft.Json;
using WebApi.Common;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Net;
using WebApi.Hubs.HubMessengers;
namespace WebApi.GameLogic;


public class BlackJackGame : IBaseGame<StandardCard>
{
    public LyssiePlayerOrder PlayerOrder;
    public Dictionary<string, BlackJackPlayer> Players;
    public BlackJackJsonState BlackJackJsonState;
    public string state;// NotStarted, TakingBets, GiveCards, Checkingforwinners, DrawingCards, round end
    //right now we have given the cards to the players and we ened to check if there are any winners
    //right now i have made it so that the cardsa re given out and players can then ask for cards with drwa cards state in the correct order and checks for busts.
    //stay button and a finish game button or something, idk im tired
    public StandardCardDeck Deck;
    public iUnoMessenger _messenger;
    public BlackJackGame(iUnoMessenger messenger) {//dealer will jsut be a player i control.
        _messenger = messenger;
        PlayerOrder = new();
        Players = new();
        BlackJackJsonState = new();
        Deck = new();
        state = "NotStarted";
    }

    public void StartGame()
    {
        AddPlayer("Dealer", "Dealer");//keep dealer in first pos and just pretend like they are last
        StartRound();
    }

    public bool StartRound(){//this may need a check before bets to see if players have joined
        if (state == "NotStarted" && Players.Count >= 2) {
            InitDeck();
            //give promt to frontend to take bets
            state = "TakingBets";
            return true;
        }
        Console.WriteLine("Not all players have made bets yet");
        return false;
    }

    public bool Restart(){//Before this is called i need to send frontend json so it knows winners/losers
        //!!!!!!!!!!!!!!! send front end stuff here maybe!!!!!!!!!!!!!!!!!!!!!!!!!!they will need info here that tells frontend to allow for restart button.
        if (state == "Restart") {
            state = "NotStarted";
            foreach(KeyValuePair<string, BlackJackPlayer> player in Players) {
                Players[player.Key].Hand.Clear();
                Players[player.Key].Busted = false;
                Players[player.Key].Winner = false;
                Players[player.Key].StillPlaying = true;
            }
            PlayerOrder.BackToFirstPlayer();
            StartRound();
            return true;
        }
        return false;
    }

    public bool CheckWinnersOrLosers(){//will just apply attributes to players. we can check those later.
        if (state == "CheckingForWinners"){
            int playerScore = 0;
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                playerScore = GetPlayerScoreFromGame(player.Key);
                if (playerScore > 21) {
                    Players[player.Key].Busted = true;
                    Players[player.Key].StillPlaying = false;
                }
                else if (playerScore == 21) {
                    Players[player.Key].Winner = true;
                    Players[player.Key].StillPlaying = false;
                } else
                    continue;
            }
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                if (Players[player.Key].Busted == true || Players[player.Key].Winner == true || Players[player.Key].StillPlaying == false){
                    state = "Restart";//will send json here to tell admin to restart
                }
                else {
                    state = "DrawingCards";//will send json here.drawing cards section needs a check /s as well. it needs to manage turns from players having free will and shouldnt be allowed to.
                    break;
                }
            }
            return true;
        }
        return false;
    }
    public bool GivingCards(){//either called auto or will make button for this.
        //will need to be sending json for each card draw. so maybe just do it in draw i guess.
        if (state == "GivingCards"){
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
                if (player.Key != "Dealer")
                    DrawCard(player.Key);
            DrawCard("Dealer");
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
                if (player.Key != "Dealer")
                    DrawCard(player.Key);
            DrawCard("Dealer");
            state = "CheckingForWinners";
            CheckWinnersOrLosers();//may want a wait or something here for like 2 seconds.
            return true;//now the round either end or they draw cards. end round done, drwing cards not done.
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
    public bool ResetForNextRound(){
        Restart();
        return true;
    }
    public bool TakeBet(string connStr, int amt){
        if (state == "TakingBets") { 
            Players[connStr].CurrentBet = amt;
            Players[connStr].HasBet = true;
            Console.WriteLine("Player:", connStr, "has bet:", amt);//send frontedn stuffs
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                if (player.Value.HasBet == false) {
                    state = "TakingBets";
                    Console.WriteLine("Not all bets are made");
                    return true;
                } else
                    state = "GivingCards";//all bets have been made
            }
            if (state == "GivingCards")//may want to give promt for button here so its not instant.
                GivingCards();
            return true;
        }
        Console.WriteLine("we arnt in the TakingBets State");
        return false;
    }
    public void GivePlayerCard(string connStr, StandardCard card){//this is just for testing
        Players[connStr].TakeCard(card);
    }

    public List<string> GetPlayerList()
    {
        return PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
    }

    public bool AddPlayer(string playerName, string connStr)
    {
        if (state == "NotStarted") {
            PlayerOrder.AddPlayer(connStr);
            Players[connStr] = new BlackJackPlayer(playerName);
            return true;
        }
        return false;
    }

    public bool RemovePlayer(string connStr)
    {
        if (state == "NotStarted") {
            PlayerOrder.RemovePlayer(connStr);
            Players.Remove(connStr);
            return true;
        }
        return false;
    }  
    public List<StandardCard> GetPlayerHand(string connStr)
    {
        return Players[connStr].ShowHand();
    }  

    public bool CheckAllPlayersStanding(){
        foreach (KeyValuePair<string, BlackJackPlayer> player in Players){
            if (player.Value.StillPlaying == false)
                state = "DealersTurn";
            else
                break;
        }
        return true;
    }

    public bool DealersTurn(){//dealer draws cards
        if (state == "DealersTurn") {
            int score = GetPlayerScoreFromGame("Dealer");
            while (score < 17) {
                DrawCard("Dealer");
                score = GetPlayerScoreFromGame("Dealer");
            }
            Stand("Dealer");
            CheckWinnersOrLosers();//!!!!!!! send json to front to show button for restart
            return true;
        }
        return false;
    }
    public bool Stand(string connStr) {
        if (state == "drawingcards" && connStr == PlayerOrder.GetCurrentPlayer()) {
            Players[connStr].NotPlaying = true;
            PlayerOrder.NextTurn();
            CheckAllPlayersStanding();
            if (state == "DealersTurn")
                DealersTurn();
            return true;
        }
        else {
            Console.WriteLine("Its not that platers turn\n");
            return false;
        }
    }

    public bool DrawCard(string connStr)//this will check to see that players can draw cards too. also needs to check each players score. 
    //
    {//need to iterate over player turn here too
        if ((state == "GivingCards") && Players[connStr].Busted == false && Players[connStr].Winner == false){
            Players[connStr].TakeCard(Deck.Draw());
            CheckWinnersOrLosers();//checks all players which is weird when i just need to check single bust. may fix later
            return true;
        } else if (state == "DrawingCards"){//must be correct players turn.
            string currentPlayer = PlayerOrder.GetCurrentPlayer();
            if (currentPlayer == connStr) {//eventually it will be dealers turn. and that will be a block.
                Players[connStr].TakeCard(Deck.Draw());
                CheckWinnersOrLosers();//checks all players which is weird when i just need to check single bust. may fix later
                return true;
            }
            else//need to send json struct here
                {Console.WriteLine("Current players turn is", currentPlayer);}
            return true;
        } else
            Console.WriteLine("Either wrong state or player cant draw");
            return false;
    }



    public int GetPlayerScoreFromGame(string connStr){//this wont work for splits. player will need more than one hand
        int score = 0, aces = 0;
        List<StandardCard> hand = Players[connStr].ShowHand();
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
        Players[connStr].CurrentScore = score;
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