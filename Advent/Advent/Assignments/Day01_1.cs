using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.Assignments
{
    internal class Day01_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            const int DialSize = 100;
            var dial = 50;
            var zeroCount = 0;
            foreach (var item in input)
            {
                var num = int.Parse(item[1..]) % DialSize;
                if (item[0] == 'L')
                    num = DialSize - num;
                Debug.Assert(num >= 0);
                Debug.Assert(num <= DialSize);
                dial = (dial + num) % DialSize;
                if (dial == 0)
                    zeroCount++;
            }

            return zeroCount.ToString();
        }
    }
}
