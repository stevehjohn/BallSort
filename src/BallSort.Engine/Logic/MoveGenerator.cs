using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Logic;

public class MoveGenerator
{
    private readonly Board _board;

    private readonly BoardHasher _boardHasher;

    public MoveGenerator(Board board)
    {
        _board = board;

        _boardHasher = new BoardHasher(_board);
    }

    public (bool CanMove, Move Move) GetNextMove()
    {
        return (false, Move.NullMove);
    }
}