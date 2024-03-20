using System.Collections;

namespace WebApi.Common;

public enum Direction
{
    Forward,
    Backward
}

public class PlayerOrder<TPlayer, TCard> : IEnumerable<TPlayer> where TPlayer : CardPlayer<TCard>
{
    private LinkedList<TPlayer> _players;
    private LinkedListNode<TPlayer> _currentNode;
    
    private Direction _direction = Direction.Forward;
    private const int MaxCapacity = 8;
    
    public PlayerOrder()
    {
        _players = new LinkedList<TPlayer>();
    }

    public PlayerOrder(IEnumerable<TPlayer> players)
    {
        switch (players.Count())
        {
            case > MaxCapacity:
                throw new ArgumentException("Too many players");
            case < 2:
                throw new ArgumentException("Too few players");
            default:
                _players = new LinkedList<TPlayer>(players);
                _currentNode = _players.First ?? throw new ArgumentException("No players in the list");
                break;
        }
    }

    public int Count() => _players.Count;

    public TPlayer Current() => _currentNode.ValueRef;
    
    public TPlayer Next() => GetNextNode().ValueRef;

    public TPlayer GetPlayer(string name)
    {
        var player = _players.SingleOrDefault(p => p.Name == name);
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
        return string.IsNullOrEmpty(nextPlayer.Name) ? "" : nextPlayer.Name;
    }

    public bool Add(TPlayer player)
    {
        if (_players.Count >= MaxCapacity || _players.Any(p => p.Name == player.Name))
        {
            return false;
        }

        _players.AddLast(player);
        return true;
    }

    public bool Remove(string name)
    {
        var playerToRemove = _players.SingleOrDefault(p => p.Name == name);
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
    
    public void RandomizeOrder()
    {
        if (_players.Count < 2) return;
        var random = new Random();
        var randomizedPlayers = _players.OrderBy(p => random.Next());
        _players = new LinkedList<TPlayer>(randomizedPlayers);
        _direction = Direction.Forward;
        _currentNode = _players.First ?? throw new ArgumentException("No players in the list");
    }

    private LinkedListNode<TPlayer> GetNextNode()
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

    public IEnumerator<TPlayer> GetEnumerator()
    {
        var startNode = _currentNode;
        do
        {
            yield return _currentNode.Value;
            _currentNode = GetNextNode();
        } while (startNode != _currentNode);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// Console.WriteLine("\n-- Players inserted --");
// string[] p = { "alex", "rubi", "lyssie", "liam", "juno" };
// var playerOrder = new PlayerOrder(p);
// playerOrder.PrintPlayers();
//
// Console.WriteLine("\n-- Current player --");
// Console.WriteLine(playerOrder.Current());
//
// Console.WriteLine("\n-- Next player --");
// Console.WriteLine(playerOrder.SetNextCurrent());
//
// Console.WriteLine("\n-- Han added, Lyssie removed --");
// playerOrder.Add("han");
// playerOrder.Remove("lyssie");
// playerOrder.PrintPlayers();
//
// Console.WriteLine("\n-- Liam disconnected --");
// playerOrder.SetPlayerConnectionStatus("liam", false);
// playerOrder.PrintPlayers();
//
// Console.WriteLine("\n-- Randomize order --");
// playerOrder.RandomizeOrder();
// playerOrder.PrintPlayers();
//
// Console.WriteLine("\n-- Test real game loop --");
// playerOrder.TraversePlayers();
//
// playerOrder.ToggleDirection();
// playerOrder.SetPlayerConnectionStatus("han", false);
//
// Console.WriteLine("\n-- Test real game loop (BACKWARD), Han disconnected --");
// playerOrder.TraversePlayers();