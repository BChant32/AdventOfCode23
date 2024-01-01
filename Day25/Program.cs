using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day25_input.txt");
        Dictionary<string, HashSet<string>> graph = new();
        makeGraph(graph, lines);
        List<(string, string)> edges = getAllEdges(graph);
        for (int i = 0; i < edges.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                for (int k = 0; k < j; k++)
                {
                    int num = cut3Works(graph, edges[i], edges[j], edges[k]);
                    if (num < graph.Count)
                    {
                        Console.WriteLine(num + " " + (graph.Count - num));
                        goto breakpoint;
                    }
                }
            }
        }
    breakpoint:;
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