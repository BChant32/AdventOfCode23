using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Grid grid = Grid.GetGrid(File.ReadAllLines("day3_input.txt"));
        Console.WriteLine(grid);
        Console.WriteLine(grid.GetCode());
    }

    public class Grid
    {
        public int[,] digits;
        public Grid(int x, int y)
        {
            digits = new int[x, y];
        }

        public int GetCode()
        {
            int sum = 0;
            for (int i = 0; i < digits.GetLength(0); i++)
            {
                for (int j = 0; j < digits.GetLength(1); j++)
                {
                    if (digits[i, j] == -2)
                    {
                        List<int> neighbours = FindNeighbours(i, j);
                        if (neighbours.Count == 2)
                            sum += neighbours[0] * neighbours[1];
                    }
                }
            }
            return sum;
        }

        private static (int, int)[] neighbourPositions = new[] {
            (-1, 1),
            (-1, 0),
            (-1, -1),
            (0, 1),
            (0, -1),
            (1, 1),
            (1, 0),
            (1, -1),
        };
        public List<int> FindNeighbours(int i, int j)
        {
            List<int> neighbours = new List<int>();
            foreach ((int iOffset, int jOffset) in neighbourPositions)
            {
                int iPos = i + iOffset;
                int jPos = j + jOffset;
                if (iPos >= 0
                    && jPos >= 0
                    && iPos < digits.GetLength(0)
                    && jPos < digits.GetLength(1)
                    && digits[iPos, jPos] >= 0)
                    neighbours.Add(ConsumeNumberAt(iPos, jPos));
            }
            return neighbours;
        }
        public int ConsumeNumberAt(int i, int j)
        {
            int number = 0;
            int power = 0;
            while (j + 1 < digits.GetLength(1) && digits[i, j + 1] >= 0)
            {
                j++;
            }

            while (j >= 0 && digits[i, j] >= 0)
            {
                number += (int)Math.Round(Math.Pow(10, power)) * digits[i, j];
                power++;
                digits[i, j] = -1;
                j--;
            }
            return number;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < digits.GetLength(0); i++)
            {
                for (int j = 0; j < digits.GetLength(1); j++)
                {
                    if (digits[i, j] >= 0)
                        sb.Append(digits[i, j]);
                    else if (digits[i, j] == -1)
                        sb.Append('_');
                    else if (digits[i, j] == -2)
                        sb.Append('*');
                    else
                        sb.Append('X');
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public static Grid GetGrid(string[] lines)
        {
            Grid grid = new Grid(lines.Length, lines[0].Length);
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    string c = lines[i][j].ToString();
                    if (Int32.TryParse(c, out int digit))
                        grid.digits[i, j] = digit;
                    else if (c == ".")
                        grid.digits[i, j] = -1;
                    else if (c == "*")
                        grid.digits[i, j] = -2;
                    else
                        grid.digits[i, j] = -3;
                }
            }
            return grid;
        }
    }
}