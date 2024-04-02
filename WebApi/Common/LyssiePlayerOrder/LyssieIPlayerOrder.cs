namespace WebApi.Common.LyssiePlayerOrder;
public enum LyssiePlayerStatus {
  Active,
  Spectator,
  Afk,
  NotAfk
}

public interface LyssieIPlayerOrder {
  public List<string> GetAllPlayers();
  public List<string> GetActivePlayersInOrder();
  public string GetCurrentPlayer();
  public List<string> GetPlayers(LyssiePlayerStatus status);
  public bool AddPlayer(string playerName);
  public bool RemovePlayer(string playerName);
  public bool IsPlayerActive(string playerName);
  public bool IsPlayerAfk(string playerName);
  public bool SetPlayerStatus(string playerName, LyssiePlayerStatus status);
  public bool SetNextPlayer(string playerName);
  public bool SetNextPlayer(int offset); // 1 for the next player in the list, 0 for repeat current player
  public bool NextTurn();
}