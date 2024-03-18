using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Models;
namespace WebApi.GameLogic;

public class TexasHoldEmGame : IBaseGame<StandardCard>
{
  enum TexasGameState {
    NotStarted = 0,
    PreFlop = 1,
    PostFlop = 2,
    Turn = 3, // 4th card, or 4th street
    River = 4, // 5th card, or 5th street
    BetweenHands = 5
  }
  private TexasGameState _state;
  public string GetGameState() {
    switch (_state) {
      case TexasGameState.NotStarted:
        return "Not Started";
      case TexasGameState.PreFlop:
        return "Pre-Flop";
      case TexasGameState.PostFlop:
        return "Post-Flop";
      case TexasGameState.Turn:
        return "Turn";
      case TexasGameState.River:
        return "River";
      case TexasGameState.BetweenHands:
        return "Between Hands";
      default:
        throw new SwitchExpressionException("Game state not added to switch case");
    }
  }
  private TexasGameState GetEnumGameState() {
    return this._state;
  }
  //private
  public bool AddPlayer(string playerName)
  {
    throw new NotImplementedException();
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
    throw new NotImplementedException();
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