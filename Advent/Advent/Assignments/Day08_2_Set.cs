using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace Advent.Assignments
{
    internal class Day08_2_Set : IAssignment
    {
        public string InputFile => "day08.txt";
        public int Day => 8;
        public int Part => 2;

        private struct Edge : IComparable<Edge>
        {
            public int From;
            public int To;
            public float Length;

            public Edge(int from, int to, float length)
            {
                From = from;
                To = to;
                Length = length;
            }

            public readonly int CompareTo(Edge other)
            {
                return Length.CompareTo(other.Length);
            }
        }
        
        public string Run(IReadOnlyList<string> input)
        {
            var pointCount = input.Count;
            Vector3[] points = new Vector3[pointCount];
            int[] clusters = new int[pointCount];
            int[] clusterSizes = new int[pointCount];
            var edgeCount = points.Length * (points.Length - 1) / 2;
            var edgeHeap = new MinHeap<Edge>(edgeCount);
            Span<Range> parts = stackalloc Range[3];
            for (var i = 0; i < input.Count; i++)
            {
                var span = input[i].AsSpan();
                span.Split(parts, ',');
                var x = float.Parse(span[parts[0]], NumberStyles.None);
                var y = float.Parse(span[parts[1]], NumberStyles.None);
                var z = float.Parse(span[parts[2]], NumberStyles.None);
                points[i] = new Vector3(x, y, z);

                // Connect the edges
                for (var j = 0; j < i; j++)
                {
                    var from = points[j];
                    var to = points[i];
                    var d = Vector3.DistanceSquared(from, to);
                    if (d > 500000000)
                        continue;
                    var edge = new Edge(i, j, d);
                    edgeHeap.Insert(edge);
                }
            }

            // Create a new forest of sets (each point is added as a set)
            var forest = new UnionFind<Vector3>(points);

            // Do Kruskal's thing
            var clusterCount = pointCount;
            for (var i = 0; i < edgeCount; i++)
            {
                var edge = edgeHeap.Pop();
                var fromSet = forest.FindSetIndex(edge.From);
                var toSet = forest.FindSetIndex(edge.To);
                if (fromSet != toSet)
                {
                    forest.Union(fromSet, toSet);
                    clusterCount--;
                    if (clusterCount == 1)
                    {
                        var fromX = (long)points[edge.From].X;
                        var toX = (long)points[edge.To].X;
                        var result = fromX * toX;
                        return result.ToString();
                    }
                }
            }

            return "Nope";
        }
    }
}
