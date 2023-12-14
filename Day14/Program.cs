using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        string text = File.ReadAllText("day14_input.txt");
        text = Transpose(text.Trim());
        long sum = 0;
        foreach (string line in text.Trim().Split("\r\n"))
        {
            int slider = line.Length;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'O')
                {
                    sum += slider;
                    Console.WriteLine(slider);
                    slider--;
                }
                else if (line[i] == '#')
                    slider = line.Length - i - 1;
            }
        }
        Console.WriteLine("SUM " + sum);
    }

    private static void Calculate()
    {

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