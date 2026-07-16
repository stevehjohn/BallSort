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
        
        while (RemoveBounces(moves) || RemoveTripleBounces(moves)) { }
    }
    
    private bool RemoveBounces(List<Move> moves)
    {
        Array.Fill(_lastTouched, -1);

        for (var firstIndex = 0; firstIndex < moves.Count - 1; firstIndex++)
        {
            var first = moves[firstIndex];

            _lastTouched[first.Source] = firstIndex;
            
            _lastTouched[first.Target] = firstIndex;

            for (var secondIndex = firstIndex + 1; secondIndex < moves.Count; secondIndex++)
            {
                var second = moves[secondIndex];

                if (second.Source == first.Target && second.Target == first.Source && _lastTouched[second.Source] == firstIndex && _lastTouched[second.Target] == firstIndex)
                {
                    moves.RemoveAt(secondIndex);
                    
                    moves.RemoveAt(firstIndex);

                    return true;
                }

                _lastTouched[second.Source] = secondIndex;
                
                _lastTouched[second.Target] = secondIndex;
            }
        }

        return false;
    }

    private bool RemoveTripleBounces(List<Move> moves)
    {
        Array.Fill(_lastTouched, -1);

        for (var f = 0; f < moves.Count - 2; f++)
        {
            var first = moves[f];

            _lastTouched[first.Source] = f;

            _lastTouched[first.Target] = f;

            var secondIndex = -1;

            var thirdIndex = -1;
            
            for (var s = f + 1; s < moves.Count; s++)
            {
                if (secondIndex == -1 && moves[s].Source == first.Target && moves[s].Target == first.Source)
                {
                    if (_lastTouched[moves[s].Source] == f && _lastTouched[moves[s].Target] == f)
                    {
                        secondIndex = s;
                        
                        continue;
                    }
                }
                
                if (secondIndex > -1 && moves[s].Source == first.Source && moves[s].Target == first.Target)
                {
                    if (_lastTouched[moves[s].Source] == f && _lastTouched[moves[s].Target] == f)
                    {
                        thirdIndex = s;
                        
                        break;
                    }
                }

                _lastTouched[moves[s].Source] = s;

                _lastTouched[moves[s].Target] = s;
            }

            if (secondIndex > -1 && thirdIndex > -1)
            {
                moves.RemoveAt(thirdIndex);
                
                moves.RemoveAt(secondIndex);

                return true;
            }
        }

        return false;
    }
}