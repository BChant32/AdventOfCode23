internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day22_input.txt");
        List<Cuboid> cuboids = lines.Select(l => new Cuboid(l)).ToList();
        cuboids.Sort();
        Cuboid.SettleFalling(cuboids);

        bool[] necessaryCuboids = new bool[cuboids.Count];
        for (int i = 1; i < cuboids.Count; i++)
        {
            Cuboid cuboid = cuboids[i];
            List<int> supports = new();
            for (int j = 0; j < i; j++)
            {
                if (cuboids[j].Supports(cuboid))
                    supports.Add(j);
            }
            if (supports.Count == 1)
                necessaryCuboids[supports[0]] = true;
        }
        Console.WriteLine(necessaryCuboids.Count(b => !b));
    }
}

public class Cuboid : IComparable<Cuboid>
{
    public (int X, int Y, int Z) Min;
    public (int X, int Y, int Z) Max;

    public Cuboid(string line)
    {
        string[] halves = line.Trim().Split('~');
        string[] left = halves[0].Split(',');
        string[] right = halves[1].Split(',');
        int x1, x2, y1, y2, z1, z2;
        x1 = Int32.Parse(left[0]);
        y1 = Int32.Parse(left[1]);
        z1 = Int32.Parse(left[2]);
        x2 = Int32.Parse(right[0]);
        y2 = Int32.Parse(right[1]);
        z2 = Int32.Parse(right[2]);
        if (x1 > x2) (x1, x2) = (x2, x1);
        if (y1 > y2) (y1, y2) = (y2, y1);
        if (z1 > z2) (z1, z2) = (z2, z1);
        Min = (x1, y1, z1);
        Max = (x2, y2, z2);
    }

    public void Fall()
    {
        Min.Z--;
        Max.Z--;
    }

    public bool Supports(Cuboid other)
    {
        return Max.Z == other.Min.Z - 1
            && !(Min.X > other.Max.X || other.Min.X > Max.X
                || Min.Y > other.Max.Y || other.Min.Y > Max.Y);
    }

    public int CompareTo(Cuboid? other)
    {
        return Min.Z.CompareTo(other.Min.Z);
    }

    public override string ToString()
    {
        return $"({Min.X},{Min.Y},{Min.Z})~({Max.X},{Max.Y},{Max.Z})";
    }

    public static void SettleFalling(List<Cuboid> cuboids)
    {
        bool anyDoFall = true;
        while (anyDoFall)
        {
            anyDoFall = false;
            for (int i = 0; i < cuboids.Count; i++)
            {
                Cuboid cuboid = cuboids[i];
                if (cuboid.Min.Z == 1) continue;
                bool isSupported = false;
                for (int j = 0; j < i; j++)
                {
                    if (cuboids[j].Supports(cuboid))
                    {
                        isSupported = true;
                        break;
                    }
                }
                if (!isSupported)
                {
                    anyDoFall = true;
                    cuboid.Fall();
                }
            }
        }
    }
}