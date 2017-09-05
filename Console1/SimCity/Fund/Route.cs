using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCity.Fund
{
    class Route
    {
        public void FindAllNodes(List<Position> nodes)
        {
            HashSet<Position> visited = new HashSet<Position>();
            List<List<string>> result = new List<List<string>>();

            if (nodes.Count != 0)
            {
                foreach (Position node in nodes)
                {
                    if (visited.Contains(node))
                        continue;
                    List<string> tmp = new List<string>();
                    //Dfs(node, visited, tmp);
                    Bfs(node, visited, tmp);
                    result.Add(tmp);
                }
            }

            foreach (List<string> list in result)
            {
                foreach (string s in list)
                {
                    Console.Write(s);
                }
                Console.WriteLine("\n---");
            }
        }

        public void FindRoute(string current, string target, List<Position> nodes)
        {
            Position s = nodes.Find(item => item.GetName().Equals(current));
            Position t = nodes.Find(item => item.GetName().Equals(target));

            if (s == null || t == null)
            {
                Console.Error.WriteLine("Route Failed! Can't find the start or end position:\""+ current + "\" " + target + "\"");
                return;
            }

            HashSet<Position> visited = new HashSet<Position>();
            List<Position> route = new List<Position>();

            foreach (Position node in s.GetNeighbors())
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

            if (route.Count == 0)
            {
                Console.WriteLine("Can't find the route from \"" + current + "\" to " + target + "\"");
            }

            foreach (Position node in route)
            {
                Console.Write(node.GetName());
            }
            Console.WriteLine("");

            SimplifyRoute(route);

            foreach (Position node in route)
            {
                Console.Write(node.GetName());
            }
            Console.WriteLine("");
        }

        private void SimplifyRoute(List<Position> route)
        {
            List<Position> removeList = new List<Position>();
            for (int i = 0; i < route.Count; i++)
            {
                for (int j = route.Count - 1; j > i - 1 && !removeList.Contains(route[j]); j--)
                {
                    if (i < j - 1 && route[i].GetNeighbors().Contains(route[j]))
                    {
                        for (int n = 0; n < j - i - 1; n++)
                        {
                            removeList.Add(route[i + n + 1]);
                        }
                        i = j;
                        break;
                    }
                }
            }

            route.RemoveAll(item => removeList.Contains(item));
        }

        // route DFS
        private bool Dfs(Position s, Position t, HashSet<Position> visited, List<Position> route)
        {
            if (s.GetName() == t.GetName())
                return true;

            foreach (Position node in s.GetNeighbors())
            {
                if (visited.Contains(node))
                    continue;
                visited.Add(node);
                route.Add(node);

                if (Dfs(node, t, visited, route))
                    return true;
                
                route.Remove(route[route.Count - 1]);
            }
            //route.Clear();
            return false;
        }

        private void Dfs(Position current, HashSet<Position> visited, List<string> result)
        {
            visited.Add(current);
            result.Add(current.GetName());

            foreach (Position node in current.GetNeighbors())
            {
                if (visited.Contains(node))
                    continue;
                Dfs(node, visited, result);
            }
        }

        private void Bfs(Position current, HashSet<Position> visited, List<string> result)
        {
            Queue<Position> q = new Queue<Position>();
            visited.Add(current);
            q.Enqueue(current);

            while (q.Count != 0)
            {
                Position qNode = q.Dequeue();
                result.Add(qNode.GetName());
                foreach (Position node in qNode.GetNeighbors())
                {
                    if (visited.Contains(node))
                        continue;
                    q.Enqueue(node);
                    visited.Add(node);
                }
            }
        }
    }
}
