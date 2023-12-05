internal class Program
{
    private static string[] maps = [
        "seed-to-soil",
        "soil-to-fertilizer",
        "fertilizer-to-water",
        "water-to-light",
        "light-to-temperature",
        "temperature-to-humidity",
        "humidity-to-location",
    ];
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day5_input.txt");
        long[] seeds = lines[0].Split(':')[1].Trim().Split(' ').Select(s => Int64.Parse(s)).ToArray();
        List<(long, long)> seedRanges = new List<(long, long)> ();
        for (int i = 0; i < seeds.Length; i = i+2)
        {
            seedRanges.Add((seeds[i], seeds[i] + seeds[i + 1]));
        }

        foreach (string mapStr in maps)
        {
            Dictionary<(long, long), long> soilMap = makeMap(lines, mapStr);
            seedRanges = weldList(seedRanges.SelectMany(seed => useMap(soilMap, seed)).ToList());
            Console.WriteLine(String.Join(" ", seedRanges));
        }
        //Console.WriteLine("Min: " + seeds.Min());
    }

    private static Dictionary<(long, long), long> makeMap(string[] lines, string mapStr)
    {
        long i = 0;
        while (!lines[i].Contains(mapStr))
        {
            i++;
        }
        i++;
        Dictionary<(long, long), long> map = new Dictionary<(long, long), long>();
        while (i < lines.Length && !String.IsNullOrEmpty(lines[i]))
        {
            long[] parts = lines[i].Trim().Split(' ').Select(s => Int64.Parse(s)).ToArray();
            map.Add((parts[1], parts[1] + parts[2]), parts[0] - parts[1]);
            i++;
        }
        return map;
    }

    private static List<(long, long)> useMap(Dictionary<(long, long), long> map, (long, long) seedRange)
    {
        List<(long, long)> output = new List<(long, long)>();
        List<(long, long)> usedRanges = new List<(long, long)>();
        foreach ((long, long) r in map.Keys)
        {
            if (seedRange.Item1 >= r.Item2 || seedRange.Item2 <= r.Item1) continue;
            (long, long) usedRange = (seedRange.Item1 < r.Item1 ? r.Item1: seedRange.Item1,
                seedRange.Item2 > r.Item2 ? r.Item2: seedRange.Item2);
            usedRanges.Add(usedRange);
            long offset = map[r];
            output.Add((usedRange.Item1 + offset, usedRange.Item2 + offset));
        }
        usedRanges = usedRanges.OrderBy(x => x.Item1).ToList();
        long needle = seedRange.Item1;
        foreach ((long, long) r in usedRanges)
        {
            if (r.Item1 != needle)
            {
                output.Add((needle, r.Item1));
            }
            needle = r.Item2;
        }
        if (needle != seedRange.Item2)
            output.Add((needle, seedRange.Item2));
        return weldList(output);
    }

    private static List<(long, long)> weldList(List<(long, long)> l)
    {
        l = l.OrderBy(x => x.Item1).ToList();
        int i = 0;
        while (i < l.Count - 1)
        {
            if (l[i].Item2 == l[i + 1].Item1)
            {
                l[i] = (l[i].Item1, l[i+1].Item2);
                l.RemoveAt(i + 1);
            }
            i++;
        }
        return l;
    }
}