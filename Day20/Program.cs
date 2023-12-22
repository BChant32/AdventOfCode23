internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day20_input.txt");
        List<(Module, List<string>)> allModulesAndOutputs = new();
        foreach (string line in lines)
        {
            List<string> outputs = line.Split("->")[1].Trim().Split(", ").ToList();
            if (line.StartsWith("broadcaster"))
                allModulesAndOutputs.Add((new Broadcaster() { Name = "broadcaster" }, outputs));
            else if (line.StartsWith("%"))
                allModulesAndOutputs.Add((new FlipFlop() { Name = line.TrimStart('%').Split("->")[0].Trim() }, outputs));
            else if (line.StartsWith("&"))
                allModulesAndOutputs.Add((new Conjunction() { Name = line.TrimStart('&').Split("->")[0].Trim() }, outputs));
            else throw new Exception();
        }

        List<Module> allModules = allModulesAndOutputs.Select(m => m.Item1).ToList();
        foreach ((Module module, List<string> outputs) in allModulesAndOutputs)
        {
            foreach (string outputName in outputs)
            {
                Module? output = allModules.FirstOrDefault(m => m.Name == outputName);
                if (output is null)
                {
                    output = new Dud() { Name = outputName };
                    allModules.Add(output);
                }
                else if (output is Conjunction conjunction)
                {
                    conjunction.Register(module.Name);
                }
                module.outputs.Add(output);
            }
        }

        int i = 0;
        while (!Module.rxHit)
        {
            i++;
            if (i % 100000 == 0) Console.WriteLine(i);
            Module.LowCounter++;
            allModules.OfType<Broadcaster>().Single().Send(false);
            if (Module.rxHit) Console.WriteLine("rx#: " + i);
        }
        
        Console.WriteLine($"Low:{Module.LowCounter} High:{Module.HighCounter}");
        Console.WriteLine(Module.LowCounter * Module.HighCounter);
    }
}

public abstract class Module
{
    public static int LowCounter = 0;
    public static int HighCounter = 0;
    public static bool rxHit = false;

    public string Name { get; set; }
    public List<Module> outputs = new();

    public abstract bool? Process(Module sender, bool signal);
    public void Send(bool signal)
    {
        if (!signal && Name == "ls") rxHit = true;
        //Console.WriteLine(Name + ": " + (signal ? "high" : "low"));
        Dictionary<Module, bool> tick = new();
        foreach (Module module in outputs)
        {
            if (signal) HighCounter++;
            else LowCounter++;
            bool? send = module.Process(this, signal);
            if (send.HasValue) tick[module] = send.Value;
        }

        foreach ((Module module, bool send) in tick)
        {
            module.Send(send);
        }
    }
}

public class Broadcaster : Module
{
    public override bool? Process(Module sender, bool signal)
        => signal;
}

public class FlipFlop : Module
{
    private bool internalState = false;

    public override bool? Process(Module sender, bool signal)
    {
        if (signal) return null;
        
        internalState = !internalState;
        return internalState;
    }
}

public class Conjunction : Module
{
    private Dictionary<string, bool> memory = new();

    public void Register(string senderName) => memory[senderName] = false;

    public override bool? Process(Module sender, bool signal)
    {
        memory[sender.Name] = signal;
        return !memory.Values.All(v => v);
    }
}

public class Dud : Module
{
    public override bool? Process(Module sender, bool signal)
        => null;
}