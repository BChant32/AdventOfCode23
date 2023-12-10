internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day10_input.txt");
        Pipe[,] pipeGrid = new Pipe[lines.Length, lines[0].Length];
        int xStart = 0, yStart = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                pipeGrid[i, j] = Pipe.ReadPipe(lines[i][j]);
                if (lines[i][j] == 'S')
                {
                    xStart = i;
                    yStart = j;
                }
            }
        }
        int stepCount = 1;
        int iStep = xStart - 1, jStep = yStart;
        Direction comeFrom = Direction.D;
        bool[,] visited = new bool[lines.Length, lines[0].Length];
        visited[xStart, yStart] = true;
        lines[xStart] = lines[xStart].Replace('S','|');
        while (!(iStep == xStart && jStep == yStart))
        {
            visited[iStep, jStep] = true;
            Pipe pipeStep = pipeGrid[iStep, jStep];
            if (pipeStep.L && comeFrom != Direction.L)
            {
                comeFrom = Direction.R;
                jStep--;
            }
            else if (pipeStep.R && comeFrom != Direction.R)
            {
                comeFrom = Direction.L;
                jStep++;
            }
            else if (pipeStep.U && comeFrom != Direction.U)
            {
                comeFrom = Direction.D;
                iStep--;
            }
            else if (pipeStep.D && comeFrom != Direction.D)
            {
                comeFrom = Direction.U;
                iStep++;
            }

            stepCount++;
        }
        Console.WriteLine(stepCount / 2.0);

        bool isIn(int i, int j)
        {
            int val = 0;
            char open = '.';
            for (int k = 0; k < j; k++)
            {
                if (!visited[i, k]) continue;

                if (lines[i][k] == '|') val++;
                else if (lines[i][k] == 'L') open = 'L';
                else if (lines[i][k] == 'F') open = 'F';
                else if (lines[i][k] == '7')
                {
                    if (open == 'L') val++;
                    open = '.';
                }
                else if (lines[i][k] == 'J')
                {
                    if (open == 'F') val++;
                    open = '.';
                }
            }
            return val % 2 == 1;
        }

        int sum = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            Console.WriteLine(String.Join("", Enumerable.Range(0, lines[i].Length).Select(j => visited[i, j] ? lines[i][j] : isIn(i,j) ? 'X' : '.')));
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (!visited[i,j] && isIn(i,j))
                    sum++;
            }
        }
        Console.WriteLine(sum);
    }
}

public enum Direction
{
    L,
    R,
    U,
    D,
}

public class Pipe
{
    public bool L;
    public bool R;
    public bool U;
    public bool D;

    public Pipe(bool l, bool r, bool u, bool d)
    {
        L = l;
        R = r;
        U = u;
        D = d;
    }

    public static Pipe ReadPipe(char c)
        => c switch
        {
            '|' => new Pipe(false, false, true, true),
            '-' => new Pipe(true, true, false, false),
            'L' => new Pipe(false, true, true, false),
            'J' => new Pipe(true, false, true, false),
            '7' => new Pipe(true, false, false, true),
            'F' => new Pipe(false, true, false, true),
            '.' => new Pipe(false, false, false, false),
            'S' => new Pipe(true, true, true, true),
        };
}