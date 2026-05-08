using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Logic;

public class MoveGenerator
{
    private readonly Board _board;

    private int _moveId;

    private List<Move> _newMoves = [];
    
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

            if (_board.TopRunLength(x) == _board.Height - 1 && _board.BallCount(x) == _board.Height - 1)
            {
                continue;
            }

            GetMoves(ball, x);

            foreach (var move in _newMoves)
            {
                if (! (move.Source == lastMove.Target && move.Target == lastMove.Source))
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

        CheckForMerges(ball, source);

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
                _newMoves.Add(new Move(source, x, _board.Top(source), ++_moveId));
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
            if (_board.IsEmpty(x))
            {
                _newMoves.Add(new Move(source, x, _board.Top(source), ++_moveId));
            }
        }
    }

    private void CheckForMerges(Colour ball, int source)
    {
        for (var i = _board.Height - 2; i > 0; i--)
        {
            for (var x = 0; x < _board.Width; x++)
            {
                if (_board.Top(x) != ball)
                {
                    continue;
                }

                if (! IsMergeCandidate(x, source))
                {
                    continue;
                }

                if (_board.TopRunLength(x) == i)
                {
                    _newMoves.Add(new Move(source, x, _board.Top(source), ++_moveId));

                    break;
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