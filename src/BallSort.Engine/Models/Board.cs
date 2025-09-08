namespace BallSort.Engine.Models;

public class Board
{
    private int[][] _columns;
    
    public Board(Puzzle puzzle)
    {
        _columns = new int[puzzle.GridWidth][];

        var index = 0;
        
        for (var column = 0; column < puzzle.GridWidth; column++)
        {
            _columns[column] = new int[puzzle.GridHeight];

            for (var row = 0; row < puzzle.GridHeight; row++)
            {
                _columns[column][row] = puzzle.Data.Layout[index];
                
                index++;
            }
        }
    }
    
    public Board Clone()
    {
        return null;
    }
}