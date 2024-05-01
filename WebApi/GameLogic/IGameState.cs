using WebApi.Models;

namespace WebApi.GameLogic;

public interface IGameState {
  bool SetSpectatorsOnly(bool spectatorsOnly);
  bool SetPlayerLimit(int limit); // might remove; might be more relevant for playerOrder
  bool AddPlayer(string playerName);
  bool RemovePlayer(string playerName);
  bool KickPlayer(string playerName);
  bool SetPlayerToActive(string playerName);
  bool SetPlayerToAFK(string playerName);
  bool SetPlayerToSpectator(string playerName);
  bool DrawCard(string playerName);
  void NextTurn(); //updates state to move to next turn when necessary
  string ToString();
}