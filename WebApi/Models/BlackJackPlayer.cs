using System.Diagnostics.Eventing.Reader;

namespace WebApi.Models;

public class BlackJackPlayer : Player {
  public string strConn { get; set; }
  public List<StandardCard> Hand;
  public int CurrentScore { get; set; }
  public int TotalMoney { get; set; }
  public int CurrentBet { get; set; }
  public bool HasBet { get; set; }
  public bool NotPlaying { get; set; }
  public bool Busted{ get; set;}
  public bool Winner {get; set;}
  public bool StillPlaying {get; set;}
  public bool Standing {get; set;}
  public string Namee {get; set;}
  public BlackJackPlayer(string name) : base(name) {
    strConn = "";
    CurrentScore = 0;
    CurrentBet = 0;
    Hand = new List<StandardCard>();
    NotPlaying = false;
    HasBet = false;
    Busted = false;
    Winner = false;
    StillPlaying = true;
    Namee = name;
  }

  public bool TakeCard(StandardCard card){
    Hand.Add(card);
    return true;
  }

  public List<StandardCard> ShowHand(){
    return Hand;
  }
}