using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day09_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var points = new Point[input.Count];
            Span<Range> parts = stackalloc Range[2];
            var biggestArea = 0L;
            for (var i  = 0; i < input.Count; i++)
            {
                var span = input[i].AsSpan();
                span.Split(parts, ',');
                var x = int.Parse(span[parts[0]], NumberStyles.None);
                var y = int.Parse(span[parts[1]], NumberStyles.None);
                points[i] = new Point(x, y);

                for (var j = 0; j < i; j++)
                {
                    long dx = Math.Abs(x - points[j].x) + 1;
                    long dy = Math.Abs(y - points[j].y) + 1;
                    var area = dx * dy;

                    if (area >  biggestArea)
                        biggestArea = area;
                }
            }

            return biggestArea.ToString();
        }
    }
}
