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
    return game.PlayerOrder.AddPlayer(playerName);
  }

  public override bool RemovePlayer(string playerName)
  {
    return game.PlayerOrder.RemovePlayer(playerName);
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

  public override string ToString()
  {
    throw new NotImplementedException();
  }
}