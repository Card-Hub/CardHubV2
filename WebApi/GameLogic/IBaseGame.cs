using WebApi.Models;

namespace WebApi.GameLogic {

  public interface IBaseGame<CardType> {
    void StartGame();
    List<string> GetPlayerList();
    bool AddPlayer(string playerName);
    bool RemovePlayer(string playerName);
    List<CardType> GetPlayerHand(string playerName);
    List<string> GetPlayersInOrder();

    bool DrawCard(string playerName);
    void EndGame();
  }
}