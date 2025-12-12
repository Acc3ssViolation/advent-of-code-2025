using System.Collections.Generic;
using System.Linq.Expressions;

namespace Advent.Shared
{
    public static class PathFinding
    {
        public interface INodeProvider<T>
        {
            T GetStart();
            T GetGoal();
            bool IsGoal(T node);
            IEnumerable<T> GetNodes();
            IEnumerable<T> GetNeighbours(T node);
            int GetDistance(T node, T neighbour);
            int GetHeuristic(T node);
        }

        public static int CountPaths<T>(INodeProvider<T> nodeProvider) where T : notnull
        {
            var indegree = new Dictionary<T, int>();
            foreach (var node in nodeProvider.GetNodes())
            {
                foreach (var neighbour in nodeProvider.GetNeighbours(node))
                {
                    if (!indegree.TryGetValue(neighbour, out var value))
                        value = 0;
                    indegree[neighbour] = value + 1;
                }
            }

            var queue = new Queue<T>();
            foreach (var node in nodeProvider.GetNodes())
                if (!indegree.ContainsKey(node))
                    queue.Enqueue(node);

            var topOrder = new List<T>();
            while (queue.TryDequeue(out var node))
            {
                topOrder.Add(node);
                foreach (var neighbour in  nodeProvider.GetNeighbours(node))
                {
                    indegree[neighbour]--;
                    if (indegree[neighbour] == 0)
                        queue.Enqueue(neighbour);
                }
            }

            var ways = new Dictionary<T, int>();
            ways[nodeProvider.GetStart()] = 1;
            foreach (var node in topOrder)
            {
                if (!ways.TryGetValue(node, out var nodeWays))
                    nodeWays = 0;

                foreach (var neighbour in nodeProvider.GetNeighbours(node))
                {
                    if (!ways.TryGetValue(neighbour, out var neighbourWays))
                        neighbourWays = 0;
                    ways[neighbour] = neighbourWays + nodeWays;
                }
            }

            return ways[nodeProvider.GetGoal()];
        }

        public static List<T>? Dijkstra<T>(INodeProvider<T> nodeProvider) where T : notnull
        {
            var visited = new HashSet<T>();
            var openSet = new PriorityQueue<T, int>();
            var gScore = new Dictionary<T, int>();
            var cameFrom = new Dictionary<T, T?>();
            var start = nodeProvider.GetStart();
            openSet.Enqueue(start, 0);
            cameFrom[start] = default;
            gScore[start] = 0;
            while (openSet.TryDequeue(out var node, out var _))
            {
                if (visited.Contains(node))
                    continue;
                visited.Add(node);

                if (nodeProvider.IsGoal(node))
                {
                    var path = new List<T>();
                    while (node != null)
                    {
                        path.Add(node);
                        node = cameFrom[node];
                    }
                    return path;
                }

                var neighbours = nodeProvider.GetNeighbours(node);
                foreach (var neighbour in neighbours)
                {
                    var tgScore = gScore[node] + nodeProvider.GetDistance(node, neighbour);
                    if (!gScore.TryGetValue(neighbour, out var gScoreNeighbour))
                    {
                        gScoreNeighbour = int.MaxValue;
                    }
                    if (tgScore < gScoreNeighbour)
                    {
                        cameFrom[neighbour] = node;
                        gScore[neighbour] = tgScore;
                        openSet.Enqueue(neighbour, tgScore);
                    }
                }
            }

            return null;
        }
    }
}
