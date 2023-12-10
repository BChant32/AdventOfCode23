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
        while (!(iStep == xStart && jStep == yStart))
        {
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