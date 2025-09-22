using System.Runtime.CompilerServices;
using BallSort.Engine.Exceptions;
using BallSort.Engine.Extensions;
using BallSort.Engine.Models;

namespace BallSort.Engine.Game;

public class Board
{
    private Stack<Colour>[] _columns;

    private readonly HashSet<Colour> _colours = [];

    private Stack<Move> _history = [];
    
    public int Width { get; private init; }

    public int Height { get; private init; }

    public int Colours => _colours.Count;

    private Board()
    {
    }

    public Board(Puzzle puzzle)
    {
        _columns = new Stack<Colour>[puzzle.GridWidth];

        var index = 0;

        Width = puzzle.GridWidth;

        Height = puzzle.GridHeight;

        for (var column = 0; column < Width; column++)
        {
            _columns[column] = new Stack<Colour>(Height);

            for (var row = 0; row < Height; row++)
            {
                var ball = (Colour) puzzle.Data.Layout[index];

                if (ball != Colour.Empty)
                {
                    _columns[column].Push(ball);

                    _colours.Add(ball);
                }

                index++;
            }
        }
    }

    public void Move(Move move, bool force = false)
    {
        var source = move.Source;

        var target = move.Target;
            
        Guard(source, "Source column {column} is out of bounds.");

        if (_columns[source].Count == 0)
        {
            throw new InvalidMoveException($"Source column {source} is empty. Move id {move.Id}.");
        }

        var sourceBall = _columns[source].Peek();

        Guard(target,  $"Target column {move.Target} is out of bounds. Move id {move.Id}.");

        if (Height - _columns[target].Count == 0)
        {
            throw new InvalidMoveException($"Target column {target} is full. Move id {move.Id}.");
        }

        var targetBall = Top(target);

        if (! force && targetBall != Colour.Empty && sourceBall != targetBall)
        {
            throw new InvalidMoveException($"Cannot move {sourceBall.ToHumanReadable()} ball from column {source} onto {targetBall.ToHumanReadable()} ball in column {target}. Move id {move.Id}.");
        }

        _columns[target].Push(_columns[source].Pop());

        _history.Push(new Move(source, target));
    }

    public void Move(int source, int target, bool force = false)
    {
        Move(new Move(source, target), force);
    }

    public void UndoLastMove()
    {
        if (_history.Count > 0)
        {
            var lastMove = _history.Pop();
            
            Move(lastMove.Target, lastMove.Source, true);
        }
    }

    public Board Clone()
    {
        var board = new Board
        {
            _columns = new Stack<Colour>[Width],
            Width = Width,
            Height = Height
        };

        for (var column = 0; column < _columns.Length; column++)
        {
            board._columns[column] = new Stack<Colour>(_columns[column].Reverse());
        }

        return board;
    }

    public Colour[] GetColumn(int column)
    {
        Guard(column);

        var data = new Colour[Height];

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
        Guard(column);

        if (_columns[column].Count != Height)
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
        Guard(column);

        return _columns[column].Count == 0;
    }

    public int BallCount(int column)
    {
        Guard(column);

        return _columns[column].Count;
    }

    public Colour Top(int column)
    {
        Guard(column);

        if (_columns[column].Count == 0)
        {
            return Colour.Empty;
        }

        return _columns[column].Peek();
    }

    public int TopRunLength(int column)
    {
        Guard(column);

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
        return Capacity(column) == 0;
    }

    public int Capacity(int column)
    {
        Guard(column);

        return Height - _columns[column].Count;
    }

    public bool IsSolved()
    {
        for (var x = 0; x < Width; x++)
        {
            if (IsEmpty(x))
            {
                continue;
            }

            if (! IsComplete(x))
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Guard(int column, string messageTemplate = null)
    {
        if ((uint) column >= (uint) Width)
        {
            if (messageTemplate == null)
            {
                throw new OutOfBoundsException($"Column {column} is out of bounds.");
            }
            
            throw new OutOfBoundsException(messageTemplate.Replace("{column}", column.ToString()));
        }
    }
}