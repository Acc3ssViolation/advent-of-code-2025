using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day11_2 : IAssignment
    {
        public string TestFile => "test-day11p2.txt";

        private class ServerNodes : PathFinding.INodeProvider<string>
        {
            private readonly Dictionary<string, List<string>> _connections = new();
            private readonly string _start;
            private readonly string _goal;

            public ServerNodes(string start, string goal, IEnumerable<string> input)
            {
                _start = start;
                _goal = goal;
                _connections[_start] = [];
                _connections[_goal] = [];

                foreach (var node in input)
                    Parse(node);
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
            // Figure out if FFT is in front or behind of DAC
            long fftToDac = PathFinding.CountPaths(new ServerNodes("fft", "dac", input));
            long dacToFft = PathFinding.CountPaths(new ServerNodes("dac", "fft", input));
            Debug.Assert(fftToDac == 0 || dacToFft == 0);

            if (fftToDac != 0)
            {
                // SVR -> FFT -> DAC -> OUT
                Logger.DebugLine("SVR -> FFT -> DAC -> OUT");
                long svrToFft = PathFinding.CountPaths(new ServerNodes("svr", "fft", input));
                long dacToOut = PathFinding.CountPaths(new ServerNodes("dac", "out", input));
                long pathCount = svrToFft * fftToDac * dacToOut;
                return pathCount.ToString();
            }
            else if (dacToFft != 0)
            {
                // SVR -> DAC -> FFT -> OUT
                Logger.DebugLine("SVR -> DAC -> FFT -> OUT");
                long svrToDac = PathFinding.CountPaths(new ServerNodes("svr", "dac", input));
                long fftToOut = PathFinding.CountPaths(new ServerNodes("fft", "out", input));
                long pathCount = svrToDac * dacToFft * fftToOut;
                return pathCount.ToString();
            }
            else
            {
                // Impossible!
                return "NO PATH";
            }
        }
    }
}
