namespace BallSort.Engine.Models;

public readonly record struct Move
{
    public static Move NullMove = new Move(-1, -1);
    
    public int Source { get; }

    public int Target { get; }

    public Move(int source, int target)
    {
        Source = source;
        
        Target = target;
    }
}