using BallSort.Engine.Game;

namespace BallSort.Engine.Logic;

public class BoardHasher
{
    private readonly Board _board;

    public BoardHasher(Board board)
    {
        _board = board;
    }

    public byte[] GetHash()
    {
        var hash = new byte[_board.Width * _board.Height];

        return hash;
    }
}