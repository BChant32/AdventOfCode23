internal class Program
{
    private static void Main(string[] args)
    {
        string input = File.ReadAllText("day15_input.txt");
        long sum = 0;
        foreach (string line in input.Trim().Split(','))
        {
            int hash = GetHash(line);
            Console.WriteLine(hash);
            sum += hash;
        }
        Console.WriteLine("SUM " + sum);
    }

    private static int GetHash(string str)
    {
        int hash = 0;
        foreach (char c in str)
        {
            hash += (int)c;
            hash *= 17;
            hash = hash % 256;
        }
        return hash;
    }
}