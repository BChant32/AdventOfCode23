using System.Text.RegularExpressions;

internal class Program
{
    private static Regex r = new Regex(@"([A-z\d]{3})\s=\s\(([A-z\d]{3}),\s([A-z\d]{3})\)", RegexOptions.IgnoreCase);
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day8_input.txt");
        string steps = lines[0].Trim();
        Dictionary<string, (string, string)> map = new();
        foreach (string line in lines.Skip(2))
        {
            Match match = r.Match(line);
            if (!match.Success) throw new Exception("Unable to parse line: " + line);
            map.Add(match.Groups[1].Value, (match.Groups[2].Value, match.Groups[3].Value));
        }
        foreach (string loc in map.Keys.Where(s => s.EndsWith("A")))
        {
            p(map, steps, loc);
        }
    }

    private static void p(Dictionary<string, (string, string)> map, string steps, string location)
    {
        int numSteps = steps.Length;
        int i = 0;
        List<(string, int)> visited = new() { (location, 0) };
        while (true)
        {
            (string, string) lr = map[location];
            location = (steps[i % numSteps] == 'L') ? lr.Item1 : lr.Item2;
            i++;
            (string, int) search = visited.FirstOrDefault(pair => pair.Item1 == location && pair.Item2 % numSteps == i % numSteps);
            if (search.Item1 is not default(string))
            {
                Console.WriteLine(String.Join(",",visited.Where(p => p.Item1.EndsWith("Z")).Select(p => p.Item2)));
                Console.WriteLine(location + " " +search.Item2 + " " + i);
                break;
            }
            else visited.Add((location, i));
        }
        //IEnumerable<IGrouping<string, (string, int)>> groups = visited.GroupBy(p => p.Item1);
        //Console.WriteLine(String.Join(",", groups.Select(g => g.Key)));
        //Console.WriteLine("Number in group: " + groups.Count());
    }
}

//string[] locations = map.Keys.Where(s => s.EndsWith("A")).ToArray();
//Console.WriteLine("#locations: " + locations.Length);
//while (locations.Any(s => !s.EndsWith("Z")))
//{
//    for (int j = 0; j < locations.Length; j++)
//    {
//        locations[j] = (steps[i % numSteps] == 'L') ? map[locations[j]].Item1 : map[locations[j]].Item2;
//    }
//    i++;
//    int numMatches = locations.Count(s => s.EndsWith("Z"));
//    if (numMatches > 2) Console.WriteLine($"{i} {numMatches}");
//}

// ANS 8,245,452,805,243