using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Advent.Assignments
{
    internal class Day09_2_Kompressor : IAssignment
    {
        public string InputFile => "day09.txt";
        public int Day => 9;
        public int Part => 2;

        const byte TileInside = 0;
        const byte TileRed = 1;
        const byte TileGreen = 2;
        const byte TileOutside = 3;
        const int GridOffset = 1;

        public string Run(IReadOnlyList<string> input)
        {
            var pointCount = input.Count;
            var points = new Point[pointCount];
            var xCoordinates = new int[pointCount];
            var yCoordinates = new int[pointCount];

            Span<Range> parts = stackalloc Range[2];
            for (var i  = 0; i < input.Count; i++)
            {
                var span = input[i].AsSpan();
                span.Split(parts, ',');
                var x = int.Parse(span[parts[0]], NumberStyles.None);
                var y = int.Parse(span[parts[1]], NumberStyles.None);
                points[i] = new Point(x, y);
                xCoordinates[i] = x;
                yCoordinates[i] = y;
            }

            Array.Sort(xCoordinates);
            Array.Sort(yCoordinates);

            var grid = new CompactGrid(xCoordinates.Length + GridOffset * 2, yCoordinates.Length + GridOffset * 2);

            // Compress coordinates and populate the grid with the corners
            for (var i = 0; i < pointCount; i++)
            {
                ref var point = ref points[i];
                point.x = Array.IndexOf(xCoordinates, point.x) + GridOffset;
                point.y = Array.IndexOf(yCoordinates, point.y) + GridOffset;
                grid[point] = TileRed;
            }

            // Build edges in the grid
            for (var i = 0; i < points.Length; i++)
            {
                var j = (i + 1) % points.Length;
                var point = points[i];
                var nextPoint = points[j];

                if (point.x == nextPoint.x)
                {
                    // Vertical line
                    int minY, maxY;
                    if (point.y < nextPoint.y)
                        (minY, maxY) = (point.y, nextPoint.y);
                    else
                        (minY, maxY) = (nextPoint.y, point.y);

                    for (var y = minY; y < maxY; y++)
                        grid[new Point(point.x, y)] = TileGreen;
                }
                else
                {
                    // Horizontal line
                    int minX, maxX;
                    if (point.x < nextPoint.x)
                        (minX, maxX) = (point.x, nextPoint.x);
                    else
                        (minX, maxX) = (nextPoint.x, point.x);

                    for (var x = minX; x < maxX; x++)
                        grid[new Point(x, point.y)] = TileGreen;
                }
            }

            // Flood fill
            grid.FloodFill(new Point(0, 0), TileOutside);

            //Logger.DebugLine(grid.ToString(['X', '#', 'X', '.']));

            // Now do the thing
            var biggestArea = 0L;
            for (var i = 0; i < points.Length; i++)
            {
                var fromPoint = points[i];

                for (var j = i + 1; j < points.Length; j++)
                {
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

                    // Calculate the original area
                    long dx = xCoordinates[xMax - GridOffset] - xCoordinates[xMin - GridOffset] + 1;
                    long dy = yCoordinates[yMax - GridOffset] - yCoordinates[yMin - GridOffset] + 1;
                    var area = dx * dy;

                    // Don't bother if the area is not a candidate anyway
                    if (area <= biggestArea)
                        continue;

                    // Note that (0,0) is the top left corner
                    // A --- D
                    // |     |
                    // C --- B

                    // A --- D
                    // C --- B
                    var intersectsEdge = false;
                    for (var x = xMin; x <= xMax; x++)
                    {
                        if (grid[new Point(x, yMin)] == TileOutside || grid[new Point(x, yMax)] == TileOutside)
                        {
                            intersectsEdge = true;
                            break;
                        }
                    }

                    // A    D
                    // |    |
                    // C    B
                    if (!intersectsEdge)
                    {
                        for (var y = yMin; y <= yMax; y++)
                        {
                            if (grid[new Point(xMin, y)] == TileOutside || grid[new Point(xMax, y)] == TileOutside)
                            {
                                intersectsEdge = true;
                                break;
                            }
                        }
                    }

                    if (!intersectsEdge)
                        biggestArea = area;
                }
            }

            return biggestArea.ToString();
        }
    }
}
