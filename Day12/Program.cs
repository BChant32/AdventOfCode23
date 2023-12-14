using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

internal class Program
{
    private static ConcurrentDictionary<(string, int[]), long> knownResults = new(new Comp());

    protected class Comp : IEqualityComparer<(string, int[])>
    {
        public bool Equals((string, int[]) x, (string, int[]) y)
        {
            return x.Item1.Equals(y.Item1) && x.Item2.SequenceEqual(y.Item2);
        }

        public int GetHashCode([DisallowNull] (string, int[]) obj)
        {
            return obj.Item1.GetHashCode();
        }
    }

    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day12_input.txt");
        long sum = 0;
        int numDone = 0;
        Parallel.ForEach(lines, line =>
        {
            string[] halves = line.Trim().Split(' ');
            string word = halves[0];
            word = $"{word}?{word}?{word}?{word}?{word}";
            int[] sequence = halves[1].Split(',').Select(c => Int32.Parse(c)).ToArray();
            sequence = Enumerable.Range(0, 5).SelectMany(i => sequence).ToArray();
            long combinations = CalcCombinations(word, (int[])sequence.Clone());
            //long combinations = AnotherCalc(word.Select(c => (byte)(c == '.' ? 0 : c == '#'? 1 : 2)).ToArray(), sequence);
            sum += combinations;
            Console.WriteLine(numDone++);
        });
        Console.WriteLine("Sum: " + sum);
    }

    private static long CalcCombinations(string word, int[] sequence, bool forceConsume = false)
    {
        //Console.WriteLine(word + ' ' + String.Join(',', sequence));
        if (sequence.Length < 1) return word.IndexOf('#') < 0 ? 1 : 0;
        if (forceConsume && word.StartsWith('.')) return 0;
        word = word.TrimStart('.');
        if (word.Length < 1) return sequence.Any() ? 0 : 1;
        if (knownResults.ContainsKey((word, sequence))) return knownResults[(word, sequence)];
        if (word[0] == '?')
        {
            string subword = word.Substring(1);
            if (forceConsume) return CalcCombinations('#' + subword, sequence, true);
            long variant1 = CalcCombinations('#' + subword, (int[])sequence.Clone(), forceConsume);
            //Console.WriteLine(variant1 + " #" + subword + ' ' + String.Join(',', sequence));
            long variant2 = CalcCombinations('.' + subword, (int[])sequence.Clone(), forceConsume);
            //Console.WriteLine(variant2 + " ." + subword + ' ' + String.Join(',', sequence));
            knownResults[(word, sequence)] = variant1 + variant2;
            return variant1 + variant2;
        }
        else // word[0] == '#'
        {
            if (word.Length < sequence[0]) return 0;
            for (int i = 0; i < sequence[0]; i++)
            {
                if (word[i] == '.') return 0;
            }
            word = word.Substring(sequence[0]);
            if (word.Length == 0) return sequence.Length == 1 ? 1 : 0;
            if (word[0] == '#') return 0;
            return CalcCombinations(word.Substring(1), sequence.Skip(1).ToArray());

            //word = word.Substring(1);
            //if (sequence[0] == 1)
            //{
            //    if (word.Length == 0)
            //        return sequence.Length == 1 ? 1 : 0;

            //    sequence = sequence.Skip(1).ToArray();
            //    return word[0] == '#' ? 0 : CalcCombinations(word.Substring(1), sequence);
            //}
            //else
            //{
            //    sequence[0]--;
            //    return CalcCombinations(word, sequence, true);
            //}
        }
    }

    private static long AnotherCalc(byte[] word, int[] sequence)
    {
        int question = -1;
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 2)
            {
                question = i;
                break;
            }
        }
        if (question == -1)
        {
            BitArray b = new BitArray(word.Length, false);
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == 1) b[i] = true;
            }
            if (b.Cast<bool>().Count() != sequence.Sum()) return 0;
            return ReadBitArray(b).ToArray().SequenceEqual(sequence) ? 1 : 0;
        }
        else
        {
            byte[] word1 = word.ToArray(), word2 = word.ToArray();
            word1[question] = 0;
            word2[question] = 1;
            return AnotherCalc(word1, sequence) + AnotherCalc(word2, sequence);
        }
    }
    private static IEnumerable<int> ReadBitArray(BitArray b)
    {
        int count = 0;

        for (int i = 0; i < b.Length; i++)
        {
            if (b[i]) count++;
            else if (count > 0)
            {
                yield return count;
                count = 0;
            }
        }
        if (count > 0)
            yield return count;
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
    // 2nd half and: 30568243604962
}