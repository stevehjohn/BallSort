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
        
    public static string Path { get; set; }

    private static readonly Lazy<PuzzleManager> Lazy = new(GetPuzzleManager);

    public static PuzzleManager Instance => Lazy.Value;

    private PuzzleManager()
    {
    }

    private static PuzzleManager GetPuzzleManager()
    {
        if (Path == null)
        {
            throw new InvalidOperationException("Please set the Path property before using the PuzzleManager.");
        }

        var puzzleJson = File.ReadAllText(Path);

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