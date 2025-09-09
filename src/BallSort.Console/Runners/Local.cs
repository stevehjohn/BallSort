using BallSort.Console.Infrastructure;
using BallSort.Engine;
using BallSort.Engine.Infrastructure;
using static System.Console;

namespace BallSort.Console.Runners;

public class Local
{
    public void Run(LocalOptions options)
    {
        PuzzleManager.Path = "Data/Puzzles.json";
        
        var board = PuzzleManager.Instance.GetPuzzle(options.PuzzleNumber);
        
        Clear();
        
        WriteLine();
        
        WriteLine($"  Solving puzzle #{options.PuzzleNumber}: {board.Width}x{board.Height}, .");

        var solver = new Solver(board);

        var solution = solver.Solve();

        foreach (var step in solution)
        {
            
        }
    }
}