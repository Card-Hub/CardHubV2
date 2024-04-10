using WebApi.GameLogic.TexasHoldEmStates;

namespace WebApi.GameLogic;

abstract public class TexasHoldEmState : ITexasHoldEmState
{
  protected TexasHoldEmGame game;
  public TexasHoldEmState(TexasHoldEmGame game) {
    this.game = game;
  }
  public abstract bool SetSpectatorsOnly(bool spectatorsOnly);
  public abstract bool SetPlayerLimit(int limit);
  public abstract bool AddPlayer(string playerName);
  public abstract bool RemovePlayer(string playerName);
  public abstract bool KickPlayer(string playerName);
  public abstract bool SetPlayerToActive(string playerName);
  public abstract bool SetPlayerToAFK(string playerName);
  public abstract bool SetPlayerToSpectator(string playerName);
  public abstract bool DrawCard(string playerName);
  public abstract bool Call(string playerName);
  public abstract bool Check(string playerName);
  public abstract bool Fold(string playerName);
  public abstract bool Raise(string playerName, int amtRaised);
  public abstract void NextTurn();
  public abstract void RoundStart();
  public abstract void RoundEnd();
  public abstract void Setup();
  public abstract override string ToString();
}