using BallSort.Engine.Game;
using BallSort.Engine.Models;

namespace BallSort.Engine.Tests.Extensions;

public static class StringExtensions
{
    public static Board BoardFromLayout(this string layout, int width, int height)
    {
        var puzzle = new Puzzle
        {
            GridWidth = width,
            GridHeight = height,
            Data = new Data
            {
                Layout = new int[width * height]
            }
        };

        var balls = layout.Split(',');

        for (var i = 0; i < balls.Length; i++)
        {
            puzzle.Data.Layout[i] = int.Parse(balls[i]);
        }

        return new Board(puzzle);
    }
}