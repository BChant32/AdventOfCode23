internal class Program
{
    private static void Main(string[] args)
    {
        Dictionary<int, (LinkedList<string>, Dictionary<string, int>)> boxes = new Dictionary<int, (LinkedList<string>, Dictionary<string, int>)>();
        string input = File.ReadAllText("day15_input.txt");
        foreach (string line in input.Trim().Split(','))
        {
            string label;
            int hash;
            if (line.Contains('-'))
            {
                label = line.TrimEnd('-');
                hash = GetHash(label);
                if (!boxes.ContainsKey(hash)) continue;
                (LinkedList<string> ll, Dictionary<string, int> d) = boxes[hash];
                ll.Remove(label);
                d.Remove(label);
            }
            else
            {
                label = line.Split('=')[0];
                int power = Int32.Parse(line.Split('=')[1]);
                hash = GetHash(label);
                if (!boxes.ContainsKey(hash))
                    boxes.Add(hash, (new LinkedList<string>([label]), new Dictionary<string, int>() { { label, power} }));
                else
                {
                    (LinkedList<string> ll, Dictionary<string, int> d) = boxes[hash];
                    if (ll.Find(label) is not null)
                        d[label] = power;
                    else
                    {
                        ll.AddLast(label);
                        d.Add(label, power);
                    }
                }
            }
        }

        long sum = 0;
        foreach (var kvp in boxes)
        {
            string printStr = String.Join("] [", kvp.Value.Item1.Select(s => s + ' ' + kvp.Value.Item2[s]));
            Console.WriteLine($"Box {kvp.Key}: [{printStr}]");
            int boxNum = kvp.Key + 1;
            int position = 1;
            foreach (string label in kvp.Value.Item1)
            {
                sum += boxNum * position++ * kvp.Value.Item2[label];
            }
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