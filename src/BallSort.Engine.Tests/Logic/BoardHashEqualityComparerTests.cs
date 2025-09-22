using BallSort.Engine.Logic;
using Xunit;

namespace BallSort.Engine.Tests.Logic;

public class BoardHashEqualityComparerTests
{
    [Fact]
    public void EqualsReturnsFalseIfLengthsAreDifferent()
    {
        var left = new UInt128[2];

        var right = new UInt128[3];
        
        Assert.False(new BoardHashEqualityComparer().Equals(left, right));
    }
    
    [Fact]
    public void EqualsReturnsFalseIfHashesAreDifferent()
    {
        var left = new UInt128[2];

        var right = new UInt128[2];

        right[0] = 1;
        
        Assert.False(new BoardHashEqualityComparer().Equals(left, right));
    }
    
    [Fact]
    public void EqualsReturnsFalseIfOneIsNull()
    {
        var left = new UInt128[2];
        
        Assert.False(new BoardHashEqualityComparer().Equals(left, null));
    }
}