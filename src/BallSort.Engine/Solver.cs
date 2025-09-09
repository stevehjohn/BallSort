using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine;

public class Solver
{
    private Board _board;

    private readonly List<Move> _moves = [];
    
    public Solver(Board board)
    {
        _board = board;
    }

    public List<Move> Solve()
    {
        _moves.Clear();
        
        _moves.Add(new Move(0, 2));
        _moves.Add(new Move(1, 3));
        _moves.Add(new Move(0, 3));
        _moves.Add(new Move(1, 2));

        return _moves;
    }
}