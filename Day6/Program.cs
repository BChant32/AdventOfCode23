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
    private static void Main(string[] args)
    {
        long product = 1;
        foreach ((int time, int recordDistance) in input)
        {
            int numberFaster = Enumerable.Range(1, time - 1).Count(i => (time - i) * i > recordDistance);
            Console.WriteLine(numberFaster);
            product *= numberFaster;
        }
        Console.WriteLine("Product: " + product);
    }
}