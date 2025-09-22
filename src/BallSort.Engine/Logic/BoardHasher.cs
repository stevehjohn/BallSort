using BallSort.Engine.Game;

namespace BallSort.Engine.Logic;

public class BoardHasher
{
    private readonly Board _board;

    public BoardHasher(Board board)
    {
        _board = board;
    }

    public UInt128[] GetHash()
    {
        var hash = new UInt128[_board.Width];

        for (var x = 0; x < _board.Width; x++)
        {
            var column = _board.GetColumn(x);

            for (var y = 0; y < _board.Height; y++)
            {
                hash[x] |= (UInt128) (byte) column[y] << (y * 5);
            }
        }

        Array.Sort(hash);
        
        return hash;
    }
}