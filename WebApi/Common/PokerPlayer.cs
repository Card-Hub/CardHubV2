using WebApi.Common;

namespace WebApi.Models;

public class PokerPlayer : CardPlayerLyssie<StandardCard> {
  public int AmountOfMoneyLeft { get; set; }
  public int CurrentBet { get; set; }
  public bool CurrentlyPlaying { get; set; } // someone can sit out a game. they are not playing
  public bool CanAffordToPlay { get; set; } // someone can afford to play. if not, they sit out and must fold
  public bool Folded { get; set; } // they have folded and are not playing
  public bool CanFold {get; set; } // they can fold. only true on their turn
  public bool CanRaise {get; set; } // they can raise. only true on their turn
  public bool CanCall {get; set; } // they can call. only true on their turn
  public bool CanCheck {get; set; } // they can check. only true on their turn
  public bool HasDeclaredWhetherPlaying { get; set; } // between rounds, they declare whether or not they're playing the next round
  public PokerPlayer(string name, string connectionString) : base(name, connectionString) {
    AmountOfMoneyLeft = 0;
    CurrentBet = 0;
    Afk = false;
    Folded = false;
    CanFold = false;
    CanRaise = false;
    CanCall = false;
    CanCheck = false;
  }
}