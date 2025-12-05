using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day05_1 : IAssignment
    {
        private record Range(long Start, long End)
        {
            public bool Overlaps(long value)
                => Start <= value && value <= End;
        }

        public string Run(IReadOnlyList<string> input)
        {
            var freshIds = 0;

            var ranges = new List<Range>();
            var i = 0;
            // Build the list of ranges
            Span<System.Range> parts = stackalloc System.Range[2];
            for (i = 0; i < input.Count; i++)
            {
                var line = input[i];
                if (line.Length == 0)
                    break;

                var span = line.AsSpan();
                span.Split(parts, '-');
                var start = long.Parse(span[parts[0]], NumberStyles.None);
                var end = long.Parse(span[parts[1]], NumberStyles.None);
                ranges.Add(new Range(start, end));
            }
            // Skip empty line
            i++;
            // Process IDs
            for (; i < input.Count; i++)
            {
                var id = long.Parse(input[i], NumberStyles.None);
                for (var r = 0; r < ranges.Count; r++)
                {
                    if (ranges[r].Overlaps(id))
                    {
                        //Logger.DebugLine($"ID {id} overlaps range {ranges[r]}, so it is fresh!");
                        freshIds++;
                        break;
                    }
                }
            }

            return freshIds.ToString();
        }
    }
}
