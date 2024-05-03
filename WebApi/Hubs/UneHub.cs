using Microsoft.AspNetCore.SignalR;
using WebApi.Common;
using WebApi.GameLogic;
using WebApi.GameLogic.LyssieUno;
using WebApi.Models;
using WebApi.Services;
using Newtonsoft.Json;

namespace WebApi.Hubs;

public class UneHub : Hub
{
    private const string Name = "Name";
    private const string Room = "Room";
    private const string PlayerType = "PlayerType";
    private UnoMessenger _messenger;
    
    private string ContextId => Context.ConnectionId;

    private IDictionary<string, UnoGameModLyssie?> _games;
    //private readonly CahFactory _factory;

    public UneHub(IDictionary<string, UnoGameModLyssie?> games, UnoMessenger messenger)
    {
        _games = games;
        _messenger = messenger;
        //_factory = factory;
    }

    public async Task JoinRoom(ConnectionOptions connectionOptions)
    {
      Console.WriteLine("JoinRoom UneHub");
        var roomId = connectionOptions.Room;
        if (!TryGetGame(out var game, roomId))
        {
            var newDeckBuilder = new UnoDeckBuilderLyssie();
            var newGame = new UnoGameModLyssie(_messenger, newDeckBuilder);
            newGame.GameboardConnStr = ContextId;
            //newGame.Room = roomId;
            //newGame.PickingFinished += TimerTest;
            _games.Add(roomId, newGame);
            Console.WriteLine("Joined as gameboard.");
        }
        else
        {
            if (connectionOptions.Name is null) return;
            if (!game.AddPlayer(connectionOptions.Name, ContextId)) return;
            Console.WriteLine("Joined as player.");
        }
        
        Context.Items.Add(Room, roomId);

        await Groups.AddToGroupAsync(ContextId, roomId);
    }

    //private void TimerTest(object? sender, EventArgs e)
    //{
    //    Console.WriteLine("TTT Timer test");
    //    TryGetGame(out var game);
    //    Console.Write(game.Room);
    //}

    public async Task StartGame()
    {
        if (!TryGetGame(out var game)) return;
        await game.StartGame();
        //var playerHands = game.StartGame();
        
        //await Clients.Group(GetRoomId()).SendAsync("GameStarted");
        //foreach (var (player, cards) in playerHands)
        //{
        //    await Clients.Client(player).SendAsync("ReceiveCards", cards);
        //}
    }
    public async Task DrawCard()
    {
        if (!TryGetGame(out var game)) return;
        await game.DrawCard(ContextId);
    }
    public async Task SelectWildColor(string color)
    {
        UnoColorLyssie colorEnum = UnoColorLyssie.Black;
        if (!TryGetGame(out var game)) return;
        switch (color.ToLower()) {
          case "red":
            colorEnum = UnoColorLyssie.Red;
            break;
          case "blue":
            colorEnum = UnoColorLyssie.Blue;
            break;
          case "green":
            colorEnum = UnoColorLyssie.Green;
            break;
          case "yellow":
            colorEnum = UnoColorLyssie.Yellow;
            break;
          default:
            break;
        }
        await game.SelectWild(ContextId, colorEnum);
    }

    public async Task PressUne() {
      if (!TryGetGame(out var game)) return;
      await game.PressUne(ContextId);
    }
    public async Task PlayCard(string cardJson) {
      // correct for DrawTwo lmao
      string newCardJson = cardJson;
      newCardJson = newCardJson.Replace("Draw Two", "DrawTwo");
      newCardJson = newCardJson.Replace("Wild Draw Four", "WildDrawFour");
      newCardJson = newCardJson.Replace("Skip All", "SkipAll");
      UnoCardModLyssie card = JsonConvert.DeserializeObject<UnoCardModLyssie>(newCardJson);
      if (!TryGetGame(out var game)) return;
      await game.PlayCard(ContextId, card);
    }
    
    // await _hubContext.Clients.Group(Room).SendAsync("ReceiveBlackCard", _currentBlackCard);
    //
    // var nonCzarPlayers = GetNonCzarPlayers();
    // var cardCzar = _playerOrder.Current();
    //     if (_pickAmount == 3)
    // {
    //     foreach (var player in nonCzarPlayers)
    //     {
    //         var cards = _whiteDeck.Draw(2);
    //         await _hubContext.Clients.Client(player).SendAsync("ReceiveWhiteCards", cards);
    //     }
    // }
    //     
    // await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("StartTimer", _pickingTimeLimit.Seconds);
    // await _hubContext.Clients.Clients(nonCzarPlayers).SendAsync("SetPickAmount", _pickAmount);
    //
    // await _hubContext.Clients.Client(cardCzar).SendAsync("CardCzar");

    //public async Task SendCards(List<CahCard> cards)
    //{
    //    if (!TryGetGame(out var game)) return;
    //    if (!game.PlayCards(ContextId, cards)) return;

    //    await Clients.Client(ContextId).SendAsync("CardsPlayed", cards);
    //}

    //public async Task Gamble(CahCard pickedCard, CahCard gambledCard)
    //{
    //    if (!TryGetGame(out var game)) return;
    //    if (!game.Gamble(ContextId, pickedCard, gambledCard)) return;

    //    await Clients.Client(ContextId).SendAsync("Gambled", pickedCard, gambledCard);
    //}

    //public async Task SendWinner(string winner)
    //{
    //    if (!TryGetGame(out var game)) return;
    //    if (!game.SelectWinner(ContextId, winner)) return;

    //    await Clients.Group(GetRoomId()).SendAsync("ReceiveWinner", winner);
    //}

    public async Task Ping() => await Clients.Caller.SendAsync("Pong");
    

  
    #region Helpers
    
    private bool TryGetGame(out UnoGameModLyssie game, string? roomId = null)
    {
        roomId ??= GetRoomId();
        return _games.TryGetValueAs(roomId, out game);
    }

    private string GetRoomId()
    {
        return Context.Items.TryGetValueAs(Room, out string roomName)
            ? roomName
            : throw new InvalidOperationException("Une Room id not found");
    }
    
    #endregion
}
