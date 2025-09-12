using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Logic;

public class MoveGenerator
{
    private readonly Board _board;

    public MoveGenerator(Board board)
    {
        _board = board;
    }

    public List<Move> GetMoves(Move lastMove)
    {
        var moves = new List<Move>();
        
        for (var i = 0; i < _board.Width; i++)
        {
            var move = CheckForColumnMove(i);

            if (move != null)
            {
                moves.Add(move.Value);
            }
        }

        return moves;
    }

    private Move? CheckForColumnMove(int column)
    {
        return null;
    }
}