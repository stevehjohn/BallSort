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
            var moveList = _moves.Reverse().ToList();

            PostProcessMoves(moveList);

            return (true, moveList);
        }

        return (false, null);
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

    private static void PostProcessMoves(List<Move> moves)
    {
        while (RemoveBounces(moves) || CollapseForwarding(moves)) { }
    }
    
    private static bool RemoveBounces(List<Move> moves)
    {
        for (var i = 0; i < moves.Count - 1; i++)
        {
            var first = moves[i];

            for (var j = i + 1; j < moves.Count; j++)
            {
                var current = moves[j];

                if (current.Source == first.Target && current.Target == first.Source)
                {
                    moves.RemoveAt(j);
                    
                    moves.RemoveAt(i);

                    return true;
                }

                if (Touches(current, first.Source) || Touches(current, first.Target))
                {
                    break;
                }
            }
        }

        return false;
    }

    private static bool Touches(Move move, int tube)
    {
        return move.Source == tube || move.Target == tube;
    }
    
    private static bool CollapseForwarding(List<Move> moves)
    {
        for (var i = 0; i < moves.Count - 1; i++)
        {
            var first = moves[i];
            var second = moves[i + 1];

            if (first.Target != second.Source)
            {
                continue;
            }

            if (first.Source == second.Target)
            {
                continue;
            }

            moves[i] = new Move(first.Source, second.Target, first.Id);
            moves.RemoveAt(i + 1);

            return true;
        }

        return false;
    }
}