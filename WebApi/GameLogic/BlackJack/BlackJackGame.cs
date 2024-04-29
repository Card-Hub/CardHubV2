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



//so first the gamebaord player will select blackjack and they will be put into a room. At that point, the game instance has been made.
//after the game board is waiting in the room, players will begin to join that room code and in turn invoke the add player command while in the not started state.
//after all player are there, the gamebaord will press start game button. This in turn should result in a page change for everyone to the blackjack table with their cards delivered.
//

public class BlackJackGame : IBaseGame<StandardCard>
{
    public LyssiePlayerOrder PlayerOrder;
    public Dictionary<string, BlackJackPlayer> Players;
    public BlackJackJsonState BlackJackJsonState;
    public string state;// NotStarted, TakingBets, GiveCards, Checkingforwinners, DrawingCards, round end

    public StandardCardDeck Deck;
    public iUnoMessenger _messenger;
    public string GameboardConnStr;
    public BlackJackGame(iUnoMessenger messenger, string gameboardConnStr) {
        _messenger = messenger;
        PlayerOrder = new();
        Players = new();
        BlackJackJsonState = new();
        Deck = new();
        state = "NotStarted";
        GameboardConnStr = gameboardConnStr;
        AddPlayer("Dealer", "Dealer");
        Players["Dealer"].HasBet= true;
    }

    public void StartGame()//waiting for palyers to join here before we continue, gameB will decide when to start the round
    {
        // AddPlayer("Dealer", "Dealer");
        // Players["Dealer"].HasBet= true;
        state = "Started";
        StartRound();
    }


    // public bool GivePlayersMoney() {
    //     foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
    //         if (player.Key != "Dealer")
    //             player.Value.TotalMoney = 100;
    //     return true;
    // }

    public bool StartRound(){
        if (state == "Restart" || state == "Started" && Players.Count >= 2) {
            InitDeck();
            PlayerOrder.NextTurn();/////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!this will break code depending on where he is added
            //give promt to frontend to take bets
            state = "TakingBets";
            return true;
        }
        Console.WriteLine("Not all players have made bets yet");
        return false;
    }

    public bool Restart(){
        if (state == "Restart") {
            // state = "NotStarted";
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

    public bool CheckForWinnersOrLosers(){//will just apply attributes to players. we can check those later.
        if (state == "CheckingForQuickWinners" || state == "DrawingCards" || state == "DealersTurn"){
            int playerScore = 0;
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                playerScore = GetPlayerScoreFromGame(player.Key);
                if (playerScore > 21) {
                    Players[player.Key].Busted = true;
                    Players[player.Key].StillPlaying = false;
                    Players[player.Key].Standing = true;
                    if (PlayerOrder.GetCurrentPlayer() == player.Key && player.Key != "Dealer")
                        PlayerOrder.NextTurn();
                }
                else if (playerScore == 21) {
                    Players[player.Key].Winner = true;
                    Players[player.Key].StillPlaying = false;
                    Players[player.Key].Standing = true;
                    if (PlayerOrder.GetCurrentPlayer() == player.Key && player.Key != "Dealer")
                        PlayerOrder.NextTurn();
                    
                } else
                    continue;
            } 
            if (state == "DealersTurn" && Players["Dealer"].CurrentScore >= 17) {
                state = "MakeListsAndPayPlayers";
            }
            else if (state != "DealersTurn") {
                foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {//fix this later, its ugly
                    if (Players[player.Key].Busted == true || Players[player.Key].Winner == true || Players[player.Key].StillPlaying == false || player.Value.Standing == true && state != "CheckingForQuickwinners" || player.Key == "Dealer"){
                        Console.WriteLine("\n\nim here in the place you think it will be\n\n");
                        // state = "MakeListsAndPayPlayers";
                        state = "DealersTurn";
                        // MakeListsAndPayPlayers();
                        // break;
                    }
                    else {//might be bad state here else if
                        Console.WriteLine("\n\nIM IN THE ELSE\n\n");
                        state = "DrawingCards";//will send json here.drawing cards section needs a check /s as well. it needs to manage turns from players having free will and shouldnt be allowed to.
                        break;
                    }
                }
                if (state == "DealersTurn")
                    DealersTurn();
            }
            if (state == "MakeListsAndPayPlayers")
                MakeListsAndPayPlayers();
            return true;
        }
        else {
            Console.WriteLine("Incorrect state to check winners\n");
            return false;
        }
    }
    public bool GivingCards(){
        if (state == "GivingCards"){
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
                if (player.Key != "Dealer")
                    DrawCard(player.Key);
            DrawCard("Dealer");
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
                if (player.Key != "Dealer")
                    DrawCard(player.Key);
            DrawCard("Dealer");
            Console.WriteLine("In giving cards");
            state = "CheckingForQuickWinners";
            CheckForWinnersOrLosers();//may want a wait or something here for like 2 seconds.
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
            if (Players[connStr].TotalMoney >= amt){
                Players[connStr].TotalMoney -= amt;
                Players[connStr].CurrentBet = amt;
                Players[connStr].HasBet = true;
                // BlackJackJsonState.Update(this);
                // Console.WriteLine("Player:", connStr, "has bet:", amt);//send frontedn stuffs
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
            } else
                Console.WriteLine("Not Enough money in players total!\n");
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
            Players[connStr].TotalMoney = 100;
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
            if (player.Value.Standing == true && player.Value.StillPlaying == false || player.Key == "Dealer") {
                state = "DealersTurn";
            } else{
                state = "DrawingCards";
                break;
            }
        }
        return true;
    }

    public bool MakeListsAndPayPlayers(){
        if (state == "MakeListsAndPayPlayers"){
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                if (player.Key != "Dealer") {
                    if (player.Value.CurrentScore > Players["Dealer"].CurrentScore && player.Value.Busted == false || Players["Dealer"].Busted && player.Value.Busted == false) {
                        BlackJackJsonState.Winners.Add(player.Value.Name);
                        player.Value.TotalMoney += player.Value.CurrentBet * 2;
                    }
                    else if (player.Value.CurrentScore < Players["Dealer"].CurrentScore || player.Value.Busted == true)
                        BlackJackJsonState.Losers.Add(player.Value.Name);
                    else {
                        BlackJackJsonState.Stalemates.Add(player.Value.Name);
                        player.Value.TotalMoney += player.Value.CurrentBet;
                    }
                } else
                    continue;
            }
            state = "Restart";//send the strings here to frontend to parse.
            return true;
        }else {
            Console.WriteLine("Not MakeListsAndPayPlayers states, in MakeListsAndPayPlayers");
            return false;
        }
    }


    private List<string> GetAllConnStrsIncGameboard() {
      List<string> allConnStrs = new List<string>(PlayerOrder.GetAllPlayers()){GameboardConnStr}; 
      return allConnStrs;
    }

    public bool DealersTurn(){//dealer draws cards
        if (state == "DealersTurn") {

            Console.WriteLine("im if statement dealersturn");
            int score = GetPlayerScoreFromGame("Dealer");
            int x = 0;
            while (score < 17 && x < 12) {
                Console.WriteLine("INLOOP\n");
                DrawCard("Dealer");
                score = GetPlayerScoreFromGame("Dealer");
                x++;
            }
            Players["Dealer"].Standing = true;
            Players["Dealer"].StillPlaying = false;
            // if (state == "DealersTurn")
            CheckForWinnersOrLosers();
            // state = "MakeListsAndPayPlayers";
            return true;
        }
        else {
            Console.WriteLine("not in dealersturn state\n");
            return false;
        }
    }

    public bool Stand(string connStr) {
        if (state == "DrawingCards" && connStr == PlayerOrder.GetCurrentPlayer()) {
            Players[connStr].StillPlaying = false;
            Players[connStr].Standing = true;
            PlayerOrder.NextTurn();
            CheckAllPlayersStanding();
            // Console.WriteLine("\n\nin stand function\n\n");
            if (state == "DealersTurn")
                DealersTurn();
            return true;
        }
        else {
            Console.WriteLine("Its not that players turn to stand\n");
            return false;
        }
    }

    public bool DrawCard(string connStr)//this will check to see that players can draw cards too. also needs to check each players score. 
    //
    {//need to iterate over player turn here too
        if (state == "GivingCards"){
            Players[connStr].TakeCard(Deck.Draw());
            PlayerOrder.NextTurn();
            return true;
        } else if (state == "DrawingCards" || state == "DealersTurn"){//must be correct players turn.
            string currentPlayer = PlayerOrder.GetCurrentPlayer();
            if (currentPlayer == connStr && Players[connStr].Busted == false && Players[connStr].Winner == false && Players[connStr].StillPlaying == true) {//eventually it will be dealers turn. and that will be a block.
                Players[connStr].TakeCard(Deck.Draw());
                CheckForWinnersOrLosers();//checks all players which is weird when i just need to check single bust. may fix later
                return true;
            } else//need to send json struct here
                Console.WriteLine("Its not your turn! Current players turn is", currentPlayer);
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