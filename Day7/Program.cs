internal class Program
{
    private static void Main(string[] args)
    {
        List<Hand> allHands = File.ReadAllLines("day7_input.txt").Select(l => Hand.ReadHand(l)).ToList();
        allHands.Sort();
        Console.WriteLine(String.Join("\r\n", allHands));
        long sum = 0;
        for (int i = 1; i <= allHands.Count; i++)
        {
            sum += i * allHands[i-1].Bid;
        }
        Console.WriteLine(sum);
    }
}

public class Hand : IComparable<Hand>
{
    public int Bid = 0;
    public int[] Cards = new int[5];

    public Hand(int bid)
    {
        Bid = bid;
    }

    // Calculations
    public int CompareTo(Hand? otherHand)
    {
        if (otherHand is null) return 1;
        int thisTypeValue = (int)getType();
        int otherTypeValue = (int)otherHand.getType();
        if (thisTypeValue != otherTypeValue) return thisTypeValue - otherTypeValue;
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (Cards[i] != otherHand.Cards[i])
                    return Cards[i] - otherHand.Cards[i];
            }
        }
        return 0;
    }
    enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeKind,
        FullHouse,
        FourKind,
        FiveKind,
    }
    private HandType getType()
    {
        IEnumerable<int> cardsNoJ = Cards.Where(c => c != 1);
        IGrouping<int, int>[] grouping = cardsNoJ
            .GroupBy(i => i)
            .OrderByDescending(g => g.Count())
            .ToArray();
        if (grouping.Length == 0) return HandType.FiveKind; // All J
        int most = grouping[0].Count();
        int numJ = 5 - cardsNoJ.Count();
        return (most + numJ) switch
        {
            5 => HandType.FiveKind,
            4 => HandType.FourKind,
            3 => (grouping[1].Count() == 2) ? HandType.FullHouse : HandType.ThreeKind,
            2 => (grouping[1].Count() == 2) ? HandType.TwoPair : HandType.OnePair,
            _ => HandType.HighCard,
        };
    }

    // Helpers
    public override string ToString()
    {
        return $"{String.Join("", Cards.Select(i => WriteCard(i)))} {Bid}";
    }

    public static Hand ReadHand(string line)
    {
        string[] lineHalves = line.Trim().Split(' ');
        Hand hand = new Hand(Int32.Parse(lineHalves[1]));
        for (int i = 0; i < 5; i++)
        {
            hand.Cards[i] = ReadCard(lineHalves[0][i]);
        }
        return hand;
    }
    public static int ReadCard(char c)
        => c switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 1,
            'T' => 10,
            _ => Int32.Parse(c.ToString()),
        };
    public static char WriteCard(int i)
        => i switch
        {
            14 => 'A',
            13 => 'K',
            12 => 'Q',
            1 => 'J',
            10 => 'T',
            _ => char.Parse(i.ToString()),
        };
}