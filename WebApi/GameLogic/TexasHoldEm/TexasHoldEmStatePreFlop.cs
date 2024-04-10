using WebApi.Models;
using WebApi.GameLogic;
using WebApi.Common;
namespace WebApi.GameLogic.TexasHoldEmStates;

public class TexasHoldEmStatePreFlop : TexasHoldEmState
{
  public TexasHoldEmStatePreFlop(TexasHoldEmGame game) : base(game) { }

  public override bool SetSpectatorsOnly(bool spectatorsOnly)
  {
    throw new NotImplementedException();
  }

  public override bool SetPlayerLimit(int limit)
  {
    throw new NotImplementedException();
  }

  public override bool AddPlayer(string playerName)
  {
    // add spectator if possible
    if (game.PlayerOrder.AddPlayer(playerName)) {
      game.Players[playerName] = new PokerPlayer(playerName);
      game.PlayerOrder.SetPlayerStatus(playerName, Common.LyssiePlayerOrder.LyssiePlayerStatus.Spectator);
      return true;
    }
    else {
      return false;
    }
  }

  public override bool RemovePlayer(string playerName)
  {
    if (game.PlayerOrder.RemovePlayer(playerName)) { // remove from playerorder
      game.Players.Remove(playerName); // remove from game
      return true;
      // correct PlayerWhoLastRaised

    }
    else {
      return false;
    }
  }



  public override bool KickPlayer(string playerName)
  {
    throw new NotImplementedException();
  }




  public override bool SetPlayerToActive(string playerName)
  {
    Console.WriteLine($"Can't set {playerName} to active, pre-flop");
    return false;
  }

  public override bool SetPlayerToAFK(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool SetPlayerToSpectator(string playerName)
  {
    Console.WriteLine($"Can't set {playerName} to spectator; setting up");
    return false;
  }

  public override bool DrawCard(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool Call(string playerName)
  {
    Console.WriteLine($"{playerName} called");
    // if they're the current player
    // and they haven't folded yet
    // and if they're not the last person who raised
    if (playerName == game.PlayerOrder.GetCurrentPlayer() && game.Players[playerName].Folded != true && playerName != game.LastPlayerWhoRaised) {
      int betIncrease = game.CurrentBet - game.Players[playerName].CurrentBet;
      game.Players[playerName].CurrentBet += betIncrease;
      game.Players[playerName].Pot -= betIncrease;
      Console.WriteLine("Valid call");
      // skip anyone who's folded
      do {
        game.PlayerOrder.NextTurn();
      } while (game.Players[game.PlayerOrder.GetCurrentPlayer()].Folded);
      return true;
    }
    else {
      return false;
    }
  }

  public override bool Check(string playerName)
  {
    Console.WriteLine("Can't check during preflop");
    return false;
  }

  public override bool Fold(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool Raise(string playerName, int amtRaised)
  {
    throw new NotImplementedException();
  }

  public override void NextTurn()
  {
    throw new NotImplementedException();
  }

  public override void RoundStart()
  {
    throw new NotImplementedException();
  }

  public override void RoundEnd()
  {
    throw new NotImplementedException();
  }
  
  public override void Setup()
  {
    throw new NotImplementedException();
  }

  public override string ToString()
  {
    return "Pre-Flop";
  }
}

/**************************************************************
public class TexasHoldEmStatePreFlop : TexasHoldEmState
{
  public TexasHoldEmStatePreFlop(TexasHoldEmGame game) : base(game) { }

  public override bool SetSpectatorsOnly(bool spectatorsOnly)
  {
    throw new NotImplementedException();
  }

  public override bool SetPlayerLimit(int limit)
  {
    throw new NotImplementedException();
  }

  public override bool AddPlayer(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool RemovePlayer(string playerName)
  {
    throw new NotImplementedException();
  }



  public override bool KickPlayer(string playerName)
  {
    throw new NotImplementedException();
  }




  public override bool SetPlayerToActive(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool SetPlayerToAFK(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool SetPlayerToSpectator(string playerName)
  {
    throw new NotImplementedException();
  }
  
  public override bool DrawCard(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool Call(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool Check(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool Fold(string playerName)
  {
    throw new NotImplementedException();
  }

  public override bool Raise(string playerName)
  {
    throw new NotImplementedException();
  }

  public override void NextTurn()
  {
    throw new NotImplementedException();
  }

  public override void RoundStart()
  {
    throw new NotImplementedException();
  }

  public override void RoundEnd()
  {
    throw new NotImplementedException();
  }
  
  public override void Setup()
  {
    throw new NotImplementedException();
  }

  public override string ToString()
  {
    return "PreFlop";
  }
}
********************************************************/