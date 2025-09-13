using System.Text.Json;
using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Infrastructure;

public class PuzzleManager
{
    private readonly List<Board> _puzzles;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Board GetPuzzle(int puzzleNumber) => _puzzles[puzzleNumber].Clone();

    public PuzzleManager(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidOperationException("Please pass in a valid path.");
        }

        var puzzleJson = File.ReadAllText(path);

        var puzzles = JsonSerializer.Deserialize<Puzzle[]>(puzzleJson, JsonSerializerOptions);

        _puzzles = new List<Board>();

        foreach (var puzzle in puzzles)
        {
            var board = new Board(puzzle);

            _puzzles.Add(board);
        }
    }
}