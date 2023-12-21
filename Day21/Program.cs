using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        // Setup
        string[] lines = File.ReadAllLines("day21_input.txt");
        int height = lines.Length;
        int width = lines[0].Length;
        int[,] grid = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = lines[i][j] switch
                {
                    '#' => -1,
                    '.' => 0,
                    'S' => 1,
                };
            }
        }

        // Steps
        for (int k = 0; k < 64; k++)
        {
            int[,] nextStepGrid = freshGrid(grid, height, width);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (grid[i, j] == 1)
                    {
                        tryStep(nextStepGrid, height, width, i-1, j);
                        tryStep(nextStepGrid, height, width, i+1, j);
                        tryStep(nextStepGrid, height, width, i, j-1);
                        tryStep(nextStepGrid, height, width, i, j+1);
                    }
                }
            }
            grid = nextStepGrid;
        }

        // Write out
        long count = 0;
        for (int i = 0; i < height; i++)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < width; j++)
            {
                if (grid[i, j] == 1) count++;
                sb.Append(grid[i, j] switch
                {
                    -1 => "#",
                    0 => ".",
                    1 => "O",
                });
            }
            Console.WriteLine(sb.ToString());
        }
        Console.WriteLine(count);
    }

    private static void tryStep(int[,] grid, int height, int width, int i, int j)
    {
        if (i < 0 || j < 0 || i >= height || j >= width) return;
        else if (grid[i, j] == 0) grid[i, j] = 1;
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