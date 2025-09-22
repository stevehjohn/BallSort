using BallSort.Engine.Logic;
using Xunit;

namespace BallSort.Engine.Tests.Logic;

public class BoardHashEqualityComparerTests
{
    [Fact]
    public void EqualsReturnsFalseIfLengthsAreDifferent()
    {
        var left = new ulong[2];

        var right = new ulong[3];
        
        Assert.False(new BoardHashEqualityComparer().Equals(left, right));
    }
    
    [Fact]
    public void EqualsReturnsFalseIfHashesAreDifferent()
    {
        var left = new ulong[2];

        var right = new ulong[2];

        right[0] = 1;
        
        Assert.False(new BoardHashEqualityComparer().Equals(left, right));
    }
    
    [Fact]
    public void EqualsReturnsFalseIfOneIsNull()
    {
        var left = new ulong[2];
        
        Assert.False(new BoardHashEqualityComparer().Equals(left, null));
    }
}