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

        var i = 0;
        
        for (var x = 0; x < _board.Width; x++)
        {
            var column = _board.GetColumn(x);

            for (var y = 0; y < _board.Height; y++)
            {
                hash[i] = (byte) column[y];

                i++;
            }
        }

        return hash;
    }
}