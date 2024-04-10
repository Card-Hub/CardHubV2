namespace WebApi.Models;

public class PokerPlayer : Player {
  public int Pot { get; set; }
  public int CurrentBet { get; set; }
  public bool Afk { get; set; }
  public bool Folded { get; set; }
  public List<StandardCard> Hand;
  public PokerPlayer(string name) : base(name) {
    Pot = 0;
    CurrentBet = 0;
    Afk = false;
    Folded = false;
    Hand = new List<StandardCard>();
  }
}