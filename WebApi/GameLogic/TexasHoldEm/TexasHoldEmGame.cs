using System.Runtime.CompilerServices;
using WebApi.Models;
using WebApi.GameLogic.TexasHoldEmStates;
using WebApi.Common;
using WebApi.Common.LyssiePlayerOrder;
namespace WebApi.GameLogic;

public class TexasHoldEmGame : IBaseGame<StandardCard>
{
  private TexasHoldEmState _State {get; set;}
  public LyssiePlayerOrder PlayerOrder;
  public bool SpectatorsOnly {get; set;}

  public Deck<StandardCard> Deck {get; set;}

  public int LittleBlindAmt {get;set;}
  public int BigBlindAmt {get;set;}

  public TexasHoldEmGame() {
    this.PlayerOrder = new();
    this._State = new TexasHoldEmStateNotStarted(this);
    this.Deck = new();
    this.LittleBlindAmt = 1;
    this.BigBlindAmt = 2;
  }
  public void SetState(TexasHoldEmState newState) {
    this._State = newState;
  }
  public string GetGameState() {
    string modelString = "";
    modelString += "";
    string players = "";
    List<string> ActivePlayers = this.PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
    List<string> SpectatorPlayers = this.PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);

    for (int i = 0; i < ActivePlayers.Count; i++) {
      string playerName = ActivePlayers.ElementAt(i);
    }
    return $"{{{modelString}}}";
    //return _State.ToString();
  }
  //private
  public bool AddPlayer(string playerName)
  {
    return this._State.AddPlayer(playerName);
  }

  public bool DrawCard(string playerName)
  {
    throw new NotImplementedException();
  }

  public void EndGame()
  {
    throw new NotImplementedException();
  }

  public List<StandardCard> GetPlayerHand(string playerName)
  {
    throw new NotImplementedException();
  }

  public List<string> GetPlayerList()
  {
    return PlayerOrder.GetPlayers(LyssiePlayerStatus.Active);
  }
  public List<string> GetSpectatorList() {
    return this.PlayerOrder.GetPlayers(LyssiePlayerStatus.Spectator);
    //throw new NotImplementedException();
  }

  public List<string> GetPlayersInOrder()
  {
    throw new NotImplementedException();
  }

  public bool RemovePlayer(string playerName)
  {
    throw new NotImplementedException();
  }

  public void StartGame()
  {
    throw new NotImplementedException();
  }

  public bool InitDeck()
  {
    throw new NotImplementedException();
  }

  public bool ResetForNextRound()
  {
    throw new NotImplementedException();
  }
}