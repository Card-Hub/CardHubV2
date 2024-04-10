using WebApi.Models;
using WebApi.GameLogic;
using WebApi.Common;
namespace WebApi.GameLogic.TexasHoldEmStates;

class TexasHoldEmStateNotStarted : TexasHoldEmState {
  public TexasHoldEmStateNotStarted(TexasHoldEmGame game) : base(game) {}

  public override bool SetSpectatorsOnly(bool spectatorsOnly)
  {
    this.game.SpectatorsOnly = spectatorsOnly;
    Console.WriteLine("(in NotStarted) Set spectatorsOnly");
    return true;
  }
  public override bool SetPlayerLimit(int limit)
  {
    throw new NotImplementedException();
  }
  public override bool AddPlayer(string playerName)
  {
    if (game.PlayerOrder.AddPlayer(playerName)) {
      game.Players[playerName] = new PokerPlayer(playerName);
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
    }
    else {
      return false;
    }
  }

  public override bool KickPlayer(string playerName)
  {
    return RemovePlayer(playerName);
  }

  public override bool SetPlayerToActive(string playerName)
  {
    Console.WriteLine($"Can't set {playerName} to active; setting up");
    return false;
  }

  public override bool SetPlayerToAFK(string playerName)
  {
    Console.WriteLine($"Set {playerName} to AFK");
    return game.PlayerOrder.SetPlayerStatus(playerName, Common.LyssiePlayerOrder.LyssiePlayerStatus.Afk);
  }

  public override bool SetPlayerToSpectator(string playerName)
  {
    return game.PlayerOrder.SetPlayerStatus(playerName, Common.LyssiePlayerOrder.LyssiePlayerStatus.Spectator);
  }

  public override bool DrawCard(string playerName)
  {
    Console.WriteLine($"{playerName} tried to draw a card, couldn't.");
    return false;
  }

  public override bool Call(string playerName)
  {
    Console.WriteLine($"{playerName} tried to call, couldn't.");
    return false;
  }

  public override bool Check(string playerName)
  {
    Console.WriteLine($"{playerName} tried to check, couldn't.");
    return false;
  }

  public override bool Fold(string playerName)
  {
    Console.WriteLine($"{playerName} tried to fold, couldn't.");
    return false;
  }

  public override bool Raise(string playerName, int amtRaised)
  {
    Console.WriteLine($"{playerName} tried to fold, couldn't.");
    return false;
  }

  public override void NextTurn()
  {
    throw new Exception("Can't go to the next turn, the game hasn't started!");
  }

  public override void RoundStart()
  {
    if (game.PlayerOrder.GetPlayers(Common.LyssiePlayerOrder.LyssiePlayerStatus.Active).Count >= 2) {
      Console.WriteLine("Round starting!");
      game.State = new TexasHoldEmStateSetup(game);
      game.State.Setup();
    }
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
    return "Not Started";
  }
}