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

                var prevDial = dial;
                if (item[0] == 'L')
                {
                    // Turn LEFT aka the dial value decreases
                    dial = (dial + DialSize - num) % DialSize;

                    // If the dial value increased that means we crossed the zero
                    // Alternatively if we landed on zero we must also count it
                    if ((dial > prevDial || dial == 0) && prevDial != 0)
                        zeroCount++;
                }
                else
                {
                    // Turn RIGHT aka the dial value increases
                    dial = (dial + num) % DialSize;

                    // If the dial value decreased that means we crossed the zero (or ended on it)
                    if (dial < prevDial && prevDial != 0)
                        zeroCount++;
                }
            }

            return zeroCount.ToString();
        }
    }
}
