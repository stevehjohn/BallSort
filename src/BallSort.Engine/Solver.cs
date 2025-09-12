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

    public List<Move> Solve()
    {
        _moves.Clear();

        _visited.Add(_boardHasher.GetHash());

        while (! _board.IsSolved())
        {
            ExploreMoves(_moveGenerator.GetMoves());
        }
        
        return _moves.ToList();
    }

    private void ExploreMoves(List<Move> moves)
    {
        foreach (var move in moves)
        {
            _board.Move(move);

            var hash = _boardHasher.GetHash();

            if (! _visited.Add(hash))
            {
                _board.Move(move.Target, move.Source);
                    
                continue;
            }

            _moves.Push(move);
            
            ExploreMoves(_moveGenerator.GetMoves());
        }
    }
}