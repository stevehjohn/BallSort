using BallSort.Engine.Exceptions;
using BallSort.Engine.Extensions;
using BallSort.Engine.Models;

namespace BallSort.Engine.Game;

public class Board
{
    private Stack<Colour>[] _columns;

    private int _gridWidth;

    private int _gridHeight;

    private Board()
    {
    }

    public Board(Puzzle puzzle)
    {
        _columns = new Stack<Colour>[puzzle.GridWidth];

        var index = 0;

        _gridWidth = puzzle.GridWidth;

        _gridHeight = puzzle.GridHeight;

        for (var column = 0; column < _gridWidth; column++)
        {
            _columns[column] = new Stack<Colour>(_gridHeight);

            for (var row = 0; row < _gridHeight; row++)
            {
                var ball = (Colour) puzzle.Data.Layout[index];

                if (ball != Colour.Empty)
                {
                    _columns[column].Push(ball);
                }

                index++;
            }
        }
    }

    public void Move(int source, int target)
    {
        if (source >= _gridWidth)
        {
            throw new InvalidMoveException($"Source column {source} is out of bounds");
        }

        var sourceBall = GetTopmostBall(source);
        
        if (sourceBall == Colour.Empty)
        {
            throw new InvalidMoveException($"Source column {source} contains no balls.");
        }

        if (target >= _gridWidth)
        {
            throw new InvalidMoveException($"Target column {target} is out of bounds");
        }

        if (IsFull(target))
        {
            throw new InvalidMoveException($"Target column {target} is full.");
        }

        var targetBall = GetTopmostBall(target);

        if (targetBall != Colour.Empty && sourceBall != targetBall)
        {
            throw new InvalidMoveException($"Cannot move {sourceBall.ToHumanReadable()} ball from column {source} onto {targetBall.ToHumanReadable()} ball in column {target}.");
        }
        
        _columns[target].Push(_columns[source].Pop());
    }

    public Board Clone()
    {
        var board = new Board
        {
            _columns = new Stack<Colour>[_gridWidth],
            _gridWidth = _gridWidth,
            _gridHeight = _gridHeight
        };

        for (var column = 0; column < _columns.Length; column++)
        {
            board._columns[column] = new Stack<Colour>(_columns[column].Reverse());
        }

        return board;
    }

    public Colour[] GetColumn(int column)
    {
        var data = new Colour[_gridHeight];

        var i = _columns[column].Count - 1;
        
        foreach (var item in _columns[column])
        {
            data[i] = item;

            i--;
        }
        
        return data;
    }

    private Colour GetTopmostBall(int column)
    {
        if (_columns[column].Count == 0)
        {
            return Colour.Empty;
        }

        return _columns[column].Peek();
    }

    private bool IsFull(int column)
    {
        return _columns[column].Count == _gridHeight;
    }
}