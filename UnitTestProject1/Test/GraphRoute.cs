using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

[TestClass]
public class GraphRoute
{
    List<GraphNode> nodes;

    
    public void RouteTest()
    {
        //FindRoute(3, 5);
        FindAllNodes();
    }

    private void InitNodes()
    {
        nodes = new List<GraphNode>();

        GraphNode n1 = new GraphNode(1);
        GraphNode n2 = new GraphNode(2);
        GraphNode n3 = new GraphNode(3);
        GraphNode n4 = new GraphNode(4);
        GraphNode n5 = new GraphNode(5);
        GraphNode n6 = new GraphNode(6);
        GraphNode n7 = new GraphNode(7);

        n1.neighbors.Add(n2);
        n1.neighbors.Add(n3);
        n3.neighbors.Add(n5);
        n5.neighbors.Add(n7);
        n4.neighbors.Add(n6);

        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        nodes.Add(n5);
        nodes.Add(n6);
        nodes.Add(n7);
    }

    private void FindAllNodes()
    {
        InitNodes();

        HashSet<GraphNode> visited = new HashSet<GraphNode>();
        List<List<int>> result = new List<List<int>>();

        if (nodes.Count != 0)
        {
            foreach (GraphNode node in nodes)
            {
                if (visited.Contains(node))
                    continue;
                List<int> tmp = new List<int>();
                //Dfs(node, visited, tmp);
                Bfs(node, visited, tmp);
                result.Add(tmp);
            }
        }

        foreach (List<int> list in result)
        {
            foreach (int i in list)
            {
                Debug.Write(i);
            }
            Debug.WriteLine("\n---");
        }
    }

    private void FindRoute(int current, int target)
    {
        InitNodes();

        GraphNode s = nodes[current];
        GraphNode t = nodes[target];
        HashSet<GraphNode> visited = new HashSet<GraphNode>();
        List<GraphNode> route = new List<GraphNode>();

        foreach (GraphNode node in s.neighbors)
        {
            visited.Add(s);
            route.Add(s);

            if (visited.Contains(node))
                continue;
            visited.Add(node);
            route.Add(node);

            if (Dfs(node, t, visited, route))
                break;
            route.Clear();
            visited.Clear();
        }

        foreach (GraphNode node in route)
        {
            Debug.Write(node.value);
        }
        Debug.WriteLine("");
    }

    private bool Dfs(GraphNode s, GraphNode t, HashSet<GraphNode> visited, List<GraphNode> route)
    {
        if (s.value == t.value)
            return true;

        foreach (GraphNode node in s.neighbors)
        {
            if (visited.Contains(node))
                continue;
            visited.Add(node);
            route.Add(node);

            if (Dfs(node, t, visited, route))
                return true;
        }
        return false;
    }

    private void Dfs(GraphNode current, HashSet<GraphNode> visited, List<int> result)
    {
        visited.Add(current);
        result.Add(current.value);

        foreach (GraphNode node in current.neighbors)
        {
            if (visited.Contains(node))
                continue;
            Dfs(node, visited, result);
        }
    }

    private void Bfs(GraphNode current, HashSet<GraphNode> visited, List<int> result)
    {
        Queue<GraphNode> q = new Queue<GraphNode>();
        visited.Add(current);
        q.Enqueue(current);

        while (q.Count != 0)
        {
            GraphNode qNode = q.Dequeue();
            result.Add(qNode.value);
            foreach (GraphNode node in qNode.neighbors)
            {
                if (visited.Contains(node))
                    continue;
                q.Enqueue(node);
                visited.Add(node);
            }
        }
    }
}

class GraphNode
{
    public int value;
    public List<GraphNode> neighbors;
    public GraphNode(int value)
    {
        this.value = value;
        neighbors = new List<GraphNode>();
    }
}
