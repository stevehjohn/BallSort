using System.Collections.Immutable;
using System.Text.Json;
using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Infrastructure;

public class PuzzleManager
{
    private readonly ImmutableArray<Board> _puzzles;

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

        var builder = ImmutableArray.CreateBuilder<Board>(puzzles.Length);
        
        foreach (var puzzle in puzzles)
        {
            var board = new Board(puzzle);

            builder.Add(board);
        }

        _puzzles = builder.ToImmutable();
    }
}