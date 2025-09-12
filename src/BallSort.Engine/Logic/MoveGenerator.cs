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
        
        for (var x = 0; x < _board.Width; x++)
        {
            var ball = _board.Top(x);

            var move = GetBestMove(ball, x);

            if (move != null)
            {
                if (move.Value.Source != lastMove.Target && move.Value.Target != lastMove.Source)
                {
                    moves.Add(move.Value);
                }
            }
        }

        return moves;
    }

    private Move? GetBestMove(Colour ball, int source)
    {
        for (var x = 0; x < _board.Width; x++)
        {
            if (x == source)
            {
                continue;
            }
        }

        return null;
    }
}