using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Logic;

public class MoveGenerator
{
    private readonly Board _board;

    private readonly List<Move> _newMoves = [];

    private Move _lastMove;

    private int _moveId;

    public MoveGenerator(Board board)
    {
        _board = board;
    }

    public List<Move> GetMoves(Move lastMove)
    {
        _lastMove = lastMove;

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

            if (_board.IsPure(x) && _board.BallCount(x) == _board.Height - 1)
            {
                continue;
            }

            GetMoves(ball, x);

            foreach (var move in _newMoves)
            {
                if (! (move.Source == _lastMove.Target && move.Target == _lastMove.Source))
                {
                    moves.Add(move);
                }
            }
        }

        return moves;
    }

    private void GetMoves(Colour ball, int source)
    {
        _newMoves.Clear();

        CheckForCompletion(ball, source);

        if (_newMoves.Count > 0)
        {
            return;
        }
        
        CheckForMerges(ball, source);

        if (_newMoves.Count > 0)
        {
            return;
        }

        if (_board.BallCount(source) > 1)
        {
            CheckForEmpty(source);
        }
    }

    private void CheckForCompletion(Colour ball, int source)
    {
        for (var x = 0; x < _board.Width; x++)
        {
            if (x == source)
            {
                continue;
            }

            if (_board.Top(x) == ball && _board.Capacity(x) == 1)
            {
                _newMoves.Add(new Move(source, x, ++_moveId));
                
                return;
            }
        }
    }

    private void CheckForEmpty(int source)
    {
        if (_board.TopRunLength(source) == _board.BallCount(source))
        {
            return;
        }

        for (var x = 0; x < _board.Width; x++)
        {
            if (x == _lastMove.Target || _board.Capacity(x) == 0)
            {
                continue;
            }

            if (_board.IsEmpty(x))
            {
                _newMoves.Add(new Move(source, x, ++_moveId));

                return;
            }
        }
    }

    private void CheckForMerges(Colour ball, int source)
    {
        for (var i = _board.Height - 2; i > 0; i--)
        {
            for (var x = 0; x < _board.Width; x++)
            {
                if (_board.Top(x) != ball || _board.Capacity(x) == 0)
                {
                    continue;
                }

                if (! IsMergeCandidate(x, source))
                {
                    continue;
                }

                if (_board.TopRunLength(x) == i)
                {
                    if (_board.IsPure(source) && _board.IsPure(x) && _board.BallCount(source) > _board.BallCount(x))
                    {
                        continue;
                    }

                    _newMoves.Add(new Move(source, x, ++_moveId));

                    return;
                }
            }
        }
    }

    private bool IsMergeCandidate(int x, int source)
    {
        if (x == source || _board.IsEmpty(x) || _board.IsFull(x) || _board.Capacity(x) == 1 || _board.IsComplete(x))
        {
            return false;
        }

        return true;
    }
}