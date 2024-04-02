using WebApi.Models;

namespace WebApi.GameLogic.TexasHoldEmStates;

public interface ITexasHoldEmState : IGameState {
  void RoundStart(); // using implementation of new round/State, perform new things
  void RoundEnd(); // updates state to next State
  bool Call(string playerName);
  bool Check(string playerName);
  bool Fold(string playerName);
  bool Raise(string playerName);
}