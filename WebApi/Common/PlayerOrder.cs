namespace WebApi.Common;

public record PlayerInfo
{
    public string Id { get; }
    public bool IsConnected { get; set; }

    public PlayerInfo(string id, bool isConnected = true)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("Id must be provided.", nameof(id));
        Id = id;
        IsConnected = isConnected;
    }
}

public enum Direction
{
    Forward,
    Backward
}

public class PlayerOrder
{
    private LinkedList<PlayerInfo> _players;
    private LinkedListNode<PlayerInfo> _currentNode;
    private Direction _direction;
    private const int MaxCapacity = 8;

    public PlayerOrder(IEnumerable<string> players)
    {
        switch (players.Count())
        {
            case > MaxCapacity:
                throw new ArgumentException("Too many players");
            case < 2:
                throw new ArgumentException("Too few players");
            default:
                _players = new LinkedList<PlayerInfo>(players.Select(id => new PlayerInfo(id)));
                _currentNode = _players.First ?? throw new ArgumentException("No players in the list");
                _direction = Direction.Forward;
                break;
        }
    }

    public int GetPlayerCount() => _players.Count;

    public int GetActivePlayerCount() => _players.Count(p => p.IsConnected);

    public string GetCurrentPlayer()
    {
        return _currentNode.Value.Id;
    }

    public void ToggleDirection()
    {
        _direction = _direction == Direction.Forward ? Direction.Backward : Direction.Forward;
    }
    
    public string SetNextCurrentPlayer(int playersToIterate = 1)
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

        var nextPlayer = _currentNode.Value.Id;
        return string.IsNullOrEmpty(nextPlayer) ? "" : nextPlayer;
    }

    public bool AddPlayer(string id)
    {
        if (_players.Count >= MaxCapacity || _players.Any(p => p.Id == id))
        {
            return false;
        }

        _players.AddLast(new PlayerInfo(id));
        return true;
    }

    public bool RemovePlayer(string id)
    {
        var playerToRemove = _players.SingleOrDefault(p => p.Id == id);
        if (playerToRemove == null)
        {
            return false;
        }

        if (playerToRemove == _currentNode.Value)
        {
            SetNextCurrentPlayer();
        }

        _players.Remove(playerToRemove);
        return true;
    }


    public void RandomizeOrder()
    {
        if (_players.Count < 2) return;
        var random = new Random();
        var randomizedPlayers = _players.OrderBy(p => random.Next());
        _players = new LinkedList<PlayerInfo>(randomizedPlayers);
        _direction = Direction.Forward;
        var current = _players.First;
        while (current != null)
        {
            if (current.Value.IsConnected)
            {
                _currentNode = current;
                break;
            }

            current = current.Next;
        }
    }
    
    public bool SetPlayerConnectionStatus(string id, bool connectPlayer)
    {
        var playerToEdit = _players.SingleOrDefault(info => info.Id == id);
        if (playerToEdit == null || playerToEdit.IsConnected == connectPlayer)
            return false;
        
        playerToEdit.IsConnected = connectPlayer;
        return true;
    }

    private LinkedListNode<PlayerInfo> GetNextNode()
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
    
    
    
    // For debugging purposes
    public void PrintPlayers()
    {
        foreach (var player in _players)
        {
            Console.WriteLine("Player: " + player.Id + " IsConnected: " + player.IsConnected);
        }
    }

    // For debugging purposes
    public void TraversePlayers()
    {
        var startNode = _currentNode;
        do
        {
            if (_currentNode.Value.IsConnected)
            {
                Console.WriteLine("Player: " + _currentNode.Value.Id + " IsConnected: " + _currentNode.Value.IsConnected);
            } 
            _currentNode = GetNextNode();
        } while (startNode != _currentNode);
    }
}

// Console.WriteLine("\n-- Players inserted --");
// string[] p = { "alex", "rubi", "lyssie", "liam", "juno" };
// var playerOrder = new PlayerOrder(p);
// playerOrder.PrintPlayers();
//
// Console.WriteLine("\n-- Current player --");
// Console.WriteLine(playerOrder.GetCurrentPlayer());
//
// Console.WriteLine("\n-- Next player --");
// Console.WriteLine(playerOrder.SetNextCurrentPlayer());
//
// Console.WriteLine("\n-- Han added, Lyssie removed --");
// playerOrder.AddPlayer("han");
// playerOrder.RemovePlayer("lyssie");
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