using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day12_1 : IAssignment
    {
        private record Present(CharGrid Shape, int Area)
        {
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append("Area: ");
                sb.Append(Area);
                sb.AppendLine();
                sb.AppendLine(Shape.ToString());
                return sb.ToString();
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var presents = new List<Present>(6);
            var i = 0;
            for (i = 0; i < input.Count; i += 5)
            {
                var line = input[i];
                if (!line.EndsWith(':'))
                    break;

                var shape = new CharGrid(3, 3);
                for (var y = 0; y < shape.Height; y++)
                    for (var x = 0; x < shape.Width; x++)
                        shape[new Point(x, y)] = input[i + 1 + y][x];

                var area = shape.Count('#');
                presents.Add(new Present(shape, (int)area));
                Logger.DebugLine(presents[^1].ToString());
            }

            var okRegions = 0;
            var potentialRegions = 0;
            var invalidRegions = 0;
            for (; i < input.Count; i++)
            {
                var parts = input[i].Split(' ');
                var areaSizes = parts[0].ExtractInts();
                var width = areaSizes[0];
                var height = areaSizes[1];

                var shapeCounts = parts.Skip(1).Select(v => int.Parse(v)).ToArray();
                var shapeAreaSum = shapeCounts.Select((count, index) => presents[index].Area * count).Sum();
                var shapeAreaMax = shapeCounts.Sum() * 9;
                var regionArea = width * height;
                Logger.DebugLine($"Area {width}x{height} => {shapeCounts.AggregateString()}");
                if (shapeAreaSum > regionArea)
                {
                    invalidRegions++;
                    Logger.DebugLine("> Region has smaller area than combined presents!");
                    continue;
                }

                if (shapeAreaMax <= regionArea)
                {
                    okRegions++;
                    Logger.DebugLine("> Region can always fit all presents!");
                    continue;
                }
                
                potentialRegions++;
            }

            Logger.DebugLine($"Potential regions: {potentialRegions}");
            Logger.DebugLine($"Invalid regions: {invalidRegions}");
            Logger.DebugLine($"OK regions: {okRegions}");

            return okRegions.ToString();
        }
    }
}
