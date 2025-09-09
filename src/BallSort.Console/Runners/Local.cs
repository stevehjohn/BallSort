using BallSort.Console.Infrastructure;
using BallSort.Engine.Infrastructure;

namespace BallSort.Console.Runners;

public class Local
{
    public void Run(LocalOptions options)
    {
        PuzzleManager.Path = "Data/Puzzles.json";
        
        var puzzle = PuzzleManager.Instance.GetPuzzle(options.PuzzleNumber);
    }
}