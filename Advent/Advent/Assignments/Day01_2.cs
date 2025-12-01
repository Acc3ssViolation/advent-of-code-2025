using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day01_2 : IAssignment
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

            //for (var p = 0; p < left.Count; p++)
            //{
            //    Logger.DebugLine($"[{p:D3}] {left[p]}    {right[p]}");
            //}

            var sum = 0;
            var i = 0;
            var k = 0;

            // Both lists are sorted, which we can use to move through both lists only once to find our answer
            while (i < left.Count)
            {
                var number = left[i];
                var count = 0;

                // Skip over any numbers in the right list that are lower than the current number.
                // We won't ever be seeing this number in the left list anymore due to the sorting, so we can skip them.
                while (k < right.Count && right[k] < number)
                {
                    k++;
                }

                // Find how many occurences there are of the current number in the right list.
                // Once we hit another number we know there are no more instances in the right list.
                while (k < right.Count && right[k] == number)
                {
                    k++;
                    count++;
                }

                // Count how many times the number happens in the left list, as we need to multiply that as well.
                // This also ensures we move through the left list normally.
                var iterations = 0;
                while (i < left.Count && left[i] == number)
                {
                    i++;
                    iterations++;
                }

                //Logger.DebugLine($"{number} (x{iterations}) appears {count} times, score {number * count} i={i} k={k}");
                sum += number * count * iterations;
            }

            return sum.ToString();
        }
    }
}
