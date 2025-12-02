using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day02_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var ranges = input[0].Split(',');
            var invalidIdSum = 0L;

            foreach (var range in ranges)
            {
                var parts = range.Split('-');
                var rangeStart = long.Parse(parts[0], System.Globalization.NumberStyles.None);
                var rangeEnd = long.Parse(parts[1], System.Globalization.NumberStyles.None);

                for (var id = rangeStart; id <= rangeEnd; id++)
                {
                    var digits = (long)Math.Log10(id) + 1;
                    if ((digits & 1) != 0)
                    {
                        // Odd, cannot consist of two pairs of identical digits
                        continue;
                    }

                    var halfDigits = digits / 2;
                    var factor = (long)Math.Pow(10, halfDigits);
                    var upper = id / factor;
                    var lower = id % factor;
                    if (upper == lower)
                        invalidIdSum += id;
                }
            }

            return invalidIdSum.ToString();
        }
    }
}
