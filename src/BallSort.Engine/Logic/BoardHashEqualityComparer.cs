namespace BallSort.Engine.Logic;

public class BoardHashEqualityComparer : IEqualityComparer<UInt128[]>
{
    public bool Equals(UInt128[] x, UInt128[] y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        for (var i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(UInt128[] source)
    {
        unchecked
        {
            var hash = 17;

            foreach (var b in source)
            {
                hash = hash * 31 + (int) b;
                hash = hash * 31 + (int) (b >> 32);
                hash = hash * 31 + (int) (b >> 64);
                hash = hash * 31 + (int) (b >> 96);
            }

            return hash;
        }
    }
}