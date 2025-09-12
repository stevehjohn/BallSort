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

        _visited.Add(_boardHasher.GetHash());

        var moves = _moveGenerator.GetMoves();

        var solved = false;
        
        foreach (var move in moves)
        {
            _board.Move(move);

            _visited.Add(_boardHasher.GetHash());
            
            _moves.Push(move);
            
            if (ExploreMoves(_moveGenerator.GetMoves()))
            {
                solved = true;
                
                break;
            }
            
            _board.UndoLastMove();

            _moves.Pop();
        }

        if (solved)
        {
            var moveList = _moves.ToList();

            moveList.Reverse();
            
            return (true, moveList);
        }

        return (false, null);
    }

    private bool ExploreMoves(List<Move> moves)
    {
        foreach (var move in moves)
        {
            _board.Move(move);

            if (_board.IsSolved())
            {
                return true;
            }

            var hash = _boardHasher.GetHash();

            if (! _visited.Add(hash))
            {
                _board.UndoLastMove();
                    
                continue;
            }

            _moves.Push(move);

            if (ExploreMoves(_moveGenerator.GetMoves()))
            {
                return true;
            }

            _board.UndoLastMove();
            
            _moves.Pop();
        }

        return false;
    }
}