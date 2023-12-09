internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day9_input.txt");
        int sum = 0;
        foreach (string line in lines)
        {
            int[] sequence = line.Trim().Split(' ').Select(s => Int32.Parse(s)).ToArray();
            int nextValue = makeNext(sequence);
            Console.WriteLine(nextValue);
            sum += nextValue;
        }
        Console.WriteLine("Sum: " + sum);
    }

    private static int makeNext(int[] sequence)
    {
        if (sequence.Length == 1) throw new Exception("sequence contains only 1");
        int[] nextSequence = new int[sequence.Length - 1];
        for (int i = 0; i < sequence.Length - 1; i++)
        {
            nextSequence[i] = sequence[i + 1] - sequence[i];
        }
        if (nextSequence.All(i => i == 0))
            return sequence[0];
        else
            return sequence[0] - makeNext(nextSequence);
    }
}