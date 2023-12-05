using System.Collections.Generic;

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
        foreach (string mapStr in maps)
        {
            Dictionary<(long, long), (long, long)> soilMap = makeMap(lines, mapStr);
            seeds = seeds.Select(seed => useMap(soilMap, seed)).ToArray();
            Console.WriteLine(String.Join(" ", seeds));
        }
        Console.WriteLine("Min: " + seeds.Min());
    }

    private static Dictionary<(long, long), (long, long)> makeMap(string[] lines, string mapStr)
    {
        long i = 0;
        while (!lines[i].Contains(mapStr))
        {
            i++;
        }
        i++;
        Dictionary<(long, long), (long, long)> map = new Dictionary<(long, long), (long, long)>();
        while (i < lines.Length && !String.IsNullOrEmpty(lines[i]))
        {
            long[] parts = lines[i].Trim().Split(' ').Select(s => Int64.Parse(s)).ToArray();
            map.Add((parts[1], parts[1] + parts[2]), (parts[0], parts[0] + parts[2]));
            i++;
        }
        return map;
    }

    private static long useMap(Dictionary<(long, long), (long, long)> map, long seed)
    {
        foreach ((long, long) r in map.Keys)
        {
            if (r.Item1 <= seed && seed < r.Item2)
                return map[r].Item1 + seed - r.Item1;
        }
        return seed;
    }
}