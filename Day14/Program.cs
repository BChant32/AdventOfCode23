using System.Diagnostics.CodeAnalysis;
using System.Text;

internal class Program
{
    private static Dictionary<int[,], int> cycles = new Dictionary<int[,], int>(new comp());

    protected class comp : IEqualityComparer<int[,]>
    {
        public bool Equals(int[,]? x, int[,]? y)
        {
            return x.Cast<int>().SequenceEqual(y.Cast<int>());
        }

        public int GetHashCode([DisallowNull] int[,] obj)
        {
            return obj[0, 0];
        }
    }

    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day14_input.txt");
        int height = lines.Length;
        int width = lines[0].Length;
        int[,] grid = new int[height, width];
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                grid[i, j] = lines[i][j] switch
                {
                    '.' => -1,
                    'O' => 0,
                    '#' => 1,
                };
            }
        }

        for (int k = 0; k < 1000000000; k++)
        {
            if (!cycles.ContainsKey(grid)) cycles.Add((int[,])grid.Clone(), k);
            else k = 1000000000 - ((1000000000-k) % (k - cycles[grid]));
            grid = CalculateRotation(grid, height, width, (pos) => (pos.Item1 - 1, pos.Item2));
            grid = CalculateRotation(grid, height, width, (pos) => (pos.Item1, pos.Item2 - 1));
            grid = CalculateRevRotation(grid, height, width, (pos) => (pos.Item1 + 1, pos.Item2));
            grid = CalculateRevRotation(grid, height, width, (pos) => (pos.Item1, pos.Item2 + 1));
        }
        Print(grid, height, width);
        long sum = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[i, j] == 0) sum += height - i;
            }
        }
        Console.WriteLine("SUM " + sum);
    }

    private delegate (int, int) move((int, int) pos);
    private static int[,] CalculateRotation(int[,] grid, int height, int width, move move)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[i, j] != 0) continue;

                (int iPos, int jPos) = move((i,j));
                int iOld = i, jOld = j;
                while (iPos >= 0 && iPos < height
                    && jPos >= 0 && jPos < width
                    && grid[iPos, jPos] == -1)
                {
                    grid[iPos, jPos] = 0;
                    grid[iOld, jOld] = -1;
                    iOld = iPos;
                    jOld = jPos;
                    (iPos, jPos) = move((iOld, jOld));
                }
            }
        }
        return grid;
    }
    private static int[,] CalculateRevRotation(int[,] grid, int height, int width, move move)
    {
        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = width - 1; j >= 0; j--)
            {
                if (grid[i, j] != 0) continue;

                (int iPos, int jPos) = move((i,j));
                int iOld = i, jOld = j;
                while (iPos >= 0 && iPos < height
                    && jPos >= 0 && jPos < width
                    && grid[iPos, jPos] == -1)
                {
                    grid[iPos, jPos] = 0;
                    grid[iOld, jOld] = -1;
                    iOld = iPos;
                    jOld = jPos;
                    (iPos, jPos) = move((iOld, jOld));
                }
            }
        }
        return grid;
    }

    private static void Print(int[,] grid, int height, int width)
    {
        for (int i = 0; i < height; i++)
        {
            Console.WriteLine(String.Join("", Enumerable.Range(0, width).Select(j => grid[i, j] == -1 ? "." : grid[i,j].ToString())));
        }
    }
}