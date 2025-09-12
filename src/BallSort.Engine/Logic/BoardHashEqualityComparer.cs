namespace BallSort.Engine.Logic;

public class BoardHashEqualityComparer : IEqualityComparer<byte[]>
{
    public bool Equals(byte[] x, byte[] y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(byte[] source)
    {
        unchecked
        {
            var hash = 17;

            foreach (var b in source)
            {
                hash = hash * 31 + b;
            }

            return hash;
        }
    }
}