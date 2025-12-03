using System.Collections.Generic;

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
                for (var i = 0; i < highestJoltage.Length; i++)
                    highestJoltage[i] = '0';
                
                for (var bankIndex = 0; bankIndex < bank.Length; bankIndex++)
                {
                    var batteriesIncludingThisOne = bank.Length - bankIndex;
                    var batteriesToSkip = BatteryCount - batteriesIncludingThisOne;
                    if (batteriesToSkip < 0)
                        batteriesToSkip = 0;

                    //Logger.DebugLine($"[{bank}]");
                    //Logger.DebugLine($"[{("^".PadLeft(bankIndex + 1).PadRight(bank.Length))}]");
                    //Logger.DebugLine($"<{highestJoltage.ToNonSeparatedString()}>");

                    for (var batteryIndex = 0; batteryIndex < BatteryCount - batteriesToSkip; batteryIndex++)
                    {
                        var joltage = bank[bankIndex + batteryIndex];

                        if (joltage > highestJoltage[batteriesToSkip + batteryIndex])
                        {
                            //Logger.DebugLine($"[{batteryNumber}] {joltage} > {highestJoltage[batteryNumber]}");
                            highestJoltage[batteriesToSkip + batteryIndex] = joltage;
                            for (var k = batteryIndex + 1; k < BatteryCount; k++)
                            {
                                highestJoltage[k] = bank[bankIndex + k];
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
