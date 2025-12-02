using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day02_2 : IAssignment
    {
        private static readonly long[] Powers =
        [
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
        ];

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

                        var factor = Powers[partLength];
                        var lower = id % factor;

                        var all = true;
                        var tmp = id / factor;
                        for (var k = 0; k < i - 1 && all; k++)
                        {
                            var upper = tmp % factor;
                            tmp /= factor;
                            if (upper != lower)
                                all = false;
                        }

                        if (all)
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
