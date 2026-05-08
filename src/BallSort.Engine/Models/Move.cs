using BallSort.Engine.Game;

namespace BallSort.Engine.Models;

public readonly record struct Move
{
    public static readonly Move NullMove = new(-1, -1, Colour.Empty);

    public int Source { get; }

    public int Target { get; }
    
    public Colour Colour { get; }
    
    public int Id { get; }

    public Move(int source, int target)
    {
        Source = source;
        
        Target = target;
    }

    public Move(int source, int target, Colour colour)
    {
        Source = source;
        
        Target = target;
        
        Colour = colour;
    }

    public Move(int source, int target, Colour colour, int id)
    {
        Source = source;
        
        Target = target;

        Colour = colour;
        
        Id = id;
    }
}