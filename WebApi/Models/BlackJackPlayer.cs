namespace WebApi.Models;

public class BlackJackPlayer : Player {
  public int CurrentBet { get; set; }
  public bool Afk { get; set; }
  public List<StandardCard> Hand;
  public BlackJackPlayer(string name) : base(name) {
    CurrentBet = 0;
    Afk = false;
    Hand = new List<StandardCard>();
  }
}