using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day12_input.txt");
        int sum = 0;
        foreach (string line in lines)
        {
            string[] halves = line.Trim().Split(' ');
            string word = halves[0];
            int[] sequence = halves[1].Split(',').Select(c => Int32.Parse(c)).ToArray();
            int combinations = CalcCombinations(word, (int[])sequence.Clone());
            //sum += combinations;
            int secondTry = OtherCalc(word, sequence);
            sum += secondTry;
            //Console.WriteLine(combinations + " " + secondTry);
        }
        Console.WriteLine("Sum: " + sum);
    }

    private static int CalcCombinations(string word, int[] sequence, bool forceConsume = false)
    {
        //Console.WriteLine(word + ' ' + String.Join(',', sequence));
        if (sequence.Length < 1) return 1;
        if (forceConsume && word.StartsWith('.')) return 0;
        word = word.TrimStart('.');
        if (word.Length < 1) return sequence.Any() ? 0 : 1;
        if (word[0] == '?')
        {
            word = word.Substring(1);
            if (forceConsume) return CalcCombinations('#' + word, sequence, true);
            int variant1 = CalcCombinations('#' + word, (int[])sequence.Clone(), forceConsume);
            //Console.WriteLine(variant1 + " #" + word + ' ' + String.Join(',', sequence));
            int variant2 = CalcCombinations('.' + word, (int[])sequence.Clone(), forceConsume);
            //Console.WriteLine(variant2 + " ." + word + ' ' + String.Join(',', sequence));
            return variant1 + variant2;
        }
        else if (word[0] == '#')
        {
            word = word.Substring(1);
            if (sequence[0] == 1)
            {
                if (word.Length == 0)
                    return sequence.Length == 1 ? 1 : 0;

                sequence = sequence.Skip(1).ToArray();
                return word[0] == '#' ? 0 : CalcCombinations(word.Substring(1), sequence);
            }
            else
            {
                sequence[0]--;
                return CalcCombinations(word, sequence, true);
            }
        }
        else throw new Exception("Unhandled case");
    }

    private static int OtherCalc(string word, int[] sequence)
    {
        List<string> words = MakeVariants(word);
        string[] variantsValid = words.Where(w => Complies(w, sequence)).ToArray();
        //Console.WriteLine(String.Join("\r\n", variantsValid));
        Complies(variantsValid[0], sequence);
        return variantsValid.Count();
    }
    private static List<string> MakeVariants(string word)
    {
        int index = word.IndexOf('?');
        if (index < 0) return new List<string>() { word };
        return MakeVariants(word.Substring(0, index) + '.' + word.Substring(index + 1))
            .Concat(MakeVariants(word.Substring(0, index) + '#' + word.Substring(index + 1))).ToList();
    }
    private static bool Complies(string word, int[] sequence)
    {
        while (sequence.Length > 0)
        {
            word = word.TrimStart('.');
            if (Regex.IsMatch(word, $"^#{{{sequence[0]}}}(?=$|\\.)"))
            {
                word = word.Substring(sequence[0]);
                sequence = sequence.Skip(1).ToArray();
            }
            else return false;
        }

        word = word.TrimStart('.');
        return word.Length == 0;
    }

    // 1st half ans: 7407
}