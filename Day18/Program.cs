using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day18_input.txt");
        int x1 = 0, y1 = 0;
        int x2 = 0, y2 = 0;
        double area = 0;
        long perimeter = 0;
        foreach (string line in lines)
        {
            Match m = Regex.Match(line, @"\(#([a-f\d]{5})([a-f\d])\)");
            int len = Convert.ToInt32(m.Groups[1].Value, 16);
            perimeter += len;
            switch (m.Groups[2].Value)
            {
                case "3":
                    {
                        x2 = x1;
                        y2 = y1 - len;
                        break;
                    }
                case "1":
                    {
                        x2 = x1;
                        y2 = y1 + len;
                        break;
                    }
                case "2":
                    {
                        x2 = x1 - len;
                        y2 = y1;
                        break;
                    }
                case "0":
                    {
                        x2 = x1 + len;
                        y2 = y1;
                        break;
                    }
                default:
                    throw new Exception("");
            }
            double triangle = (x1 * (double)y2 - x2 * (double)y1) / 2.0;
            Console.WriteLine(triangle);
            area += triangle;
            x1 = x2;
            y1 = y2;
        }
        Console.WriteLine(area + " " + perimeter + " " + (Math.Abs(area) + perimeter/2 + 1));
    }
}