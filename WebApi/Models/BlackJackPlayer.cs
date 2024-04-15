namespace WebApi.Models;

public class BlackJackPlayer : Player {
  public int CurrentScore { get; set; }
  public bool hasBet { get; set; }
  public int CurrentBet { get; set; }
  public bool NotPlaying { get; set; }
  public List<StandardCard> Hand;
  public BlackJackPlayer(string name) : base(name) {
    CurrentScore = 0;
    CurrentBet = 0;
    Hand = new List<StandardCard>();
    NotPlaying = false;
  }

  public bool TakeCard(StandardCard card){
    Hand.Add(card);
    return true;
  }

  public List<StandardCard> ShowHand(){
    return Hand;
  }

}