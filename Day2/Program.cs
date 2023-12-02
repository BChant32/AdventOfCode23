using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day2_input.txt");
        int sum = 0;
        foreach (string line in lines)
        {
            Game lineGame = Game.ReadGame(line);
            //bool isPossible = lineGame.PossibleWith(12, 13, 14);
            Console.WriteLine($"Game {lineGame.ID}:{String.Join(';',lineGame.RGBs)}");
            //if (isPossible) sum += lineGame.ID;
            (int, int, int) minset = lineGame.MinSet;
            sum += minset.Item1 * minset.Item2 * minset.Item3;
            if (sum > 1e9) throw new Exception("Getting too large for Int32");
        }
        Console.WriteLine("FINAL SUM: " + sum);
    }
}

public class Game
{
    public int ID { get; set; }
    public List<(int, int, int)> RGBs { get; set; }
    public (int, int, int) MinSet
    {
        get => (RGBs.Select(rgb => rgb.Item1).Max(),
            RGBs.Select(rgb => rgb.Item2).Max(),
            RGBs.Select(rgb => rgb.Item3).Max());
    }

    public Game(int id)
    {
        ID = id;
        RGBs = new List<(int, int, int)>();
    }

    public bool PossibleWith(int r, int g, int b)
    {
        return RGBs.Select(rgb => rgb.Item1).Max() <= r
            && RGBs.Select(rgb => rgb.Item2).Max() <= g
            && RGBs.Select(rgb => rgb.Item3).Max() <= b;
    }

    private static Regex regexR = new Regex(@"\b\d+(?=\sred)", RegexOptions.IgnoreCase);
    private static Regex regexG = new Regex(@"\b\d+(?=\sgreen)", RegexOptions.IgnoreCase);
    private static Regex regexB = new Regex(@"\b\d+(?=\sblue)", RegexOptions.IgnoreCase);
    public static Game ReadGame(string line)
    {
        string[] colonSplit = line.Split(':');
        Match m = Regex.Match(colonSplit[0], @"(?<=^Game\s)\d+$");
        if (!m.Success) throw new Exception("Failed to read line " + line);
        Game newGame = new Game(Int32.Parse(m.Value));
        
        foreach (string trial in colonSplit[1].Split(';'))
        {
            (int, int, int) rgb = (0, 0, 0);
            m = regexR.Match(trial);
            if (m.Success) rgb.Item1 = Int32.Parse(m.Value);
            m = regexG.Match(trial);
            if (m.Success) rgb.Item2 = Int32.Parse(m.Value);
            m = regexB.Match(trial);
            if (m.Success) rgb.Item3 = Int32.Parse(m.Value);
            newGame.RGBs.Add(rgb);
        }
        return newGame;
    }
}