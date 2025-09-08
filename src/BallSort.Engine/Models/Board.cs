using BallSort.Engine.Exceptions;

namespace BallSort.Engine.Models;

public class Board
{
    private int[][] _columns;

    private int _gridWidth;

    private int _gridHeight;

    private int _topRow;
    
    private Board()
    {
    }
    
    public Board(Puzzle puzzle)
    {
        _columns = new int[puzzle.GridWidth][];

        var index = 0;

        _gridWidth = puzzle.GridWidth;

        _gridHeight = puzzle.GridHeight;

        _topRow = _gridWidth - 1;
        
        for (var column = 0; column < _gridWidth; column++)
        {
            _columns[column] = new int[_gridHeight];

            for (var row = 0; row < _gridHeight; row++)
            {
                _columns[column][row] = puzzle.Data.Layout[index];
                
                index++;
            }
        }
    }

    public void Move(int source, int target)
    {
        if (_columns[source][0] == 0)
        {
            throw new InvalidMoveException($"Column {source} contains no balls.");
        }

        if (_columns[target][_topRow] != 0)
        {
            throw new InvalidMoveException($"Column {source} is full.");
        }
    }

    public Board Clone()
    {
        var board = new Board
        {
            _columns = new int[_gridWidth][],
            _gridWidth = _gridWidth,
            _gridHeight = _gridHeight,
            _topRow = _topRow
        };

        for (var column = 0; column < _columns.Length; column++)
        {
            for (var row = 0; row < _gridHeight; row++)
            {
                board._columns[column][row] = _columns[column][row];
            }
        }

        return board;
    }
}