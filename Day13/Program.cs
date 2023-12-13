using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        string allText = File.ReadAllText("day13_input.txt");
        string[] patternTexts = allText.Trim().Split("\r\n\r\n");
        long sum = 0;
        foreach (string patternText in patternTexts)
        {
            int reflection = FindHorizontalReflectionLine(patternText);
            if (reflection < 0)
            {
                reflection = FindHorizontalReflectionLine(Transpose(patternText));
            }
            else reflection *= 100;
            Console.WriteLine(reflection);
            sum += reflection;
        }
        Console.WriteLine("Sum " + sum);
    }

    private static int FindHorizontalReflectionLine(string patternText)
    {
        string[] patternLines = patternText.Split("\r\n");
        for (int k = 1; k < patternLines.Length; k++)
        {
            bool allLinesMatch = true;
            for (int i = 0; i < Math.Min(k, patternLines.Length - k); i++)
            {
                if (patternLines[k + i] != patternLines[k - i - 1])
                {
                    allLinesMatch = false;
                    break;
                }
            }
            if (allLinesMatch)
                return k;
        }
        return -1;
    }
    private static string Transpose(string text)
    {
        string[] lines = text.Split("\r\n");
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lines[0].Length; i++)
        {
            for (int j = 0; j < lines.Length; j++)
            {
                sb.Append(lines[j][i]);
            }
            sb.AppendLine();
        }
        return sb.ToString().Trim();
    }
}