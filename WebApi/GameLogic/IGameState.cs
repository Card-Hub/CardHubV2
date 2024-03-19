using WebApi.Models;

namespace WebApi.GameLogic;

public interface IGameState {
  bool SetSpectatorsOnly(bool spectatorsOnly);
  bool SetPlayerLimit(string playerName); // might remove; might be more relevant for playerOrder
  bool AddPlayer(string playerName);
  bool RemovePlayer(string playerName);
  bool SetPlayerToSpectator(string playerName);
  bool SetPlayerToActive(string playerName);
  bool SetPlayerToDisconnected(string playerName);
  bool KickPlayer(string playerName);
  bool DrawCard(string playerName);
  bool RoundEnd(); // updatse state to next State
  void NextTurn(); //updates state to move to next turn when necessary
}