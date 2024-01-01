using static Program;

internal class Program
{
    private static int[,] weightingGrid;
    private static int[,] evalGrid;

    public enum Direction
    {
        None, Up, Down, Left, Right,
    }
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day17_test.txt");
        int height = lines.Length, width = lines[0].Length;
        weightingGrid = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                weightingGrid[i, j] = Int32.Parse(lines[i][j].ToString());
            }
        }

        (Direction, int, int)[] directions = [
            (Direction.Up, -1, 0),
            (Direction.Down, 1, 0),
            (Direction.Left, 0, -1),
            (Direction.Right, 0, 1),
        ];
        Dictionary<(int, int), (int, bool, Direction)> evaled = new()
        {
            { (0, 0), (0, false, Direction.None) }
        };
        for (int k = 0; k < width * height; k++)
        {
            int lowestUndecided = evaled.Values.Where(p => !p.Item2).OrderBy(p => p.Item1).First().Item1;
            KeyValuePair<(int, int), (int, bool, Direction)>[] toBeEvaled = evaled.Where(kvp => !kvp.Value.Item2 && lowestUndecided == kvp.Value.Item1).ToArray();
            if (toBeEvaled.Any(kvp => kvp.Key == (height - 1, width - 1))) break;
            foreach ((Direction dir, int iDir, int jDir) in directions)
            {
                if (dir == toBeEvaled[0].Value.Item3) continue;
                int evalPos = lowestUndecided;
                for (int n = 1; n <= 3; n++)
                {
                    int iPos = toBeEvaled[0].Key.Item1 + n * iDir;
                    int jPos = toBeEvaled[0].Key.Item2 + n * jDir;
                    if (iPos < 0 || jPos < 0 || iPos >= height || jPos >= width) continue;
                    evalPos += weightingGrid[iPos, jPos];
                    if (evaled.ContainsKey((iPos, jPos)))
                    {
                        if (evaled[(iPos, jPos)].Item2) continue;
                        else if (evaled[(iPos, jPos)].Item1 < evalPos) continue;
                        else if (evaled[(iPos, jPos)].Item1 == evalPos)
                            evaled[(iPos, jPos)] = (evalPos, false, Direction.None);
                        else
                            evaled[(iPos, jPos)] = (evalPos, false, dir);
                    }
                    else
                        evaled.Add((iPos, jPos), (evalPos, false, dir));
                }
            }
            evaled[toBeEvaled[0].Key] = (lowestUndecided, true, toBeEvaled[0].Value.Item3);
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (evaled.ContainsKey((i, j)))
                    Console.Write(evaled[(i, j)].Item1.ToString().PadLeft(3) + '|');
                else Console.Write("   |");
            }
            Console.WriteLine();
        }

        Console.WriteLine(evaled[(height - 1, width - 1)]);
    }
}