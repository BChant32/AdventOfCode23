using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day19_input.txt");
        bool isHeader = true;

        List<Workflow> workflows = new();
        List<Xmas> xmasList = new();
        foreach (string line in lines)
        {
            if (String.IsNullOrEmpty(line))
            {
                isHeader = false;
                continue;
            }

            if (isHeader)
                workflows.Add(Workflow.CreateFrom(line));
            else
                xmasList.Add(Xmas.CreateFrom(line));
        }

        long sum = 0;
        foreach (Xmas xmas in xmasList)
        {
            bool accepted = Workflow.Process(workflows, xmas);
            if (accepted)
                sum += xmas.x + xmas.m + xmas.a + xmas.s;
        }
        Console.WriteLine(sum);
    }
}

public record Xmas(int x, int m, int a, int s)
{
    public static Xmas CreateFrom(string line)
    {
        Match match = Regex.Match(line, @"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}");
        if (!match.Success) throw new Exception("");
        return new(Int32.Parse(match.Groups[1].Value),
            Int32.Parse(match.Groups[2].Value),
            Int32.Parse(match.Groups[3].Value),
            Int32.Parse(match.Groups[4].Value));
    }

    public int GetField(char c)
    {
        return c switch
        {
            'x' => x,
            'm' => m,
            'a' => a,
            's' => s,
        };
    }
};
public class Workflow
{
    public string Name;
    private List<(Check?, string)> rules = new();

    public static Workflow CreateFrom(string line)
    {
        string[] bracesSplit = line.Trim().Split('{','}');
        Workflow newWorkflow = new(){ Name = bracesSplit[0] };
        foreach (string ruleStr in bracesSplit[1].Split(','))
        {
            if (ruleStr.Contains(':'))
            {
                string[] colonSplit = ruleStr.Split(':');
                newWorkflow.rules.Add((Check.CreateFrom(colonSplit[0]), colonSplit[1]));
            }
            else
                newWorkflow.rules.Add((null, ruleStr));
        }
        return newWorkflow;
    }


    public static bool Process(List<Workflow> workflows, Xmas xmas)
    {
        Workflow curWorkflow = workflows.First(w => w.Name == "in");
        int curRule = 0;
        while (true)
        {
            (Check? check, string outcome) = curWorkflow.rules[curRule];
            if (check is null
                || check.Perform(xmas))
            {
                if (outcome == "A") return true;
                else if (outcome == "R") return false;
                else
                {
                    curWorkflow = workflows.First(w => w.Name == outcome);
                    curRule = 0;
                }
            }
            else curRule++;
        }
    }

    internal class Check
    {
        internal enum OpEnum
        {
            lt, gt,
        }
        internal OpEnum Op;
        internal char Field;
        internal int Threshold;

        internal Check(OpEnum op, char field, int threshold)
        {
            Op = op;
            Field = field;
            Threshold = threshold;
        }

        internal bool Perform(Xmas xmas)
        {
            int field = xmas.GetField(Field);
            return Op switch
            {
                OpEnum.lt => field < Threshold,
                OpEnum.gt => field > Threshold,
                _ => throw new Exception(""),
            };
        }

        internal static Check CreateFrom(string str)
        {
            if (str.Contains('<'))
            {
                string[] halves = str.Trim().Split('<');
                return new(OpEnum.lt, halves[0][0], Int32.Parse(halves[1]));
            }
            else if (str.Contains('>'))
            {
                string[] halves = str.Trim().Split('>');
                return new(OpEnum.gt, halves[0][0], Int32.Parse(halves[1]));
            }
            else
                throw new Exception("");
        }
    }
}
