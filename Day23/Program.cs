internal class Program
{
    private static string[] lines;
    private static int width;
    private static int height;
    private static void Main(string[] args)
    {
        lines = File.ReadAllLines("day23_input.txt");
        width = lines[0].Length;
        height = lines.Length;
        Dictionary<(int, int), List<Walker>> graph = new();
        int startX = lines[0].IndexOf('.');
        getNodes(graph, new Walker(startX, 0, Dir.Down, 0));
        //Stack<(int, int)> topoOrder = topologicalOrder(graph);
        Console.WriteLine(longestPath(graph, new HashSet<(int, int)> { (startX, 0) }, (startX, 0), 0));
    }

    private static Stack<(int, int)> topologicalOrder(Dictionary<(int, int), List<Walker>> graph)
    {
        Stack<(int, int)> stack = new();
        Dictionary<(int, int), bool> visited = new();
        foreach (var pos in graph.Keys)
        {
            visited.Add(pos, false);
        }
        foreach (var pos in visited.Keys)
        {
            if (!visited[pos])
                topologicalRecursive(graph, visited, stack, pos);
        }
        return stack;
    }
    private static void topologicalRecursive(Dictionary<(int, int), List<Walker>> graph, Dictionary<(int, int), bool> visited, Stack<(int, int)> stack, (int, int) pos)
    {
        visited[pos] = true;
        foreach (Walker walker in graph[pos])
        {
            if (!visited[pos])
                topologicalRecursive(graph, visited, stack, (walker.x, walker.y));
        }
        stack.Push(pos);
    }
    private static int longestPath(Dictionary<(int, int), List<Walker>> graph, HashSet<(int, int)> visited, (int, int) startPos, int dist)
    {
        if (startPos.Item2 == height - 1) return dist;
        int longest = 0;
        foreach (Walker walker in graph[startPos])
        {
            (int, int) walkerPos = (walker.x, walker.y);
            if (visited.Contains(walkerPos)) continue;
            HashSet<(int, int)> optionVisited = new HashSet<(int, int)>() { walkerPos };
            optionVisited.Union(visited);
            int optionDist = longestPath(graph, optionVisited, walkerPos, walker.dist + dist);
            if (optionDist > longest) longest = optionDist;
        }
        return longest;
    }

    private static void getNodes(Dictionary<(int, int), List<Walker>> graph, Walker walker)
    {
        HashSet<(int, int)> unevaluated = new() { (walker.x, walker.y) };
        HashSet<(int, int)> evaluated = new() { };
        while (unevaluated.Count > 0)
        {
            (int x, int y) = unevaluated.First();
            walker = new Walker(x, y, Dir.None, 0);
            unevaluated.Remove((x,y));
            evaluated.Add((x,y));

            (int, int) pos = (walker.x, walker.y);
            graph.Add(pos, new());
            foreach (Walker option in getOptions(walker)
                .Select(w => followPath(w)))
            {
                graph[pos].Add(option);
                (int, int) newPos = (option.x, option.y);
                if (!evaluated.Contains(newPos))
                    unevaluated.Add(newPos);
            }
        }
    }

    private static Walker? canWalk(int x, int y, Dir dir, int dist)
    {
        return lines[y][x] switch
        {
            '.' => new Walker(x, y, dir, dist),
            '#' => null,
            '<' => dir == Dir.Left ?  new Walker(x, y, dir, dist) : null,
            '>' => dir == Dir.Right ?  new Walker(x, y, dir, dist) : null,
            '^' => dir == Dir.Up ?  new Walker(x, y, dir, dist) : null,
            'v' => dir == Dir.Down ?  new Walker(x, y, dir, dist) : null,
        };
    }

    private static Walker? step(Walker walker, Dir dir)
    {
        return dir switch
        {
            Dir.Left => walker.x == 0 ? null : canWalk(walker.x - 1, walker.y, dir, walker.dist + 1),
            Dir.Right => walker.x == width - 1 ? null : canWalk(walker.x + 1, walker.y, dir, walker.dist + 1),
            Dir.Up => walker.y == 0 ? null : canWalk(walker.x, walker.y - 1, dir, walker.dist + 1),
            Dir.Down => walker.y == height - 1 ? null : canWalk(walker.x, walker.y + 1, dir, walker.dist + 1),
        };
    }

    private static Walker[] getOptions(Walker walker)
    {
        Dir[] allDir = [Dir.Left, Dir.Right, Dir.Up, Dir.Down];
        Dir backwards = opp(walker.dir);
        return allDir.Where(d => d != backwards)
            .Select(dir => step(walker, dir))
            .OfType<Walker>()
            .ToArray();
    }

    private static Walker followPath(Walker walker)
    {
        while (true)
        {
            Walker[] options = getOptions(walker);
            if (options.Length == 0) return walker;
            else if (options.Length == 1)
            {
                walker = options[0];
            }
            else if (options.Length > 1)
            {
                return walker;
            }
        }
    }

    private static Dir opp(Dir dir)
    {
        return dir switch
        {
            Dir.None => Dir.None,
            Dir.Left => Dir.Right,
            Dir.Right => Dir.Left,
            Dir.Up => Dir.Down,
            Dir.Down => Dir.Up,
        };
    }
}

public enum Dir
{
    None, Left, Right, Up, Down,
}
public record Walker(int x, int y, Dir dir, int dist) : IComparable<Walker>
{
    int IComparable<Walker>.CompareTo(Walker? other)
    {
        return dist.CompareTo(other.dist);
    }
};