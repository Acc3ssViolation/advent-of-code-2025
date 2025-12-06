using System.Collections.Generic;

namespace Advent.Assignments
{
    internal class Day06_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var row = input[0].ExtractInts();
            var assignmentCount = row.Count;
            var argumentCount = input.Count - 1;
            var numbers = new int[assignmentCount * argumentCount];

            for (var k = 0; k < row.Count; k++)
                numbers[k * argumentCount + 0] = row[k];

            for (var i = 1; i < input.Count - 1; i++)
            {
                row = input[i].ExtractInts();
                for (var k = 0; k < row.Count; k++)
                    numbers[k * argumentCount + i] = row[k];
            }

            long total = 0;
            var assignmentIndex = 0;
            foreach (var chr in input[^1])
            {
                long acc = 0;
                if (chr == '+')
                {
                    for (var n = 0; n < argumentCount; n++)
                    {
                        var arg = numbers[assignmentIndex * argumentCount + n];
                        acc += arg;
                    }
                    assignmentIndex++;
                }
                else if (chr == '*')
                {
                    acc = 1;
                    for (var n = 0; n < argumentCount; n++)
                    {
                        var arg = numbers[assignmentIndex * argumentCount + n];
                        acc *= arg;
                    }
                    assignmentIndex++;
                }
                total += acc;
            }

            return total.ToString();
        }
    }
}
