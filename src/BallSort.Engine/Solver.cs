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

    private int[] _lastTouched;

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
        _lastTouched = new int[_board.Width];

        while (RemoveBounces(moves)) { }
    }

    private bool RemoveBounces(List<Move> moves)
    {
        Array.Fill(_lastTouched, -1);

        for (var index = 0; index < moves.Count; index++)
        {
            var move = moves[index];

            var previousIndex = _lastTouched[move.Source];

            if (previousIndex >= 0 && previousIndex == _lastTouched[move.Target])
            {
                var previous = moves[previousIndex];

                if (previous.Source == move.Target && previous.Target == move.Source)
                {
                    moves.RemoveAt(index);
                    
                    moves.RemoveAt(previousIndex);

                    return true;
                }
            }

            _lastTouched[move.Source] = index;
            
            _lastTouched[move.Target] = index;
        }

        return false;
    }
}