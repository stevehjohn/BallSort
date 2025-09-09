namespace BallSort.Engine.Models;

public readonly record struct Move
{
    public int Source { get; }

    public int Target { get; }

    public Move(int source, int target)
    {
        Source = source;
        
        Target = target;
    }
}