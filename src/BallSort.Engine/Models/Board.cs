using BallSort.Engine.Exceptions;
using BallSort.Engine.Extensions;

namespace BallSort.Engine.Models;

public class Board
{
    private Stack<Colour>[] _columns;

    private int _gridWidth;

    private int _gridHeight;

    private int _topRow;

    private Board()
    {
    }

    public Board(Puzzle puzzle)
    {
        _columns = new Stack<Colour>[puzzle.GridWidth];

        var index = 0;

        _gridWidth = puzzle.GridWidth;

        _gridHeight = puzzle.GridHeight;

        _topRow = _gridWidth - 1;

        for (var column = 0; column < _gridWidth; column++)
        {
            _columns[column] = new Stack<Colour>(_gridHeight);

            for (var row = 0; row < _gridHeight; row++)
            {
                _columns[column].Push((Colour) puzzle.Data.Layout[index]);

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

        if (IsFull(target))
        {
            throw new InvalidMoveException($"Column {target} is full.");
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
            _columns = new Stack<Colour>[_gridWidth],
            _gridWidth = _gridWidth,
            _gridHeight = _gridHeight,
            _topRow = _topRow
        };

        for (var column = 0; column < _columns.Length; column++)
        {
            for (var row = 0; row < _gridHeight; row++)
            {
                board._columns[column] = new Stack<Colour>(_columns[column].Reverse());
            }
        }

        return board;
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