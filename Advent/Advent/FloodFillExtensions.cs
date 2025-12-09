using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    internal static class FloodFillExtensions
    {
        public static void FloodFill(this CompactGrid grid, Point start, byte value)
        {
            var queue = new Queue<Point>();
            var replaceValue = grid[start];
            queue.Enqueue(start);
            while (queue.TryDequeue(out var point))
            {
                if (grid[point] != replaceValue)
                    continue;

                grid[point] = value;

                if (point.x > 0)
                    queue.Enqueue(point with { x = point.x - 1 });
                if (point.x < grid.Width - 1)
                    queue.Enqueue(point with { x = point.x + 1 });
                if (point.y > 0)
                    queue.Enqueue(point with { y = point.y - 1 });
                if (point.y < grid.Height - 1)
                    queue.Enqueue(point with { y = point.y + 1 });
            }
        }

        public static void FloodFillRecursive(this CompactGrid grid, Point start, byte value)
        {
            static void FloodFillRecursive(CompactGrid grid, Point point, byte replaceValue, byte value)
            {
                if (grid[point] != replaceValue)
                    return;
                grid[point] = value;

                if (point.x > 0)
                    FloodFillRecursive(grid, point with { x = point.x - 1 }, replaceValue, value);
                if (point.x < grid.Width - 1)
                    FloodFillRecursive(grid, point with { x = point.x + 1 }, replaceValue, value);
                if (point.y > 0)
                    FloodFillRecursive(grid, point with { y = point.y - 1 }, replaceValue, value);
                if (point.y < grid.Height - 1)
                    FloodFillRecursive(grid, point with { y = point.y + 1 }, replaceValue, value);
            }

            var replaceValue = grid[start];
            FloodFillRecursive(grid, start, replaceValue, value);
        }
    }
}
