using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day4_input.txt");
        int[] numCopies = new int[lines.Length];
        Array.Fill(numCopies, 1);
        for (int i = 0; i < lines.Length; i++)
        {
            int score = scoreLine(lines[i]);
            Console.WriteLine(score);
            for (int j = 1; j <= score; j++)
            {
                numCopies[i + j] += numCopies[i];
            }
        }
        Console.WriteLine("SUM: " + numCopies.Sum());
    }

    private static int scoreLine(string line)
    {
        string[] halfs = line.Split(':')[1].Split('|');
        List<int> winningList = Regex.Split(halfs[0].Trim(), @"\s+").Select(s => Int32.Parse(s)).ToList();
        List<int> playersList = Regex.Split(halfs[1].Trim(), @"\s+").Select(s => Int32.Parse(s)).ToList();
        return winningList.Count(i => playersList.Contains(i));
        //return power == 0 ? 0 : (int)Math.Round(Math.Pow(2, power - 1));
    }
}