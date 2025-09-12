using BallSort.Engine.Game;
using BallSort.Engine.Logic;
using BallSort.Engine.Models;

namespace BallSort.Engine;

public class Solver
{
    private readonly Board _board;

    private readonly MoveGenerator _moveGenerator;

    private readonly List<Move> _moves = [];
    
    public Solver(Board board)
    {
        _board = board;

        _moveGenerator = new MoveGenerator(_board);
    }

    public List<Move> Solve()
    {
        _moves.Clear();
        
        return _moves;
    }
}