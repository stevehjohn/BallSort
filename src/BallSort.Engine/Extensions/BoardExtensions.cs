using System.Text;
using BallSort.Engine.Game;

namespace BallSort.Engine.Extensions;

public static class BoardExtensions
{
    extension(Board board)
    {
        public void Dump()
        {
            Console.WriteLine(string.Empty);
        
            var builder = new StringBuilder();

            var columns = new List<Colour[]>();
        
            for (var x = 0; x < board.Width; x++)
            {
                columns.Add(board.GetColumn(x).ToArray());
            }

            for (var y = 0; y < board.Height; y++)
            {
                for (var x = 0; x < board.Width; x++)
                {
                    var ball = columns[x][y];

                    if (ball == Colour.Empty)
                    {
                        builder.Append('-');
                    }
                    else
                    {
                        builder.Append((char) ('@' + (int) columns[x][y]));
                    }
                }

                Console.WriteLine($"  {builder}");

                builder.Clear();
            }
        }
    }
}