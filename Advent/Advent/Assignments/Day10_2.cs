using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Google.OrTools.LinearSolver;

namespace Advent.Assignments
{
    internal class Day10_2 : IAssignment
    {
        private record struct Machine(uint Lights, Memory<uint> Buttons, Memory<int> Joltages)
        {
            public override string ToString()
            {
                var sb = new StringBuilder();

                sb.Append('[');
                for (var bit = 0; bit < Joltages.Length; bit++)
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
            Span<Range> joltageParts = stackalloc Range[10];


            using var solver = Solver.CreateSolver("SCIP");
            Debug.Assert(solver != null);
            var totalButtonPresses = 0L;

            for (var inputIndex = 0; inputIndex < input.Count; inputIndex++)
            {
                var line = input[inputIndex];
                var c = 0;

                // Skip over lights
                var rows = 0;
                {
                    while (line[c] != '(')
                    {
                        if (line[c] == '#' || line[c] == '.')
                            rows++;
                        c++;
                    }
                }

                // Parse buttons
                var columns = line.Count(c => c == '(');
                var rowData = new int[rows, columns];

                {
                    var col = 0;
                    while (true)
                    {
                        var chr = line[c];
                        if (chr == ')')
                        {
                            col++;
                        }
                        else if (chr >= '0' && chr <= '9')
                        {
                            var bit = chr - '0';
                            rowData[bit, col] = 1;
                        }
                        else if (chr == '{')
                        {
                            break;
                        }

                        c++;
                    }
                }

                // Parse joltages
                var joltages = new int[rows];
                {
                    c++;    // Skip {
                    var joltageString = line.AsSpan().Slice(c, line.Length - c - 1);
                    var joltageCount = joltageString.Split(joltageParts, ',');
                    for (var j = 0; j < joltageCount; j++)
                    {
                        var joltage = int.Parse(joltageString[joltageParts[j]], System.Globalization.NumberStyles.None);
                        joltages[j] = joltage;
                    }
                }

                var variablesToSolve = solver.MakeIntVarArray(columns, 0, double.PositiveInfinity);
                for (var row = 0; row < rows; row++)
                {
                    var value = joltages[row];
                    var constraint = solver.MakeConstraint(value, value, $"Constraint #{row}");
                    for (var col = 0; col < columns; col++)
                        constraint.SetCoefficient(variablesToSolve[col], rowData[row,col]);
                }
                var objective = solver.Objective();
                objective.SetMinimization();
                for (var col = 0; col < columns; col++)
                    objective.SetCoefficient(variablesToSolve[col], 1);
                var result = solver.Solve();
                Debug.Assert(result is Solver.ResultStatus.OPTIMAL or Solver.ResultStatus.FEASIBLE);
                Logger.DebugLine("==========================");
                Logger.DebugLine($"Objective: {objective.Value()}");
                for (var col = 0; col < columns; col++)
                    Logger.DebugLine($"X{col} = {variablesToSolve[col].SolutionValue()}");
                Logger.DebugLine("==========================");

                totalButtonPresses += (long)objective.Value();

                solver.Clear();
            }

            return totalButtonPresses.ToString();
        }
    }
}
