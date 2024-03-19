using WebApi.Models;

namespace WebApi.GameLogic.TexasHoldEmStates;

public interface ITexasHoldEmState : IGameState{
  bool Call(string playerName);
  bool Fold(string playerName);
  bool Raise(string playerName);
  bool Check(string playerName);
}