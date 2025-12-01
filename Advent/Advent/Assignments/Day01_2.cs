using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day01_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            const int DialSize = 100;
            var dial = 50;
            var zeroCount = 0;
            foreach (var item in input)
            {
                var num = int.Parse(item[1..]);
                var fullCircles = num / DialSize;
                zeroCount += fullCircles;

                num %= DialSize;

                var dir = item[0] == 'L' ? DialSize - 1 : 1;

                // fuck it i'm tired
                var asf = 0;
                for (var click = 0; click < num; click++)
                {
                    dial = (dial + dir) % DialSize;

                    if (dial == 0)
                        asf++;
                }

                //Logger.DebugLine($"The dial is rotated {item} to the point at {dial}; during this rotation, it points at 0 {asf}");

                zeroCount += asf;
            }

            return zeroCount.ToString();
        }
    }
}
