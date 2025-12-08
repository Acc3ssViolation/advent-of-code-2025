using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace Advent.Assignments
{
    internal class Day08_1 : IAssignment
    {
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
            Vector3[] points = new Vector3[input.Count];
            int[] clusters = new int[input.Count];
            int[] clusterSizes = new int[input.Count];
            var edgeCount = points.Length * (points.Length - 1) / 2;
            var edges = new MinHeap<Edge>(edgeCount);
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
                    edges.Insert(new Edge(i, j, Vector3.DistanceSquared(from, to)));
                }
            }

            var clusterId = 1;
            var bound = input.Count > 25 ? 1000 : 10;

            for (var round = 1; round <= bound; round++)
            {
                var edge = edges.Pop();

                //Logger.DebugLine($"Connecting [{points[edge.From]}] to [{points[edge.To]}]");

                var fromCluster = clusters[edge.From];
                var toCluster = clusters[edge.To];

                if (fromCluster == 0 && toCluster == 0)
                {
                    // Neither is in a cluster, create a new cluster!
                    //Logger.DebugLine($"> Creating new cluster {clusterId}");
                    clusters[edge.From] = clusters[edge.To] = clusterId;
                    clusterSizes[clusterId] += 2;

                    clusterId++;
                }
                else if (fromCluster == 0 && toCluster != 0)
                {
                    // Join toCluster
                    //Logger.DebugLine($"> Joining cluster {toCluster}");
                    clusters[edge.From] = toCluster;
                    clusterSizes[toCluster]++;
                }
                else if (fromCluster != 0 && toCluster == 0)
                {
                    // Join fromCluster
                    //Logger.DebugLine($"> Joining cluster {fromCluster}");
                    clusters[edge.To] = fromCluster;
                    clusterSizes[fromCluster]++;
                }
                else if (fromCluster != 0 && toCluster != 0 && fromCluster == toCluster)
                {
                    // Both are in the same cluster, nothing happens
                    //Logger.DebugLine($"> Both in cluster {fromCluster}");
                }
                else
                {
                    // Both are in a cluster, so both clusters must be merged
                    //Logger.DebugLine($"> Merging cluster {fromCluster} into cluster {toCluster}");

                    for (var i = 0; i < clusters.Length; i++)
                        if (clusters[i] == fromCluster)
                            clusters[i] = toCluster;

                    clusterSizes[toCluster] += clusterSizes[fromCluster];
                    clusterSizes[fromCluster] = 0;
                }
            }

            // Now add them all up!
            Array.Sort(clusterSizes);
            long clusterResult = 1;
            for (var i = 0; i < 3; i++)
                clusterResult *= clusterSizes[clusterSizes.Length - 1 - i];

            return clusterResult.ToString();
        }
    }
}
