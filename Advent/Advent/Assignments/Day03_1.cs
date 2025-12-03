using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day03_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            var joltageSum = 0;

            foreach (var bank in input)
            {
                char highestJoltage = '0';
                char nextHighestJoltage = '0';
                
                for (var battery = 0; battery < bank.Length - 1; battery++)
                {
                    if (bank[battery] > highestJoltage) 
                    { 
                        highestJoltage = bank[battery];
                        nextHighestJoltage = bank[battery + 1];
                    }
                    else if (bank[battery] > nextHighestJoltage)
                    {
                        nextHighestJoltage = bank[battery];
                    }
                }

                if (bank[^1] > nextHighestJoltage)
                    nextHighestJoltage = bank[^1];

                var combinedJoltage = (highestJoltage - '0') * 10 + (nextHighestJoltage - '0');
                //Logger.DebugLine($"Bank '{bank}' has highest joltage {combinedJoltage}");
                joltageSum += combinedJoltage;
            }

            return joltageSum.ToString();
        }
    }
}
