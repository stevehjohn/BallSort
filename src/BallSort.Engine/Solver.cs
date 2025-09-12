using BallSort.Engine.Game;
using BallSort.Engine.Logic;
using BallSort.Engine.Models;

namespace BallSort.Engine;

public class Solver
{
    private readonly Board _board;

    private readonly MoveGenerator _moveGenerator;

    private readonly Stack<Move> _moves = [];
    
    public Solver(Board board)
    {
        _board = board;

        _moveGenerator = new MoveGenerator(_board);
    }

    public List<Move> Solve()
    {
        _moves.Clear();

        while (! _board.IsSolved())
        {
            var (canMove, move) = _moveGenerator.GetNextMove();

            if (! canMove)
            {
                var lastMove = _moves.Pop();
                
                _board.Move(lastMove.Target, lastMove.Source);

                continue;
            }

            _moves.Push(move);
            
            _board.Move(move);
        }
        
        return _moves.ToList();
    }
}