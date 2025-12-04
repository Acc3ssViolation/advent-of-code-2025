using Advent.Shared;
using System;
using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day04_2_Graph : IAssignment
    {
        public string InputFile => "day04.txt";
        public int Day => 4;
        public int Part => 2;

        private static readonly Point[] Neighbours =
        [
            Point.East,
            Point.South + Point.East,
            Point.South,
            Point.South + Point.West,
        ];

        private record Node(Point Point, FixedList<Node> Neighbours);

        private class Graph
        {
            private readonly Dictionary<Point, Node> _nodes;
            private readonly Node[] _neighbourStorage;
            private int _neighbourStorageIndex;

            public Graph(int fieldSize)
            {
                _nodes = new Dictionary<Point, Node>(fieldSize);
                _neighbourStorage = new Node[fieldSize * 8];
            }

            public Node GetNode(Point point)
            {
                if (!_nodes.TryGetValue(point, out var node))
                {
                    var mem = _neighbourStorage.AsMemory(_neighbourStorageIndex, 8);
                    _neighbourStorageIndex += 8;
                    node = new Node(point, new FixedList<Node>(mem));
                    _nodes.Add(point, node);
                }
                return node;
            }

            public void Connect(Node a, Node b)
            {
                a.Neighbours.Add(b);
                b.Neighbours.Add(a);
            }

            public int EliminateSmallClusters(int limit)
            {
                var queue = new Queue<Node>(_nodes.Count);
                var purgeCount = 0;

                foreach (var node in _nodes.Values)
                {
                    if (node.Neighbours.Count < limit)
                        queue.Enqueue(node);
                }

                while (queue.TryDequeue(out var node))
                {
                    if (node.Neighbours.Count < limit)
                    {
                        purgeCount++;
                        foreach (var n in node.Neighbours)
                        {
                            var prevCount = n.Neighbours.Count;
                            n.Neighbours.Remove(node);
                            if (prevCount >= limit && n.Neighbours.Count < limit)
                                queue.Enqueue(n);
                        }
                        node.Neighbours.Clear();
                    }
                }

                return purgeCount;
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var w = input[0].Length;
            var h = input.Count;
            var graph = new Graph(w * h);

            // Parse input into a graph of nodes. We only have to insert nodes for tiles that contain a roll (@)
            // TODO: This is still a bit slow
            for (var y = 0; y < h; y++)
            {
                var line = input[y];
                for (var x = 0; x < w; x++)
                {
                    var obj = line[x];
                    if (obj != '@')
                        continue;

                    var node = graph.GetNode(new Point(x, y));

                    for (var i = 0; i < Neighbours.Length; i++)
                    {
                        var nx = Neighbours[i].x + x;
                        var ny = Neighbours[i].y + y;

                        if (ny >= 0 && ny < w && nx >= 0 && nx < h)
                        {
                            if (input[ny][nx] != '@')
                                continue;

                            var neighbour = graph.GetNode(new Point(nx, ny));
                            graph.Connect(node, neighbour);
                        }
                    }
                }
            }

            var accessibleRolls = graph.EliminateSmallClusters(4);
            return accessibleRolls.ToString();
        }
    }
}
