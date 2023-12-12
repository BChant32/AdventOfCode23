internal class Program
{
    private static void Main(string[] args)
    {
        List<(int, int)> galaxies = new List<(int, int)>();
        string[] lines = File.ReadAllLines("day11_input.txt");
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '#')
                    galaxies.Add((i, j));
            }
        }

        for (int i = lines.Length - 1; i > 0; i--)
        {
            if (!galaxies.Any(g => g.Item1 == i))
            {
                for (int k = 0; k < galaxies.Count; k++)
                {
                    if (galaxies[k].Item1 > i)
                        galaxies[k] = (galaxies[k].Item1 + 999999, galaxies[k].Item2);
                }
            }
        }
        for (int i = lines[0].Length - 1; i > 0; i--)
        {
            if (!galaxies.Any(g => g.Item2 == i))
            {
                for (int k = 0; k < galaxies.Count; k++)
                {
                    if (galaxies[k].Item2 > i)
                        galaxies[k] = (galaxies[k].Item1, galaxies[k].Item2 + 999999);
                }
            }
        }

        long sum = 0;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                sum += Math.Abs(galaxies[i].Item1 - galaxies[j].Item1) + Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);
            }
        }
        Console.WriteLine(sum);
    }
}