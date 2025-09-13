using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Logic;

public class MoveGenerator
{
    private readonly Board _board;

    public MoveGenerator(Board board)
    {
        _board = board;
    }

    public List<Move> GetMoves(Move lastMove)
    {
        var moves = new List<Move>();
        
        for (var x = 0; x < _board.Width; x++)
        {
            var ball = _board.Top(x);

            if (ball == Colour.Empty)
            {
                continue;
            }

            if (_board.IsComplete(x))
            {
                continue;
            }

            var newMoves = GetMoves(ball, x);

            foreach (var move in newMoves)
            {
                if (move.Source != lastMove.Target && move.Target != lastMove.Source)
                {
                    moves.Add(move);
                }
            }
        }

        return moves;
    }

    private List<Move> GetMoves(Colour ball, int source)
    {
        var moves = new List<Move>();

        var move = CheckForCompletion(ball, source);

        if (move != Move.NullMove)
        {
            moves.Add(move);
        }
        
        move = CheckForEmpty(source);

        if (move != Move.NullMove)
        {
            moves.Add(move);
        }

        return moves;
    }

    private Move CheckForCompletion(Colour ball, int source)
    {
        for (var x = 0; x < _board.Width; x++)
        {
            if (x == source)
            {
                continue;
            }

            if (_board.Top(x) == ball && _board.TopRunLength(x) == _board.Height - 1)
            {
                return new Move(source, x);
            }
        }
        
        return Move.NullMove;
    }

    private Move CheckForEmpty(int source)
    {
        for (var x = 0; x < _board.Width; x++)
        {
            if (_board.IsEmpty(x))
            {
                return new Move(source, x);
            }
        }
        
        return Move.NullMove;
    }

    // for (var x = 0; x < _board.Width; x++)
    // {
    //     if (x == source || _board.IsFull(x))
    //     {
    //         continue;
    //     }
    // }
}