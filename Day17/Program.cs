internal class Program
{
    private static int[,] grid;
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day17_input.txt");
        int height = lines.Length, width = lines[0].Length;
        grid = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i,j] = Int32.Parse(lines[i][j].ToString());
            }
        }

        List<Path> paths = new() { new Path(new(),Direction.Left,0,0,0,0) };
        void writeState()
        {
            Console.Clear();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    IOrderedEnumerable<Path> matchingPaths = paths.Where(p => p.evaluated && p.iCur == i && p.jCur == j).Order();
                    if (matchingPaths.Any())
                        Console.Write(matchingPaths.First().cost.ToString().PadLeft(3, ' ') + "|");
                    else Console.Write("   |");
                }
                Console.WriteLine();
            }
        }

        int counter = 0;
        while (paths.Any(p => !p.evaluated))
        {
            counter++;
            paths.Sort();
            Path lowestPath = paths.First(p => !p.evaluated);

            lowestPath.evaluated = true;
            List<Direction> allowedDirections = new() { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
            if (lowestPath.steps.Count > 0)
            {
                allowedDirections.Remove(lowestPath.lastDir switch
                {
                    Direction.Left => Direction.Right,
                    Direction.Right => Direction.Left,
                    Direction.Up => Direction.Down,
                    Direction.Down => Direction.Up,
                });
                if (lowestPath.numInSameDir == 3)
                    allowedDirections.Remove(lowestPath.lastDir);
            }
            if (lowestPath.iCur == 0)
                allowedDirections.Remove(Direction.Up);
            if (lowestPath.iCur == height - 1)
                allowedDirections.Remove(Direction.Down);
            if (lowestPath.jCur == 0)
                allowedDirections.Remove(Direction.Left);
            if (lowestPath.jCur == width - 1)
                allowedDirections.Remove(Direction.Right);

            foreach (Direction dir in allowedDirections)
            {
                (int iNext, int jNext) = Move(dir, lowestPath.iCur, lowestPath.jCur);

                int newCost = lowestPath.cost + grid[iNext, jNext];
                paths.Add(new Path(lowestPath.steps.Append(dir).ToList(), dir, lowestPath.lastDir == dir ? lowestPath.numInSameDir + 1 : 1, newCost, iNext, jNext));

                int numSteps = 4;
                foreach (Path p in paths.Where(p => p.iCur == iNext && p.jCur == jNext).Order())
                {
                    if (p.lastDir != dir) continue;
                    if (p.numInSameDir < numSteps)
                        numSteps = p.numInSameDir;
                    else
                        paths.Remove(p);
                }
            }

            //if (counter % 1000 == 0) writeState();
        }
        writeState();
    }

    public static (int, int) Move(Direction dir, int i, int j)
    {
        return dir switch
        {
            Direction.Left => (i, j - 1),
            Direction.Right => (i, j + 1),
            Direction.Up => (i - 1, j),
            Direction.Down => (i + 1, j),
        };
    }
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}
public class Path : IComparable<Path>
{
    public bool evaluated = false;
    public List<Direction> steps = new();
    public Direction lastDir;
    public int numInSameDir = 0;
    public int cost = 0;
    public int iCur = 0;
    public int jCur = 0;

    public Path(List<Direction> steps, Direction lastDir, int numInSameDir, int cost, int iCur, int jCur)
    {
        this.steps = steps;
        this.lastDir = lastDir;
        this.numInSameDir = numInSameDir;
        this.cost = cost;
        this.iCur = iCur;
        this.jCur = jCur;
    }

    public int CompareTo(Path? other)
    {
        int comp = cost.CompareTo(other.cost);
        if (comp != 0) return comp;
        return numInSameDir.CompareTo(other.numInSameDir);
    }

    public override string ToString()
    {
        return $"({iCur},{jCur}) c{cost} n{numInSameDir}";
    }
}