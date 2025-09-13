namespace BallSort.Engine.Models;

public readonly record struct Move
{
    public static Move NullMove = new Move(-1, -1);
    
    public int Source { get; }

    public int Target { get; }
    
    public int Id { get; }

    public Move(int source, int target)
    {
        Source = source;
        
        Target = target;
    }

    public Move(int source, int target, int id)
    {
        Source = source;
        
        Target = target;
        
        Id = id;
    }
}