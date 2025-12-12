using Advent.Shared;
using static Advent.Shared.PathFinding;

namespace Advent.Test
{
    public class PathFindingTests
    {
        private class IntNodeProvider : INodeProvider<int>
        {
            private readonly Dictionary<int, List<int>> _neighbours = new();
            private readonly int _start;
            private readonly int _goal;

            public IntNodeProvider(IEnumerable<(int, int)> edges, int start, int goal)
            {
                foreach (var edge in edges) 
                {
                    var from = edge.Item1;
                    var to = edge.Item2;
                    if (!_neighbours.TryGetValue(from, out var neighbours))
                    {
                        neighbours = new List<int>();
                        _neighbours[from] = neighbours;
                    }
                    neighbours.Add(to);
                }

                _start = start;
                _goal = goal;

                if (!_neighbours.TryGetValue(_start, out var _))
                    _neighbours[start] = [];

                if (!_neighbours.TryGetValue(goal, out var _))
                    _neighbours[goal] = [];
            }

            public int GetDistance(int node, int neighbour)
            {
                return 1;
            }

            public int GetGoal()
            {
                return _goal;
            }

            public int GetHeuristic(int node)
            {
                return 1;
            }

            public IEnumerable<int> GetNeighbours(int node)
            {
                return _neighbours[node];
            }

            public IEnumerable<int> GetNodes()
            {
                return _neighbours.Keys;
            }

            public int GetStart()
            {
                return _start;
            }

            public bool IsGoal(int node)
            {
                return node == _goal; ;
            }
        }

        [Fact]
        public void CountPaths()
        {
            var edges = new List<(int, int)>()
            {
                (1, 2),
                (1, 3),
                (1, 5),
                (2, 5),
                (2, 4),
                (3, 5),
                (4, 3),
            };
            var count = PathFinding.CountPaths(new IntNodeProvider(edges, 1, 5));
            Assert.Equal(4, count);
        }
    }
}