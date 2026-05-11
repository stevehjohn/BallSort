using BallSort.Engine.Game;
using BallSort.Engine.Logic;
using BallSort.Engine.Models;

namespace BallSort.Engine;

public class Solver
{
    private readonly Board _board;

    private readonly MoveGenerator _moveGenerator;

    private readonly BoardHasher _boardHasher;

    private readonly Stack<Move> _moves = [];

    private readonly HashSet<ulong[]> _visited = new(new BoardHashEqualityComparer());

    private  int[] _lastTouched;

    public Solver(Board board)
    {
        _board = board;

        _moveGenerator = new MoveGenerator(_board);

        _boardHasher = new BoardHasher(_board);
    }

    public (bool Solved, List<Move> Moves) Solve()
    {
        _moves.Clear();

        _visited.Clear();

        _visited.Add(_boardHasher.GetHash());

        if (Explore())
        {
            var moveList = RemoveRedundantMoves(_moves.Reverse());

            PostProcessMoves(moveList);

            return (true, moveList);
        }

        return (false, null);
    }

    private static List<Move> RemoveRedundantMoves(IEnumerable<Move> moves)
    {
        var reducedMoves = new List<Move>();

        foreach (var move in moves)
        {
            if (reducedMoves.Count > 0)
            {
                var previousMove = reducedMoves[^1];

                if (previousMove.Source == move.Source && previousMove.Target == move.Target)
                {
                    continue;
                }
            }

            reducedMoves.Add(move);
        }

        return reducedMoves;
    }

    private bool Explore()
    {
        var lastMove = _moves.Count > 0 ? _moves.Peek() : Move.NullMove;

        var moves = _moveGenerator.GetMoves(lastMove);

        foreach (var move in moves)
        {
            _board.Move(move);

            if (_board.IsSolved())
            {
                _moves.Push(move);

                return true;
            }

            var hash = _boardHasher.GetHash();

            if (_visited.Add(hash))
            {
                _moves.Push(move);

                if (Explore())
                {
                    return true;
                }

                _moves.Pop();
            }

            _board.UndoLastMove();
        }

        return false;
    }

    private void PostProcessMoves(List<Move> moves)
    {
        _lastTouched = new int[_board.Width];
        
        while (RemoveBounces(moves)) { }
    }

    private bool RemoveBounces(List<Move> moves)
    {
        Array.Fill(_lastTouched, -1);

        for (var f = 0; f < moves.Count - 2; f++)
        {
            var first = moves[f];

            _lastTouched[first.Source] = f;

            _lastTouched[first.Target] = f;

            Move? second = null;

            Move? third = null;
            
            for (var s = f + 1; s < moves.Count; s++)
            {
                if (second == null && moves[s].Source == first.Target && moves[s].Target == first.Source)
                {
                    if (_lastTouched[moves[s].Source] == f && _lastTouched[moves[s].Target] == f)
                    {
                        second = moves[s];
                        
                        continue;
                    }
                }
                
                if (second != null && moves[s].Source == first.Source && moves[s].Target == first.Target)
                {
                    if (_lastTouched[moves[s].Source] == f && _lastTouched[moves[s].Target] == f)
                    {
                        third = moves[s];
                        
                        break;
                    }
                }

                _lastTouched[moves[s].Source] = s;

                _lastTouched[moves[s].Target] = s;
            }

            if (second != null && third != null)
            {
                moves.Remove(second.Value);
                
                moves.Remove(third.Value);
            }
        }

        return false;
    }
}