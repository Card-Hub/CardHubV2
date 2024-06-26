using WebApi.GameLogic;
using WebApi.Models;
using WebApi.Common;

namespace WebApi.GameLogic.LyssieUno;


// don't touch this! it's in a very particular order
public class UnoJsonStateLyssie {
  public string GameType = "Une";
  public int TimerAmt = 5;
  public bool GameStarted = false;
  public string CurrentColor = "";
  public string Direction = "";
  public string CurrentPlayer = "";
  public bool SomeoneNeedsToSelectWildColor = false;
  public string PlayerWhoHasUnoPrompt = "";
  public string Winner = "";
  public List<UnoCardModLyssie> DiscardedCards = new();
  public int DeckCardCount = -1;
  public List<UnoPlayerLyssie> ActivePlayers = new();
  public List<UnoPlayerLyssie> Spectators = new();

  public UnoJsonStateLyssie() { }

  public void Update(UnoGameModLyssie game) {
    GameStarted = game.GameStarted;
    CurrentColor = game.GetCurrentColor();
    Direction = game.GetDirection();
    if (GameStarted) {
      CurrentPlayer = game._players[game.GetCurrentPlayer()].Name;
    }
    else {
      CurrentPlayer = "";
    }
    SomeoneNeedsToSelectWildColor = game.SomeoneNeedsToSelectWildColor;
    PlayerWhoHasUnoPrompt = game.PlayerWhoHasUnoPrompt;
    Winner = game.Winner;
    DiscardedCards = game._discardPile.ToList<UnoCardModLyssie>();
    DeckCardCount = game.DeckCardCount;
    ActivePlayers = game.GetActivePlayers();
    Spectators = game.GetSpectators();
  }
}