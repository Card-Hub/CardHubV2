using WebApi.Models;

namespace WebApi.GameLogic {

  public interface IBaseGame<CardType> {
    void StartGame();

    bool InitDeck(); // Sets up deck
    List<string> GetPlayerList();
    bool AddPlayer(string playerName, string connStr);
    bool RemovePlayer(string playerName);
    List<CardType> GetPlayerHand(string playerName);
    List<string> GetPlayersInOrder();

    bool DrawCard(string playerName);
    bool ResetForNextRound();
    void EndGame();
  }
}