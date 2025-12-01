using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day01_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var left = new List<int>(input.Count);
            var right = new List<int>(input.Count);

            foreach (var item in input)
            {
                var numbers = item.ExtractInts();
                left.Add(numbers[0]);
                right.Add(numbers[1]);
            }

            left.Sort();
            right.Sort();

            Debug.Assert(left.Count == right.Count);

            var sum = 0;
            for (var i = 0; i < left.Count; i++)
            {
                var delta = Math.Abs(left[i] - right[i]);
                sum += delta;
            }

            return sum.ToString();
        }
    }
}
