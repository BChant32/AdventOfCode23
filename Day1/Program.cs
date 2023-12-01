using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day1_input.txt");
        long sum = 0;
        foreach (string line in lines)
        {
            sum += readLine(line);
        }
        Console.WriteLine(sum);
    }

    private static int readLine(string line)
    {
        line = line.Trim();
        Regex r = new Regex(@"\d");
        if (!r.IsMatch(line)) throw new Exception("Invalid line " + line);
        MatchCollection mc = r.Matches(line);
        return Int32.Parse(mc.First().Value + mc.Last().Value);
    }
}