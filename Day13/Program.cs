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
            int reflection = FindValueWithSmudge(patternText);
            Console.WriteLine(reflection);
            sum += reflection;
        }
        Console.WriteLine("Sum " + sum);
    }

    private static int FindValueWithSmudge(string patternText)
    {
        int oldReflection = FindHorizontalReflectionLine(patternText);
        bool oldIsHorizontal = oldReflection > 0;
        if (!oldIsHorizontal) oldReflection = FindHorizontalReflectionLine(Transpose(patternText));

        int reflection = -1;
        string testPattern;
        for (int i = 0; i < patternText.Length; i++)
        {
            if (patternText[i] != '#' && patternText[i] != '.') continue;
            char c = patternText[i] == '#' ? '.' : '#';
            testPattern = patternText.Substring(0,i) + c + patternText.Substring(i+1);
            reflection = FindHorizontalReflectionLine(testPattern, oldIsHorizontal ? oldReflection : 0);
            if (reflection > 0) return reflection * 100;
            reflection = FindHorizontalReflectionLine(Transpose(testPattern), oldIsHorizontal ? 0 : oldReflection);
            if (reflection > 0) return reflection;
        }
        throw new Exception("Unable to find any");
    }
    private static int FindHorizontalReflectionLine(string patternText, int ignore = 0)
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
            if (allLinesMatch && k != ignore)
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