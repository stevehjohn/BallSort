using BallSort.Engine.Exceptions;
using BallSort.Engine.Extensions;

namespace BallSort.Engine.Models;

public class Board
{
    private Colour[][] _columns;

    private int _gridWidth;

    private int _gridHeight;

    private int _topRow;
    
    private Board()
    {
    }
    
    public Board(Puzzle puzzle)
    {
        _columns = new Colour[puzzle.GridWidth][];

        var index = 0;

        _gridWidth = puzzle.GridWidth;

        _gridHeight = puzzle.GridHeight;

        _topRow = _gridWidth - 1;
        
        for (var column = 0; column < _gridWidth; column++)
        {
            _columns[column] = new Colour[_gridHeight];

            for (var row = 0; row < _gridHeight; row++)
            {
                _columns[column][row] = (Colour) puzzle.Data.Layout[index];
                
                index++;
            }
        }
    }

    public void Move(int source, int target)
    {
        var sourceBall = GetTopmostBall(source);
        
        if (sourceBall == Colour.Empty)
        {
            throw new InvalidMoveException($"Column {source} contains no balls.");
        }

        var targetBall = GetTopmostBall(target);
        
        if (targetBall != Colour.Empty && sourceBall != targetBall)
        {
            throw new InvalidMoveException($"Cannot move {sourceBall.ToHumanReadable()} ball from column {source} on to {targetBall.ToHumanReadable()} ball in column {target}.");
        }
    }

    public Board Clone()
    {
        var board = new Board
        {
            _columns = new Colour[_gridWidth][],
            _gridWidth = _gridWidth,
            _gridHeight = _gridHeight,
            _topRow = _topRow
        };

        for (var column = 0; column < _columns.Length; column++)
        {
            for (var row = 0; row < _gridHeight; row++)
            {
                board._columns[column] = new Colour[_gridHeight];
                
                board._columns[column][row] = _columns[column][row];
            }
        }

        return board;
    }

    private Colour GetTopmostBall(int column)
    {
        for (var i = _topRow; i >= 0; i--)
        {
            if (_columns[column][i] != Colour.Empty)
            {
                return _columns[column][i];
            }
        }

        return Colour.Empty;
    }
}