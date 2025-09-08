using System.Text.Json;
using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Infrastructure;

public class PuzzleManager
{
    private List<Board> _puzzles;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Board GetPuzzle(int puzzleNumber) => _puzzles[puzzleNumber].Clone();

    private static string _path;

    private static Lazy<PuzzleManager> _lazy = new(GetPuzzleManager, LazyThreadSafetyMode.PublicationOnly);

    public static string Path
    {
        set
        {
            _path = value;
            
            _lazy = new Lazy<PuzzleManager>(GetPuzzleManager, LazyThreadSafetyMode.PublicationOnly);
        }
    }

    public static PuzzleManager Instance => _lazy.Value;

    private PuzzleManager()
    {
    }

    private static PuzzleManager GetPuzzleManager()
    {
        if (_path == null)
        {
            throw new InvalidOperationException("Please set the Path property before using the PuzzleManager.");
        }

        var puzzleJson = File.ReadAllText(_path);

        var puzzles = JsonSerializer.Deserialize<Puzzle[]>(puzzleJson, JsonSerializerOptions);

        var boards = new List<Board>();

        foreach (var puzzle in puzzles)
        {
            var board = new Board(puzzle);

            boards.Add(board);
        }
        
        var instance = new PuzzleManager
        {
            _puzzles = boards
        };
        
        return instance;
    }
}