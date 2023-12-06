internal class Program
{
    private static List<(int, int)> test = new List<(int, int)>()
    {
        (7, 9),
        (15, 40),
        (30, 200),
    };
    private static List<(int, int)> input = new List<(int, int)>()
    {
        (40, 215),
        (92, 1064),
        (97, 1505),
        (90, 1100),
    };
    private static List<(int, int)> test2 = new List<(int, int)>()
    {
        (71530, 940200),
    };
    private static List<(int, long)> input2 = new List<(int, long)>()
    {
        (40929790, 215106415051100),
    };
    private static void Main(string[] args)
    {
        foreach ((int time, long recordDistance) in input2)
        {
            double delta = Math.Sqrt((double)time * (double)time - 4 * (double)recordDistance);
            //int numberFaster = Enumerable.Range(1, time - 1).Count(i => (time - i) * i > recordDistance);
            double root1 = time + delta / 2;
            double root2 = time - delta / 2;
            Console.WriteLine($"Root 1:{root1}, Root 2:{root2}, Number between:{(int)root1 - (int)root2}");
        }
    }
}