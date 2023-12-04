using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day4_input.txt");
        int sum = 0;
        foreach (string line in lines)
        {
            int score = scoreLine(line);
            Console.WriteLine(score);
            sum += score;
        }
        Console.WriteLine("SUM: " + sum);
    }

    private static int scoreLine(string line)
    {
        string[] halfs = line.Split(':')[1].Split('|');
        List<int> winningList = Regex.Split(halfs[0].Trim(), @"\s+").Select(s => Int32.Parse(s)).ToList();
        List<int> playersList = Regex.Split(halfs[1].Trim(), @"\s+").Select(s => Int32.Parse(s)).ToList();
        int power = winningList.Count(i => playersList.Contains(i));
        return power == 0 ? 0 : (int)Math.Round(Math.Pow(2, power - 1));
    }
}