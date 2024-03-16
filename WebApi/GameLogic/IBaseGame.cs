namespace WebApi.GameLogic;

public interface IBaseGame {
  void StartGame();
  List<String> GetPlayerList();
  Boolean AddPlayer(String playerName);
  Boolean RemovePlayer( String playerName);

  //List<iCard> GetPlayerHand(string playerName);


  //void EndGame();
}