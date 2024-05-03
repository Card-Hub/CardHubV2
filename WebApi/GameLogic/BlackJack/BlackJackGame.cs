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
//after the game board is waiting in the room, players willbe joined as the enter the lobby by invoking the add player command while in the not started state.
//The gamebaord will press start game button. This in turn should result in a page change for everyone, the gamebaord will show everyones cards and the dealers hand once everyone has placed a bet. (Their cards should be delivered on start and will be individual per device)
//Each player will have all the buttons on their page and wont be allowed to press when its not their turn. Players turn will be known but PlayerOrder
//When it is their trun, they may press the hit button which will invoke the draw card function.
// If they bust or win it will make it next players turn, oitherwise the may hit stand to change turns.
// once last player has hit stand or bust/win the dealers turn will be called and they will draw carsd until 17 or bust/win.
//at that point winners are shown and bet money is given.


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
    }

    public void StartGame()//waiting for palyers to join here before we continue, gameB will decide when to start the round
    {
        BlackJackJsonState.GameStarted = true;
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
            Players["Dealer"].HasBet= true;
            PlayerOrder.NextTurn();
            //give promt to frontend to take bets
            state = "TakingBets";
            Console.WriteLine("\n\nim here now changing state to taking bets\n\n");
            // await _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
            return true;
        }
        Console.WriteLine("Not all players have made bets yet");
        return false;
    }

    public bool Restart(){//may want to allow for an override here tor restart early.
        if (state == "Restart") {
            foreach(KeyValuePair<string, BlackJackPlayer> player in Players) {
                Players[player.Key].Hand.Clear();
                Players[player.Key].CurrentScore = 0;
                Players[player.Key].CurrentBet = 0;
                Players[player.Key].HasBet = false;
                Players[player.Key].Busted = false;
                Players[player.Key].Winner = false;
                Players[player.Key].StillPlaying = true;
                Players[player.Key].Standing = false;
            }
            BlackJackJsonState.Winners.Clear();
            BlackJackJsonState.Losers.Clear();
            BlackJackJsonState.Stalemates.Clear();
            BlackJackJsonState.AllPlayersHaveBet = false;
            BlackJackJsonState.DealersTurn = false;
            PlayerOrder.BackToFirstPlayer();
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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
                    BlackJackJsonState.Update(this);
                    _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
                    if (PlayerOrder.GetCurrentPlayer() == player.Key && player.Key != "Dealer")
                        PlayerOrder.NextTurn();
                }
                else if (playerScore == 21) {
                    Players[player.Key].Winner = true;
                    Players[player.Key].StillPlaying = false;
                    Players[player.Key].Standing = true;
                    BlackJackJsonState.Update(this);
                    _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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
                    if (Players[player.Key].NotPlaying == true || Players[player.Key].Busted == true || Players[player.Key].Winner == true || Players[player.Key].StillPlaying == false || player.Value.Standing == true && state != "CheckingForQuickwinners" || player.Key == "Dealer"){
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
            BlackJackJsonState.AllPlayersHaveBet = true;
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
                if (player.Key != "Dealer" && player.Value.NotPlaying == false)
                    DrawCard(player.Key);
            DrawCard("Dealer");
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players)
                if (player.Key != "Dealer" && player.Value.NotPlaying == false)
                    DrawCard(player.Key);
            DrawCard("Dealer");
            Console.WriteLine("In giving cards");
            state = "CheckingForQuickWinners";
            CheckForWinnersOrLosers();//may want a wait or something here for like 2 seconds.
            // await _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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

            Console.WriteLine(connStr,"\n\nInBlackjackGame taking bet\n\n");
            if (Players[connStr].HasBet == false) {
            // if (Players[connStr].TotalMoney >= amt){
                Players[connStr].TotalMoney -= amt;
                Players[connStr].CurrentBet = amt;
                Players[connStr].HasBet = true;
                BlackJackJsonState.Update(this);
                _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
                if (Players[connStr].TotalMoney < 1) {
                    Players[connStr].HasBet = true;
                    Players[connStr].NotPlaying = true;
                    PlayerOrder.SetPlayerStatus(connStr, LyssiePlayerStatus.Spectator);
                }

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
            // } else
            //     Console.WriteLine("Not Enough money in players total!\n");
            } else 
                Console.WriteLine("This player may not bet");
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
            Players[connStr].strConn = connStr;
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
            if (player.Value.Standing == true && player.Value.StillPlaying == false || player.Key == "Dealer" || player.Value.NotPlaying == true) {
                state = "DealersTurn";
            } else{
                state = "DrawingCards";
                break;
            }
        }
        if (state == "DealersTurn"){
            PlayerOrder.BackToFirstPlayer();
        }
        return true;
    }

    public bool MakeListsAndPayPlayers(){
        if (state == "MakeListsAndPayPlayers"){
            BlackJackJsonState.DealersTurn = true;
            foreach (KeyValuePair<string, BlackJackPlayer> player in Players) {
                if (player.Key != "Dealer" && player.Value.NotPlaying == false) {
                    if (player.Value.CurrentScore > Players["Dealer"].CurrentScore && player.Value.Busted == false || Players["Dealer"].Busted && player.Value.Busted == false) {
                        BlackJackJsonState.Winners.Add(player.Value.Name);
                        // player.Value.TotalMoney += player.Value.CurrentBet * 2;
                        Players[player.Key].TotalMoney += player.Value.CurrentBet * 2;
                    }
                    else if (player.Value.CurrentScore < Players["Dealer"].CurrentScore || player.Value.Busted == true)
                        BlackJackJsonState.Losers.Add(player.Value.Name);
                    else {
                        BlackJackJsonState.Stalemates.Add(player.Value.Name);
                        // player.Value.TotalMoney += player.Value.CurrentBet;
                        Players[player.Key].TotalMoney += player.Value.CurrentBet;
                    }
                } else
                    continue;
            }
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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
            BlackJackJsonState.Update(this);
            _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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
            Console.WriteLine("\n\nDrawing cards for players\n\n");
            Players[connStr].TakeCard(Deck.Draw());
            PlayerOrder.NextTurn();
            return true;
        } else if (state == "DrawingCards" || state == "DealersTurn"){//must be correct players turn.
            string currentPlayer = PlayerOrder.GetCurrentPlayer();
            if (currentPlayer == connStr && Players[connStr].Busted == false && Players[connStr].Winner == false && Players[connStr].StillPlaying == true && Players[connStr].NotPlaying == false) {//eventually it will be dealers turn. and that will be a block.
                Players[connStr].TakeCard(Deck.Draw());
                CheckForWinnersOrLosers();//checks all players which is weird when i just need to check single bust. may fix later
                BlackJackJsonState.Update(this);
                _messenger.SendFrontendJson(GetAllConnStrsIncGameboard(), GetGameState());
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