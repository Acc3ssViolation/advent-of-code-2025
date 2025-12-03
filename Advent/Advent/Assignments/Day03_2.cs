using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day03_2 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            const int BatteryCount = 12;
            var joltageSum = 0L;

            char[] highestJoltage = new char[BatteryCount];

            foreach (var bank in input)
            {
                Array.Fill(highestJoltage, '0');
                
                for (var bankIndex = 0; bankIndex < bank.Length; bankIndex++)
                {
                    var joltage = bank[bankIndex];
                    var batteriesIncludingThisOne = bank.Length - bankIndex;
                    var batteriesToSkip = BatteryCount - batteriesIncludingThisOne;
                    if (batteriesToSkip < 0)
                        batteriesToSkip = 0;

                    //Logger.DebugLine($"[{bank}]");
                    //Logger.DebugLine($"[{("^".PadLeft(bankIndex + 1).PadRight(bank.Length))}]");
                    //Logger.DebugLine($"<{highestJoltage.ToNonSeparatedString()}>");

                    for (var batteryNumber = batteriesToSkip; batteryNumber < BatteryCount; batteryNumber++)
                    {
                        joltage = bank[bankIndex + batteryNumber - batteriesToSkip];

                        if (joltage > highestJoltage[batteryNumber])
                        {
                            //Logger.DebugLine($"[{batteryNumber}] {joltage} > {highestJoltage[batteryNumber]}");
                            highestJoltage[batteryNumber] = joltage;
                            for (var k = batteryNumber + 1; k < BatteryCount; k++)
                            {
                                highestJoltage[k] = bank[bankIndex + k - batteriesToSkip];
                            }
                            break;
                        }
                    }

                    //Logger.DebugLine($"<{highestJoltage.ToNonSeparatedString()}>");
                    //Logger.DebugLine("");
                }

                var combinedJoltage = 0L;
                for (var i = 0; i < BatteryCount; i++)
                {
                    combinedJoltage *= 10;
                    combinedJoltage += highestJoltage[i] - '0';
                }
                //Logger.DebugLine($"Bank '{bank}' has highest joltage {combinedJoltage}");
                joltageSum += combinedJoltage;
            }

            return joltageSum.ToString();
        }
    }
}
