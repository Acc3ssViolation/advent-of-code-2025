using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day09_2 : IAssignment
    {
        private record struct Edge(int Coord, int Start, int End) : IComparable<Edge>
        {
            public readonly int CompareTo(Edge other)
            {
                return Coord.CompareTo(other.Coord);
            }

            public readonly bool Overlaps(int value)
                => value >= Start && value <= End;
        }

        private enum Turn
        {
            Left, 
            Right,
        }

        public string Run(IReadOnlyList<string> input)
        {
            var points = new Point[input.Count];
            //var grid = new CharGrid(200, 200);
            //for (var i = 0; i < grid.Chars.Length; i++)
            //    grid.Chars[i] = '.';

            var verticalEdges = new FixedList<Edge>(new Edge[input.Count]);
            var horizontalEdges = new FixedList<Edge>(new Edge[input.Count]);

            Span<Range> parts = stackalloc Range[2];
            for (var i  = 0; i < input.Count; i++)
            {
                var span = input[i].AsSpan();
                span.Split(parts, ',');
                var x = int.Parse(span[parts[0]], NumberStyles.None) * 10;
                var y = int.Parse(span[parts[1]], NumberStyles.None) * 10;
                points[i] = new Point(x, y);

                //grid[points[i]] = '#';
            }

            static Turn DetectTurn(Point[] points, int i)
            {
                var n = (i + 1) % points.Length;
                var p = (i + points.Length - 1) % points.Length;

                var point = points[i];
                var nextPoint = points[n];
                var prevPoint = points[p];

                // # ->
                // |
                if (prevPoint.y > point.y && nextPoint.x > point.x)
                    return Turn.Right;
                // <- #
                //    |
                if (prevPoint.y > point.y && nextPoint.x < point.x)
                    return Turn.Left;
                // |
                // # ->
                if (prevPoint.y < point.y && nextPoint.x > point.x)
                    return Turn.Left;
                //    |
                // <- #
                if (prevPoint.y < point.y && nextPoint.x < point.x)
                    return Turn.Right;

                // # -<
                // |
                if (prevPoint.x > point.x && nextPoint.y > point.y)
                    return Turn.Left;
                // >- #
                //    |
                if (prevPoint.x < point.x && nextPoint.y > point.y)
                    return Turn.Right;
                // |
                // # -<
                if (prevPoint.x > point.x && nextPoint.y < point.y)
                    return Turn.Right;
                //    |
                // >- #
                if (prevPoint.x < point.x && nextPoint.y < point.y)
                    return Turn.Left;

                Debug.Fail("Not possible");
                return Turn.Right;
            }

            var offsets = new Point[]
            {
                new Point(-5, -5),
                new Point(5, -5),
                new Point(5, 5),
                new Point(-5, 5),
            };
            // TODO: Autodetect this for the initial turn
            var prevTurn = DetectTurn(points, points.Length - 1);
            // Set to 1 for assignment, 0 for demo, should make this autodetect...
            var direction = points.Length > 20 ? 1 : 0;

            var offsetPoints = new Point[points.Length];

            for (var i = 0; i < points.Length; i++)
            {
                var turn = DetectTurn(points, i);
                if (turn == prevTurn)
                {
                    // Rotate!
                    if (turn == Turn.Left)
                        direction = (direction + 3) % 4;
                    else
                        direction = (direction + 1) % 4;
                }
                prevTurn = turn;

                offsetPoints[i] = points[i] + offsets[direction];
                //grid[offsetPoints[i]] = 'O';
            }


            // Build edge lists

            for (var i = 0; i < points.Length; i++)
            {
                var j = (i + 1) % points.Length;
                var point = offsetPoints[i];
                var nextPoint = offsetPoints[j];

                if (point.x == nextPoint.x)
                {
                    // Vertical line
                    int minY, maxY;
                    if (point.y < nextPoint.y)
                        (minY, maxY) = (point.y, nextPoint.y);
                    else
                        (minY, maxY) = (nextPoint.y, point.y);

                    verticalEdges.Add(new Edge(point.x, minY, maxY));
                }
                else
                {
                    // Horizontal line
                    int minX, maxX;
                    if (point.x < nextPoint.x)
                        (minX, maxX) = (point.x, nextPoint.x);
                    else
                        (minX, maxX) = (nextPoint.x, point.x);

                    horizontalEdges.Add(new Edge(point.y, minX, maxX));
                }
            }

            verticalEdges.Sort();
            horizontalEdges.Sort();

            //foreach (var edges in verticalEdges)
            //    foreach (var edge in edges.Value)
            //        for (var y = edge.Start + 1; y < edge.End; y++)
            //            grid[new Point(edges.Key, y)] = 'V';

            //foreach (var edges in horizontalEdges)
            //    foreach (var edge in edges.Value)
            //        for (var x = edge.Start + 1; x < edge.End; x++)
            //            grid[new Point(x, edges.Key)] = 'H';

            //Logger.DebugLine(grid.ToString());

            // Now do the thing
            var biggestArea = 0L;
            for (var i = 0; i < points.Length; i++)
            {
                for (var j = i + 1; j < points.Length; j++)
                {
                    var fromPoint = points[i];
                    var toPoint = points[j];

                    // Get bounding box of the area
                    int xMin, xMax, yMin, yMax;

                    if (fromPoint.x < toPoint.x)
                        (xMin, xMax) = (fromPoint.x, toPoint.x);
                    else
                        (xMin, xMax) = (toPoint.x, fromPoint.x);

                    if (fromPoint.y < toPoint.y)
                        (yMin, yMax) = (fromPoint.y, toPoint.y);
                    else
                        (yMin, yMax) = (toPoint.y, fromPoint.y);

                    // Note that (0,0) is the top left corner
                    // A --- D
                    // |     |
                    // C --- B

                    static bool DoThing(FixedList<Edge> edges, int axisMin, int axisMax, int min, int max)
                    {
                        // Find first edge
                        var i = 0;
                        for (; i < edges.Count; i++)
                            if (edges[i].Coord >= axisMin)
                                break;

                        // If we got through all edges we didn't find any
                        if (i == edges.Count)
                            return true;

                        // Now check every axis coordinate for overlap
                        while (i < edges.Count && edges[i].Coord < axisMax)
                        {
                            if (edges[i].Overlaps(min) || edges[i].Overlaps(max))
                                return false;
                            i++;
                        }

                        // No overlap
                        return true;
                    }

                    var isValid = true;
                    // A --- D
                    // C --- B

                    {
                        isValid = DoThing(verticalEdges, xMin, xMax, yMin, yMax);
                    }
                    // A    D
                    // |    |
                    // C    B
                    {
                        isValid &= DoThing(horizontalEdges, yMin, yMax, xMin, xMax);
                    }

                    if (isValid)
                    {
                        // This can work!
                        long dx = (xMax / 10) - (xMin / 10) + 1;
                        long dy = (yMax / 10) - (yMin / 10) + 1;
                        var area = dx * dy;

                        //Logger.DebugLine($"{fromPoint} to {toPoint} has area {area}");

                        if (area > biggestArea)
                            biggestArea = area;
                    }
                    else
                    {
                        //Logger.WarningLine($"{fromPoint} to {toPoint} goes outside the shape");
                    }
                }
            }

            return biggestArea.ToString();
        }
    }
}
