internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day24_input.txt");
        Hail[] hails = lines.Select(l => new Hail(l)).ToArray();
        int count = 0;
        for (int i = 0; i < hails.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (hails[i].PathsIntersect(hails[j]))
                    count++;
                //Console.WriteLine($"{i},{j}: {hails[i].PathsIntersect(hails[j])}");
            }
        }
        Console.WriteLine(count);
    }
}

public class Hail
{
    public (long X, long Y, long Z) Pos;
    public (long X, long Y, long Z) Vel;

    public Hail(string line)
    {

        string[] halves = line.Trim().Split('@');
        string[] left = halves[0].Split(',');
        string[] right = halves[1].Split(',');
        long x1, x2, y1, y2, z1, z2;
        x1 = Int64.Parse(left[0]);
        y1 = Int64.Parse(left[1]);
        z1 = Int64.Parse(left[2]);
        x2 = Int64.Parse(right[0]);
        y2 = Int64.Parse(right[1]);
        z2 = Int64.Parse(right[2]);
        Pos = (x1, y1, z1);
        Vel = (x2, y2, z2);
    }

    public bool PathsIntersect(Hail other)
    {
        double divisor = (other.Vel.Y * (double)Vel.X - other.Vel.X * (double)Vel.Y);
        if (divisor == 0)
        {
            return false;
            //return Math.Sign(other.Pos.X - Pos.X) == Math.Sign(Vel.X) && Math.Sign(other.Pos.Y - Pos.Y) == Math.Sign(Vel.Y)
            //    || Math.Sign(Pos.X - other.Pos.X) == Math.Sign(other.Vel.X) && Math.Sign(Pos.Y - other.Pos.Y) == Math.Sign(other.Vel.Y);
        }
        double lambda = ((other.Pos.X - (double)Pos.X) * Vel.Y + (Pos.Y - (double)other.Pos.Y) * Vel.X) / divisor;
        (double x, double y) = (other.Pos.X + lambda * other.Vel.X, other.Pos.Y + lambda * other.Vel.Y);
        double otherLambda = ((other.Pos.X - (double)Pos.X) + lambda * other.Vel.X) / Vel.X;
        return lambda >= 0 && otherLambda >= 0
            && 200000000000000 <= x && x <= 400000000000000
            && 200000000000000 < y && y <= 400000000000000;
    }

    public bool Intersects(Hail other)
    {
        if (!timeAt(Pos.X, other.Pos.X, Vel.X, other.Vel.X, out double timeX)) return false;
        if (!timeAt(Pos.Y, other.Pos.Y, Vel.Y, other.Vel.Y, out double timeY)) return false;
        if (!timeAt(Pos.Z, other.Pos.Z, Vel.Z, other.Vel.Z, out double timeZ)) return false;
        return timeX == timeY
            && timeY == timeZ
            && timeX > 0;
    }
    private bool timeAt(long x1, long x2, long v1, long v2, out double time)
    {
        time = 0;
        if (v1 == v2) return false;
        time = ((double)x1 - (double)x2) / ((double)v2 - (double)v1);
        return true;
    }

    public override string ToString()
    {
        return $"({Pos.X},{Pos.Y},{Pos.Z})@({Vel.X},{Vel.Y},{Vel.Z})";
    }
}