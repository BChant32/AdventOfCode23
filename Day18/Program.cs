internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day18_input.txt");
        int x1 = 0, y1 = 0;
        int x2 = 0, y2 = 0;
        decimal area = 0;
        int perimeter = 0;
        foreach (string line in lines)
        {
            string[] strs = line.Trim().Split(' ');
            int len = Int32.Parse(strs[1]);
            perimeter += len;
            switch (strs[0])
            {
                case "U":
                    {
                        x2 = x1;
                        y2 = y1 - len;
                        break;
                    }
                case "D":
                    {
                        x2 = x1;
                        y2 = y1 + len;
                        break;
                    }
                case "L":
                    {
                        x2 = x1 - len;
                        y2 = y1;
                        break;
                    }
                case "R":
                    {
                        x2 = x1 + len;
                        y2 = y1;
                        break;
                    }
                default:
                    throw new Exception("");
            }
            decimal triangle = (x1 * y2 - x2 * y1) / 2.0m;
            Console.WriteLine(triangle);
            area += triangle;
            x1 = x2;
            y1 = y2;
        }
        Console.WriteLine(area + " " + perimeter + " " + (area + perimeter/2 + 1));
    }
}