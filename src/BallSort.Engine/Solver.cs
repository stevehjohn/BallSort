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
    
    private readonly HashSet<byte[]> _visited = new(new BoardHashEqualityComparer());

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
            
            return (true, moveList);
        }

        return (false, null);
    }

    private bool Explore()
    {
        var moves = _moveGenerator.GetMoves();
        
        foreach (var move in moves)
        {
            _board.Move(move);

            _moves.Push(move);

            if (_board.IsSolved())
            {
                return true;
            }

            var hash = _boardHasher.GetHash();

            if (! _visited.Add(hash))
            {
                _board.UndoLastMove();

                _moves.Pop();
                    
                continue;
            }

            if (Explore())
            {
                return true;
            }

            _board.UndoLastMove();
            
            _moves.Pop();
        }

        return false;
    }
}