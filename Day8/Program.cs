using System.Text.RegularExpressions;

internal class Program
{
    private static Regex r = new Regex(@"([A-z]{3})\s=\s\(([A-z]{3}),\s([A-z]{3})\)", RegexOptions.IgnoreCase);
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day8_input.txt");
        string steps = lines[0].Trim();
        int numSteps = steps.Length;
        Dictionary<string, (string, string)> map = new();
        foreach (string line in lines.Skip(2))
        {
            Match match = r.Match(line);
            if (!match.Success) throw new Exception("Unable to parse line: " +  line);
            map.Add(match.Groups[1].Value, (match.Groups[2].Value, match.Groups[3].Value));
        }
        int i = 0;
        string location = "AAA";
        while (location != "ZZZ")
        {
            (string, string) lr = map[location];
            location = (steps[i%numSteps] == 'L') ? lr.Item1 : lr.Item2;
            i++;
        }
        Console.WriteLine(i);
    }
}