using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        // Setup
        string[] lines = File.ReadAllLines("day21_test.txt");
        int height = lines.Length;
        int width = lines[0].Length;
        bool[,] grid = new bool[height, width];
        (int, int) start = (-1, -1);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = lines[i][j] != '#';
                if (lines[i][j] == 'S')
                    start = (i, j);
            }
        }

        // Steps
        HashSet<(int, int)> set = new() { start };
        HashSet<(int, int)> certain = new() { };
        for (int k = 0; k < 2500; k++) //26501365
        {
            HashSet<(int, int)> doubleStepSet = doStep(set, grid, height, width);
            doubleStepSet = doStep(doubleStepSet, grid, height, width);

            set.IntersectWith(doubleStepSet);
            certain.UnionWith(set);
            doubleStepSet.ExceptWith(set);
            set = doubleStepSet;
            if (k % 100 == 0) Console.WriteLine(k);
        }

        // Write out
        set.UnionWith(certain);
        Console.WriteLine(set.Count);
    }

    private static void tryStep(HashSet<(int, int)> set, bool[,] grid, int height, int width, int i, int j)
    {
        int iInd = i % height;
        if (iInd < 0) iInd += height;
        int jInd = j % width;
        if (jInd < 0) jInd += width;
        if (grid[iInd, jInd])
            set.Add((i, j));
    }

    private static HashSet<(int, int)> doStep(HashSet<(int, int)> set, bool[,] grid, int height, int width)
    {
        HashSet<(int, int)> nextStepSet = new();
        foreach ((int i, int j) in set)
        {
            tryStep(nextStepSet, grid, height, width, i - 1, j);
            tryStep(nextStepSet, grid, height, width, i + 1, j);
            tryStep(nextStepSet, grid, height, width, i, j - 1);
            tryStep(nextStepSet, grid, height, width, i, j + 1);
        }
        return nextStepSet;
    }

    private static int[,] freshGrid(int[,] grid, int height, int width)
    {
        int[,] freshGrid = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                freshGrid[i, j] = grid[i,j] switch
                {
                    -1 => -1,
                    0 => 0,
                    1 => 0,
                };
            }
        }
        return freshGrid;
    }
}