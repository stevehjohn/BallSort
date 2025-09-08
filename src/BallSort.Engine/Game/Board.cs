using BallSort.Engine.Exceptions;
using BallSort.Engine.Extensions;
using BallSort.Engine.Models;

namespace BallSort.Engine.Game;

public class Board
{
    private Stack<Colour>[] _columns;

    private int _gridWidth;

    private int _gridHeight;

    public int Width => _gridWidth;

    public int Height => _gridHeight;

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
        if ((uint) source >= _gridWidth)
        {
            throw new OutOfBoundsException($"Source column {source} is out of bounds.");
        }

        var sourceBall = Top(source);

        if (sourceBall == Colour.Empty)
        {
            throw new InvalidMoveException($"Source column {source} contains no balls.");
        }

        if ((uint) target >= _gridWidth)
        {
            throw new OutOfBoundsException($"Target column {target} is out of bounds.");
        }

        if (IsFull(target))
        {
            throw new InvalidMoveException($"Target column {target} is full.");
        }

        var targetBall = Top(target);

        if (targetBall != Colour.Empty && sourceBall != targetBall)
        {
            throw new InvalidMoveException(
                $"Cannot move {sourceBall.ToHumanReadable()} ball from column {source} onto {targetBall.ToHumanReadable()} ball in column {target}.");
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
        if ((uint) column >= _gridWidth)
        {
            throw new OutOfBoundsException($"Column {column} is out of bounds.");
        }

        var data = new Colour[_gridHeight];

        var i = _columns[column].Count - 1;

        foreach (var item in _columns[column])
        {
            data[i] = item;

            i--;
        }

        return data;
    }

    public bool IsComplete(int column)
    {
        if ((uint) column >= _gridWidth)
        {
            throw new OutOfBoundsException($"Column {column} is out of bounds.");
        }

        if (_columns[column].Count != _gridHeight)
        {
            return false;
        }

        var colour = _columns[column].Peek();

        foreach (var item in _columns[column])
        {
            if (item != colour)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsEmpty(int column)
    {
        if ((uint) column >= _gridWidth)
        {
            throw new OutOfBoundsException($"Column {column} is out of bounds.");
        }

        return _columns[column].Count == 0;
    }

    public Colour Top(int column)
    {
        if ((uint) column >= _gridWidth)
        {
            throw new OutOfBoundsException($"Column {column} is out of bounds.");
        }

        if (_columns[column].Count == 0)
        {
            return Colour.Empty;
        }

        return _columns[column].Peek();
    }

    public int TopRunLength(int column)
    {
        if ((uint) column >= _gridWidth)
        {
            throw new OutOfBoundsException($"Column {column} is out of bounds.");
        }

        var stack = _columns[column];

        if (! stack.TryPeek(out var colour))
        {
            return 0;
        }

        var length = 1;

        var enumerator = stack.GetEnumerator();

        enumerator.MoveNext();

        while (enumerator.MoveNext())
        {
            if (enumerator.Current != colour)
            {
                break;
            }

            length++;
        }

        return length;
    }

    public bool IsFull(int column)
    {
        if ((uint) column >= _gridWidth)
        {
            throw new OutOfBoundsException($"Column {column} is out of bounds.");
        }

        return _columns[column].Count >= _gridHeight;
    }
}