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

    private List<Move> _bestMoves;

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

        _bestMoves = null;

        _visited.Add(_boardHasher.GetHash());

        Explore();

        return _bestMoves == null
            ? (false, null)
            : (true, _bestMoves);
    }

    private void Explore()
    {
        if (_bestMoves != null && _moves.Count >= _bestMoves.Count)
        {
            return;
        }

        var lastMove = _moves.Count > 0 ? _moves.Peek() : Move.NullMove;

        var moves = _moveGenerator.GetMoves(lastMove);

        foreach (var move in moves)
        {
            _board.Move(move);

            _moves.Push(move);

            if (_board.IsSolved())
            {
                SaveBestSolution();
            }
            else
            {
                var hash = _boardHasher.GetHash();

                if (_visited.Add(hash))
                {
                    Explore();

                    _visited.Remove(hash);
                }
            }

            _moves.Pop();

            _board.UndoLastMove();
        }
    }

    private void SaveBestSolution()
    {
        if (_bestMoves != null && _moves.Count >= _bestMoves.Count)
        {
            return;
        }

        _bestMoves = _moves.Reverse().ToList();
    }
}