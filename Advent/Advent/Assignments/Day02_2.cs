using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day02_2 : IAssignment
    {
        private static bool IsRepeatingNumber(long number)
        {
            var digits = (long)Math.Log10(number) + 1;
            if ((digits & 1) != 0)
            {
                // Cannot contain two identical parts if odd
                return false;
            }

            var halfDigits = digits / 2;
            var factor = (long)Math.Pow(10, halfDigits);
            var upper = number / factor;
            var lower = number % factor;
            return upper == lower;
        }

        public string Run(IReadOnlyList<string> input)
        {
            var ranges = input[0].Split(',');
            var invalidIdSum = 0L;

            foreach (var range in ranges)
            {
                var rangeParts = range.Split('-');
                var rangeStart = long.Parse(rangeParts[0], System.Globalization.NumberStyles.None);
                var rangeEnd = long.Parse(rangeParts[1], System.Globalization.NumberStyles.None);

                for (var id = rangeStart; id <= rangeEnd; id++)
                {
                    var idString = id.ToString();

                    for (var i = 2; i <= idString.Length; i++)
                    {
                        var partLength = idString.Length / i;
                        if (partLength * i != idString.Length)
                            continue;

                        var parts = idString.Chunk(partLength).ToList();
                        if (parts.All(p => p.SequenceEqual(parts[0])))
                        {
                            //Logger.DebugLine($"{rangeStart}-{rangeEnd} has invalid ID {id}");
                            invalidIdSum += id;
                            break;
                        }
                    }
                }
            }

            return invalidIdSum.ToString();
        }
    }
}
