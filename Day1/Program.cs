using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day1_input.txt");
        int lineNum = 0;
        long sum = 0;
        foreach (string line in lines)
        {
            lineNum++;
            int extracted = readLine(line);
            Console.WriteLine($"{lineNum.ToString().PadLeft(4)}    {extracted}");
            sum += extracted;
        }
        Console.WriteLine(sum);
    }

    private static Dictionary<string, int> words = new Dictionary<string, int>
    {
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
    };

    private static int readLine(string line)
    {
        line = line.Trim();
        string pattern = @"\d";
        foreach (string word in words.Keys)
        {
            pattern += '|' + word;
        }
        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        if (!r.IsMatch(line)) throw new Exception("Invalid line " + line);
        string firstChar = readNum(r.Match(line).Value);

        MatchCollection mc = Regex.Matches(line, $"(?={pattern})", RegexOptions.IgnoreCase);
        line = line.Substring(mc.Last().Index);
        return Int32.Parse(firstChar + readNum(r.Match(line).Value));
    }

    private static string readNum(string wordOrDigit)
    {
        if (Regex.IsMatch(wordOrDigit, @"\d")) return wordOrDigit;
        return words[wordOrDigit].ToString();
    }
}