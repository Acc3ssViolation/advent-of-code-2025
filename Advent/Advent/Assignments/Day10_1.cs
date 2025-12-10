using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day10_1 : IAssignment
    {
        private record struct Machine(uint Lights, Memory<uint> Buttons, Memory<int> Joltages)
        {
            public override string ToString()
            {
                var sb = new StringBuilder();

                sb.Append('[');
                for (var bit = Joltages.Length; bit > 0; bit--)
                    if ((Lights & (1 << bit)) != 0)
                        sb.Append('#');
                    else 
                        sb.Append('.');
                sb.Append(']');

                for (var b = 0; b < Buttons.Length; b++)
                {
                    sb.Append(' ');
                    sb.Append('(');
                    var hasAdded = false;
                    for (var bit = 0; bit < 32; bit++)
                        if ((Buttons.Span[b] & (1 << bit)) != 0)
                        {
                            if (hasAdded)
                                sb.Append(',');
                            sb.Append((char)(bit + '0'));
                            hasAdded = true;
                        }

                    sb.Append(')');
                }

                sb.Append(' ');
                sb.Append('{');
                {
                    var hasAdded = false;
                    for (var j = 0; j < Joltages.Length; j++)
                    {
                        if (hasAdded)
                            sb.Append(',');
                        sb.Append(Joltages.Span[j]);
                        hasAdded = true;
                    }
                }
                sb.Append('}');

                return sb.ToString();
            }
        }

        public string Run(IReadOnlyList<string> input)
        {
            var machines = new Machine[input.Count];
            var buttons = new uint[10 * input.Count];
            var joltages = new int[10 * input.Count];
            var buttonsIndex = 0;
            var joltagesIndex = 0;
            Span<Range> joltageParts = stackalloc Range[10];

            for (var inputIndex = 0; inputIndex < input.Count; inputIndex++)
            {
                var line = input[inputIndex];
                var c = 0;

                // Parse lights
                var lightCount = 0;
                var lights = 0U;
                {
                    c++;    // Skip [
                    while (line[c] != ']')
                    {
                        if (line[c] == '#')
                            lights |= 1;
                        lights <<= 1;
                        lightCount++;

                        c++;
                    }
                    c++;    // skip ]
                }

                // Parse buttons
                var buttonCount = 0;
                {
                    var button = 0U;
                    while (true)
                    {
                        var chr = line[c];
                        if (chr == ')')
                        {
                            buttons[buttonsIndex + buttonCount] = button;
                            buttonCount++;
                            button = 0;
                        }
                        else if (chr >= '0' && chr <= '9')
                        {
                            var bit = chr - '0';
                            button |= 1u << bit;
                        }
                        else if (chr == '{')
                        {
                            break;
                        }

                        c++;
                    }
                }

                // Parse joltages
                c++;    // Skip {
                var joltageString = line.AsSpan().Slice(c, line.Length - c - 1);
                var joltageCount = joltageString.Split(joltageParts, ',');
                Debug.Assert(joltageCount == lightCount);
                for (var j = 0; j < joltageCount; j++)
                {
                    var joltage = int.Parse(joltageString[joltageParts[j]], System.Globalization.NumberStyles.None);
                    joltages[joltagesIndex + j] = joltage;
                }

                // Combine!
                ref var machine = ref machines[inputIndex];
                machine.Lights = lights;
                machine.Buttons = new Memory<uint>(buttons, buttonsIndex, buttonCount);
                machine.Joltages = new Memory<int>(joltages, joltagesIndex, joltageCount);

                // Next loop prep
                buttonsIndex += buttonCount;
                joltagesIndex += joltageCount;
            }

            return "";
        }
    }
}
