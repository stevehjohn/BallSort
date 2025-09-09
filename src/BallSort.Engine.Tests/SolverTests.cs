using BallSort.Engine.Models;
using BallSort.Engine.Tests.Extensions;
using Xunit;

namespace BallSort.Engine.Tests;

public class SolverTests
{
    // [Theory]
    // [InlineData("1,2,2,1,0,0,0,0", 2, 4, "0,2|1,3|0,3|1,2")]
    public void SolveReturnsExpectedMoves(string layout, int width, int height, string moves)
    {
        var solver = new Solver(layout.BoardFromLayout(width, height));

        var solution = solver.Solve();

        var expectedMoves = new List<Move>();

        var movesSplit = moves.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var move in movesSplit)
        {
            var moveSplit = move.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
            expectedMoves.Add(new Move(int.Parse(moveSplit[0]), int.Parse(moveSplit[1])));
        }
        
        Assert.Equal(expectedMoves.Count, solution.Count);

        var i = 0;
        
        foreach (var expectedMove in expectedMoves)
        {
            Assert.Equal(expectedMove, solution[i]);
            
            i++;
        }
    }
}