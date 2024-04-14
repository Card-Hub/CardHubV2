
using NetTopologySuite.Algorithm;

namespace WebApi.Common.LyssiePlayerOrder;

public class LyssiePlayerOrder : LyssieIPlayerOrder
{
  private List<string> ActivePlayers;
  private List<string> SpectatorPlayers;
  private Dictionary<string, bool> AfkPlayersDict;
  private int CurrentPlayerIndex;
  public int CPI { get { return CurrentPlayerIndex;}}
  public int? ForcedNextPlayerIndex {get; set;}
  public Direction DirectionInt {get;set;}
  public LyssiePlayerOrder() {
    this.ActivePlayers = new();
    this.SpectatorPlayers = new();
    this.AfkPlayersDict = new();
    this.CurrentPlayerIndex = 0;
    this.DirectionInt = Direction.Forward;
    this.ForcedNextPlayerIndex = null;
  }
  public List<string> GetAllPlayers()
  {
    List<string> AllPlayers = new();
    AllPlayers.AddRange(ActivePlayers);
    AllPlayers.AddRange(SpectatorPlayers);
    return AllPlayers;
  }
  public bool AddPlayer(string playerName)
  {
    ActivePlayers.Add(playerName);
    AfkPlayersDict[playerName] = false;
    return true;
  }

/// <summary>
/// <para>
/// Gets the active players in order.
/// </para>
/// <para>
/// <b>Note</b>: Will ignore the fact that a player might be set to go next via the SetNextPlayer() method.
/// </para>
/// </summary>
///<returns>List{string}</returns>
  public List<string> GetActivePlayersInOrder()
  {
    int i = CurrentPlayerIndex;
    List<string> inOrder = new();
    do {
      inOrder.Add(ActivePlayers.ElementAt(i));
      i += (int) DirectionInt;
      // handle too much or too little
      i %= ActivePlayers.Count; // too much
      if (i < 0) {
        i += ActivePlayers.Count;
      }
    }
    while (i != CurrentPlayerIndex);
    return inOrder;
  }

  public void ShufflePlayers() {
    Random rng = new Random();
    ActivePlayers = ActivePlayers.OrderBy(_ => rng.Next()).ToList();
  }
  public string GetCurrentPlayer()
  {
    if (ActivePlayers.Count != 0) {
      return ActivePlayers.ElementAt(CurrentPlayerIndex);
    }
    else {
      throw new IndexOutOfRangeException("Cannot get current player; no active players");
    }
  }

  public List<string> GetPlayers(LyssiePlayerStatus status)
  {
    switch (status) {
      case LyssiePlayerStatus.Active:
        return ActivePlayers;
      case LyssiePlayerStatus.Spectator:
        return SpectatorPlayers;
      case LyssiePlayerStatus.Afk:
        List<string> afkPlayers = new();
        foreach (string playerName in GetAllPlayers()) {
          if (AfkPlayersDict[playerName]) {
            afkPlayers.Add(playerName);
          }
        }
        return afkPlayers;
      case LyssiePlayerStatus.NotAfk:
        List<string> notAfkPlayers = new();
        foreach (string playerName in GetAllPlayers()) {
          if (!AfkPlayersDict[playerName]) {
            notAfkPlayers.Add(playerName);
          }
        }
        return notAfkPlayers;
      default:
        throw new ArgumentException("Cannot get players; invalid status provided");
    }
  }
  /// <summary>
  /// Returns true if player is Active, false if player is Spectator. Throws exception if player is not in game.
  /// </summary>
  /// <exception cref="ArgumentException"></exception>

  public bool IsPlayerActive(string playerName) 
  {
    if (ActivePlayers.Contains(playerName)) {
      return true;
    }
    else if (SpectatorPlayers.Contains(playerName)) {
      return false;
    }
    else {
      throw new ArgumentException("Cannot get  player activeness; player name is not in any of the lists");
    }
  }
  public bool IsPlayerAfk(string playerName) 
  {
    if (GetAllPlayers().Contains(playerName)) {
      return AfkPlayersDict[playerName];
    }
    else {
      throw new ArgumentException("Cannot get  player afkness; player name is not in any of the lists");
    }
  }

  public bool NextTurn()
  {
    if (ForcedNextPlayerIndex.HasValue) {
      CurrentPlayerIndex = ForcedNextPlayerIndex.Value;
      ForcedNextPlayerIndex = null;
    }
    else {
      CurrentPlayerIndex += (int) DirectionInt;
    }
    // loop around
    // handle index too high
    CurrentPlayerIndex %= ActivePlayers.Count;
    // handle index too low
    while (CurrentPlayerIndex < 0) {
      CurrentPlayerIndex += ActivePlayers.Count;
    }
    return true;
  }

/// <summary>
/// Removes player from player order.
/// 
/// </summary>
/// <param name="playerName"></param>
/// <returns>True if player successfully removed, false if player not in game.</returns>
  public bool RemovePlayer(string playerName)
  {
    if (GetAllPlayers().Contains(playerName)) {
      if (ActivePlayers.Contains(playerName)) {
        int RemovedPlayerIndex = ActivePlayers.IndexOf(playerName);
        ActivePlayers.Remove(playerName);
        // Correct the CurrentPlayerIndex to ensure that it is in the correct position
        // If the RemovedPlayer is sooner in the list than the CurrentPlayer, then the CPI needs to be moved down 1
        if (RemovedPlayerIndex > CurrentPlayerIndex) {
          CurrentPlayerIndex -= 1;
        }
        // If the CurrentPlayer is the one removed, and the direction int is negative, then the CPI needs to be decreased by 1
        else if (RemovedPlayerIndex == CurrentPlayerIndex && DirectionInt == Direction.Backward) {
          CurrentPlayerIndex -= 1;
        }
      }
      else {
        SpectatorPlayers.Remove(playerName);
      }
      AfkPlayersDict.Remove(playerName);
      return true;
    }
    return false;
  }

  public bool SetNextPlayer(string playerName)
  {
    if (ActivePlayers.Contains(playerName)) {
      ForcedNextPlayerIndex = ActivePlayers.IndexOf(playerName);
      return true;
    }
    else {
      return false;
    }
  }

  // Set next player to be offset * direction players away from the current player.
  public bool SetNextPlayer(int offset)
  {
    ForcedNextPlayerIndex = CurrentPlayerIndex + offset * (int) DirectionInt;
    // loop around
    // handle index too high
    ForcedNextPlayerIndex %= ActivePlayers.Count;
    // handle index too low
    while (ForcedNextPlayerIndex < 0) {
      ForcedNextPlayerIndex += ActivePlayers.Count;
    }
    return true;
  }
  public bool SetPlayerStatus(string playerName, LyssiePlayerStatus status)
  {
    if (ActivePlayers.Contains(playerName)) {
      switch (status) {
        case LyssiePlayerStatus.Active:
          break;
        case LyssiePlayerStatus.Spectator:
          ActivePlayers.Remove(playerName);
          SpectatorPlayers.Add(playerName);
          break;
        case LyssiePlayerStatus.Afk:
          AfkPlayersDict[playerName] = true;
          break;
        case LyssiePlayerStatus.NotAfk:
          AfkPlayersDict[playerName] = false;
          break;
        default: // never used
          break;
      }
      return true;
    }
    else if (SpectatorPlayers.Contains(playerName))
    { // spectator players
      switch (status) {
        case LyssiePlayerStatus.Active:
          SpectatorPlayers.Remove(playerName);
          ActivePlayers.Add(playerName);
          break;
        case LyssiePlayerStatus.Spectator:
          break;
        case LyssiePlayerStatus.Afk:
          AfkPlayersDict[playerName] = true;
          break;
        case LyssiePlayerStatus.NotAfk:
          AfkPlayersDict[playerName] = false;
          break;
        default: // never used
          break;
      }
    return true;
    }
    else {
      throw new ArgumentException($"Cannot set player status of {playerName}: not a player in current game");
    }
  }
  public bool ToggleDirection() {
    if (DirectionInt == Direction.Backward) {
      DirectionInt = Direction.Forward;
      return true;
    }
    else {
      DirectionInt = Direction.Backward;
      return true;
    }
  }
}