using WebApi.Models;
namespace WebApi.GameLogic;

public class TexasHoldEmGame : IBaseGame<StandardCard>
{
  public bool AddPlayer(string playerName)
  {
    throw new NotImplementedException();
  }

  public bool DrawCard(string playerName)
  {
    throw new NotImplementedException();
  }

  public void EndGame()
  {
    throw new NotImplementedException();
  }

  public List<StandardCard> GetPlayerHand(string playerName)
  {
    throw new NotImplementedException();
  }

  public List<string> GetPlayerList()
  {
    throw new NotImplementedException();
  }

  public List<string> GetPlayersInOrder()
  {
    throw new NotImplementedException();
  }

  public bool RemovePlayer(string playerName)
  {
    throw new NotImplementedException();
  }

  public void StartGame()
  {
    throw new NotImplementedException();
  }
}