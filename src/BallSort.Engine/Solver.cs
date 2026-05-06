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

    private int[] _lastChanged;

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

    private void PostProcessMoves(List<Move> moves)
    {
        _lastChanged = new int[_board.Width];

        while (RemoveBounces(moves)) { }
    }

    private bool RemoveBounces(List<Move> moves)
    {
        var first = -1;

        var second = -1;
        
        for (var c = 0; c < moves.Count; c++)
        {
            var currentMove = moves[c];

            _lastChanged[currentMove.Source] = c;

            _lastChanged[currentMove.Target] = c;

            for (var p = c - 1; p >= 0; p--)
            {
                var previousMove = moves[p];

                if (previousMove.Source == currentMove.Target && previousMove.Target == currentMove.Source && first == -1)
                {
                    first = p;
                    
                    continue;
                }

                if (previousMove.Source == currentMove.Source && previousMove.Target == currentMove.Target && first > -1 && second == -1)
                {
                    second = p;
                }

                if (first != -1 && second != -1)
                {
                    moves.RemoveAt(first);
                    
                    moves.RemoveAt(second);
                }
            }
        }

        return false;
    }
}