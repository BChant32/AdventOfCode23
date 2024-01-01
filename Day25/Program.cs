internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day25_input.txt");
        Dictionary<string, HashSet<string>> graph = new();
        makeGraph(graph, lines);
        List<string> nodes = graph.Keys.ToList();
        string nodeA = nodes[0];
        int count = 0;
        foreach (string nodeB in nodes.Skip(1))
        {
            Dictionary<string, HashSet<string>> graphCopy = graph.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToHashSet());
            for (int i = 0; i < 3; i++)
            {
                List<string> path = findPath(graphCopy, nodeA, nodeB);
                deletePath(graphCopy, path);
            }
            List<string> potentialPath = findPath(graphCopy, nodeA, nodeB);
            if (!potentialPath.Any()) count++;
        }
        Console.WriteLine(count);
    }

    private static void deletePath(Dictionary<string, HashSet<string>> graph, List<string> path)
    {
        for (int i = 1; i < path.Count; i++)
        {
            string nodeA = path[i-1];
            string nodeB = path[i];
            graph[nodeA].Remove(nodeB);
            graph[nodeB].Remove(nodeA);
        }
    }

    private static List<string> findPath(Dictionary<string, HashSet<string>> graph, string nodeA, string nodeB)
    {
        HashSet<List<string>> allPaths = new() { new List<string> { nodeA } };
        HashSet<string> visitedNodes = new() { nodeA };
        HashSet<string> options;
        List<string> newPath;
        while (allPaths.Any())
        {
            HashSet<string> lastNodes = visitedNodes.ToHashSet();
            foreach (List<string> path in allPaths.ToArray())
            {
                allPaths.Remove(path);
                options = graph[path.Last()];
                foreach (string option in options)
                {
                    if (!path.Contains(option))
                    {
                        visitedNodes.Add(option);
                        newPath = path.Append(option).ToList();
                        if (option == nodeB)
                            return newPath;
                        allPaths.Add(newPath);
                    }
                }
            }
            if (lastNodes.SetEquals(visitedNodes))
            {
                Console.WriteLine(lastNodes.Count); // 712
                break;
            }
        }
        return new();
    }

    private static int cut3Works(Dictionary<string, HashSet<string>> graph, (string, string) edge1, (string, string) edge2, (string, string) edge3)
    {
        bool isCutEdge(string vertex1, string vertex2)
        {
            return edge1.Item1 == vertex1 && edge1.Item2 == vertex2
                || edge1.Item1 == vertex2 && edge1.Item2 == vertex1
                || edge2.Item1 == vertex1 && edge2.Item2 == vertex2
                || edge2.Item1 == vertex2 && edge2.Item2 == vertex1
                || edge3.Item1 == vertex1 && edge3.Item2 == vertex2
                || edge3.Item1 == vertex2 && edge3.Item2 == vertex1;
        }

        HashSet<string> vertexGroup = new();
        HashSet<string> nextVertices = new() { graph.Keys.First() };
        while (nextVertices.Count > 0)
        {
            foreach (string vertex in nextVertices.ToArray())
            {
                vertexGroup.Add(vertex);
                nextVertices.Remove(vertex);
                foreach (string vertex2 in graph[vertex])
                {
                    if (!isCutEdge(vertex, vertex2) && !vertexGroup.Contains(vertex2) && !nextVertices.Contains(vertex2))
                        nextVertices.Add(vertex2);
                }
            }
        }
        return vertexGroup.Count;
    }

    private static List<(string, string)> getAllEdges(Dictionary<string, HashSet<string>> graph)
    {
        List<(string, string)> allEdges = new();
        foreach (string vertex1 in graph.Keys)
        {
            foreach (string vertex2 in graph[vertex1])
            {
                if (!allEdges.Contains((vertex1, vertex2))
                    && !allEdges.Contains((vertex2, vertex1)))
                    allEdges.Add((vertex1, vertex2));
            }
        }
        return allEdges;
    }

    private static void makeGraph(Dictionary<string, HashSet<string>> graph, string[] lines)
    {
        foreach (string line in lines)
        {
            string[] colonSplit = line.Split(':');
            foreach (string vertex in colonSplit[1].Trim().Split(' '))
            {
                addEdge(graph, colonSplit[0], vertex);
            }
        }
    }
    private static void addEdge(Dictionary<string, HashSet<string>> graph, string vertex1, string vertex2)
    {
        if (!graph.ContainsKey(vertex1)) graph.Add(vertex1, new HashSet<string>());
        if (!graph.ContainsKey(vertex2)) graph.Add(vertex2, new HashSet<string>());

        graph[vertex1].Add(vertex2);
        graph[vertex2].Add(vertex1);
    }
}