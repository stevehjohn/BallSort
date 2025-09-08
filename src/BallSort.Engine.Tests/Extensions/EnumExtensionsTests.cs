using BallSort.Engine.Models;
using Xunit;

namespace BallSort.Engine.Tests.Extensions;

public class EnumExtensionsTests
{
    [Theory]
    [InlineData(Colour.Blue, "Blue")]
    [InlineData(Colour.DarkGreen, "Dark Green")]
    [InlineData(Colour.DarkPurple, "Dark Purple")]
    public void ToHumanReadableFormatsTextCorrectly(Colour colour, string expected)
    {
        var result = colour.ToHumanReadable();
        
        Assert.Equal(expected, result);
    }
}