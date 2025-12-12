using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent.Shared.PathFinding;

namespace Advent.Assignments
{
    internal class Day11_1 : IAssignment
    {
        private class ServerNodes : INodeProvider<string>
        {
            private readonly Dictionary<string, List<string>> _connections = new();
            private readonly string _start;
            private readonly string _goal;

            public ServerNodes(string start, string goal)
            {
                _start = start;
                _goal = goal;
                _connections[_start] = [];
                _connections[_goal] = [];
            }

            public void Parse(string input)
            {
                var parts = input.Split(' ');
                var node = parts[0][..^1];
                var connections = new List<string>();
                for (var i = 1; i < parts.Length; i++)
                    connections.Add(parts[i]);
                _connections[node] = connections;
            }

            public int GetDistance(string node, string neighbour)
            {
                return 1;
            }

            public int GetHeuristic(string node)
            {
                return 1;
            }

            public IEnumerable<string> GetNeighbours(string node)
            {
                if (_connections.TryGetValue(node, out var neighbours))
                    return neighbours;
                return Array.Empty<string>();
            }

            public string GetStart()
            {
                return _start;
            }

            public bool IsGoal(string node)
            {
                return node == _goal;
            }

            public string GetGoal()
            {
                return _goal;
            }

            public IEnumerable<string> GetNodes()
            {
                return _connections.Keys;
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var serverNodes = new ServerNodes("you", "out");
            foreach (var node in input)
                serverNodes.Parse(node);

            var pathCount = PathFinding.CountPaths(serverNodes);
            return pathCount.ToString();
        }
    }
}
