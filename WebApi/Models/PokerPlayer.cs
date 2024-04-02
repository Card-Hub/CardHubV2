using System.Security.Cryptography.X509Certificates;

namespace WebApi.Models;

public class PokerPlayer : Player {
  public List<StandardCard> Hand;
  public List<int> Bet;
  public List<int> PileOfChips;
  public PokerPlayer(string name) : base(name) {
    this.Hand = new();
    this.Bet = new();
    this.PileOfChips = new();
  }
}