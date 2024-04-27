using System.Collections;

namespace WebApi.Common;

public enum Direction
{
    Forward,
    Backward
}

public class PlayerOrder : IEnumerable<string>
{
    private LinkedList<string> _players;
    private LinkedListNode<string>? _currentNode;
    
    private Direction _direction = Direction.Forward;
    private const int MaxCapacity = 8;
    
    public PlayerOrder()
    {
        _players = new LinkedList<string>();
    }

    public PlayerOrder(IEnumerable<string> players)
    {
        switch (players.Count())
        {
            case > MaxCapacity:
                throw new ArgumentException("Too many players");
            case < 2:
                throw new ArgumentException("Too few players");
            default:
                _players = new LinkedList<string>(players);
                _currentNode = _players.First ?? throw new ArgumentException("No players in the list");
                break;
        }
    }

    public int Count() => _players.Count;

    public string Current() => _currentNode.Value;
    
    public string Next() => GetNextNode().Value;

    public string GetPlayer(string name)
    {
        var player = _players.SingleOrDefault(p => p == name);
        return player ?? throw new ArgumentException("Player not found");
    }

    public void ToggleDirection()
    {
        _direction = _direction == Direction.Forward ? Direction.Backward : Direction.Forward;
    }
    
    public string SetNextCurrent(int playersToIterate = 1)
    {
        if (_players.First is null || _players.Last is null || playersToIterate < 1)
        {
            return "";
        }

        while (playersToIterate > 0)
        {
            _currentNode = GetNextNode();
            playersToIterate--;
        }

        var nextPlayer = _currentNode.Value;
        return string.IsNullOrEmpty(nextPlayer) ? "" : nextPlayer;
    }

    public bool Add(string player)
    {
        if (_players.Count >= MaxCapacity || _players.Any(p => p == player))
        {
            return false;
        }

        _players.AddLast(player);
        if (_currentNode is null)
        {
            _currentNode = _players.First ?? throw new ArgumentException("No players in the list");
        }
        
        return true;
    }

    public bool Remove(string name)
    {
        var playerToRemove = _players.SingleOrDefault(p => p == name);
        if (playerToRemove == null)
        {
            return false;
        }

        if (playerToRemove == _currentNode.Value)
        {
            SetNextCurrent();
        }

        _players.Remove(playerToRemove);
        return true;
    }
    
    public void ShuffleOrder()
    {
        if (_players.Count < 2) return;
        var random = new Random();
        var randomizedPlayers = _players.OrderBy(p => random.Next());
        _players = new LinkedList<string>(randomizedPlayers);
        _direction = Direction.Forward;
        _currentNode = _players.First ?? throw new ArgumentException("No players in the list");
    }
    
    private LinkedListNode<string> GetNextNode()
    {
        if (_players.First is null || _players.Last is null)
        {
            throw new InvalidOperationException("No players in the list");
        }

        return _direction switch
        {
            Direction.Forward => _currentNode.Next ?? _players.First,
            Direction.Backward => _currentNode.Previous ?? _players.Last,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public IEnumerator<string> GetEnumerator()
    {
        var startNode = _currentNode;
        if (startNode is null)
        {
            yield break;
        }
        do
        {
            yield return _currentNode.ValueRef;
            _currentNode = GetNextNode();
        } while (startNode != _currentNode);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}