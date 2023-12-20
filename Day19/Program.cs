using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day19_input.txt");

        List<Workflow> workflows = new();
        foreach (string line in lines)
        {
            if (String.IsNullOrEmpty(line)) break;

            workflows.Add(Workflow.CreateFrom(line));
        }

        long sum = Workflow.Process(workflows, workflows.First(w => w.Name == "in"), new((1, 4001), (1, 4001), (1, 4001), (1, 4001)));
        Console.WriteLine(sum);
    }
}

public record Xmas((int, int) x, (int, int) m, (int, int) a, (int, int) s)
{
    public (int, int) GetField(char c)
    {
        return c switch
        {
            'x' => x,
            'm' => m,
            'a' => a,
            's' => s,
        };
    }

    public long Evaluate()
        => (long)(x.Item2 - x.Item1)
        * (long)(m.Item2 - m.Item1)
        * (long)(a.Item2 - a.Item1)
        * (long)(s.Item2 - s.Item1);

    public static Xmas CreateFrom(Xmas xmas, char c, (int, int) field)
    {
        return c switch
        {
            'x' => new Xmas(field, xmas.m, xmas.a, xmas.s),
            'm' => new Xmas(xmas.x, field, xmas.a, xmas.s),
            'a' => new Xmas(xmas.x, xmas.m, field, xmas.s),
            's' => new Xmas(xmas.x, xmas.m, xmas.a, field),
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


    public static long Process(List<Workflow> workflows, Workflow workflow, Xmas xmas)
    {
        long ProcessOutcome(Xmas xmasTrue, string outcome)
        {
            if (outcome == "A") return xmasTrue.Evaluate();
            else if (outcome == "R") return 0;
            else return Workflow.Process(workflows, workflows.First(w => w.Name == outcome), xmasTrue);
        }

        long sum = 0;
        foreach ((Check? check, string outcome) in workflow.rules)
        {
            if (check is null)
                sum += ProcessOutcome(xmas, outcome);
            else
            {
                (Xmas? xmasTrue, Xmas? xmasFalse) = check.Perform(xmas);
                if (xmasTrue is not null) sum += ProcessOutcome(xmasTrue, outcome);
                if (xmasFalse is null) return sum;
                xmas = xmasFalse;
            }
        }
        return sum;
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

        internal (Xmas?, Xmas?) Perform(Xmas xmas)
        {
            (int fieldMin, int fieldMax) = xmas.GetField(Field);
            switch (Op)
            {
                case OpEnum.lt:
                    {
                        if (Threshold <= fieldMin) return (null, xmas);
                        else if (Threshold >= fieldMax) return (xmas, null);
                        else return (Xmas.CreateFrom(xmas, Field, (fieldMin, Threshold)), Xmas.CreateFrom(xmas, Field, (Threshold, fieldMax)));
                    }
                case OpEnum.gt:
                    {
                        if (Threshold >= fieldMax - 1) return (null, xmas);
                        else if (Threshold < fieldMin) return (xmas, null);
                        else return (Xmas.CreateFrom(xmas, Field, (Threshold + 1, fieldMax)), Xmas.CreateFrom(xmas, Field, (fieldMin, Threshold + 1)));
                    }
                default:
                    throw new Exception("");
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