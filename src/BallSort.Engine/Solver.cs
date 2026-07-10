using BallSort.Engine.Game;
using BallSort.Engine.Logic;
using BallSort.Engine.Models;

namespace BallSort.Engine;

public class Solver
{
    private const int MaxImprovementNodes = 250_000;

    private readonly Board _board;

    private readonly MoveGenerator _moveGenerator;

    private readonly BoardHasher _boardHasher;

    private readonly Stack<Move> _moves = [];

    private readonly HashSet<ulong[]> _visited = new(new BoardHashEqualityComparer());

    private readonly Dictionary<ulong[], int> _bestDepthByHash = new(new BoardHashEqualityComparer());

    private List<Move> _bestMoves;

    private int _improvementNodes;

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

        _bestDepthByHash.Clear();

        _bestMoves = null;

        _improvementNodes = 0;

        _visited.Add(_boardHasher.GetHash());

        if (!ExploreFirstSolution())
        {
            return (false, null);
        }

        _moves.Clear();

        _bestDepthByHash.Clear();

        _bestDepthByHash.Add(_boardHasher.GetHash(), 0);

        ExploreShorterSolutions();

        return (true, _bestMoves);
    }

    private bool ExploreFirstSolution()
    {
        var lastMove = _moves.Count > 0 ? _moves.Peek() : Move.NullMove;

        var moves = _moveGenerator.GetMoves(lastMove);

        foreach (var move in moves)
        {
            _board.Move(move);

            if (_board.IsSolved())
            {
                _moves.Push(move);

                SaveBestSolution();

                return true;
            }

            var hash = _boardHasher.GetHash();

            if (_visited.Add(hash))
            {
                _moves.Push(move);

                if (ExploreFirstSolution())
                {
                    return true;
                }

                _moves.Pop();
            }

            _board.UndoLastMove();
        }

        return false;
    }

    private void ExploreShorterSolutions()
    {
        if (_improvementNodes++ >= MaxImprovementNodes)
        {
            return;
        }

        if (_moves.Count >= _bestMoves.Count - 1)
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

                if (ShouldExplore(hash, _moves.Count))
                {
                    ExploreShorterSolutions();
                }
            }

            _moves.Pop();

            _board.UndoLastMove();
        }
    }

    private bool ShouldExplore(ulong[] hash, int depth)
    {
        if (_bestDepthByHash.TryGetValue(hash, out var bestDepth) && bestDepth <= depth)
        {
            return false;
        }

        _bestDepthByHash[hash] = depth;

        return true;
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