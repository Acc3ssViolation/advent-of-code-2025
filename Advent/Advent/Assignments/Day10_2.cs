using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

using V = MathNet.Numerics.LinearAlgebra.Vector<float>;
using M = MathNet.Numerics.LinearAlgebra.Matrix<float>;

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
                var rowData = new V[rows];
                for (var r = 0; r < rows; r++)
                    rowData[r] = V.Build.Dense(columns + 1);

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
                            rowData[bit][col] = 1;
                        }
                        else if (chr == '{')
                        {
                            break;
                        }

                        c++;
                    }
                }

                // Parse joltages
                var duplicates = new HashSet<int>();
                var joltages = V.Build.Dense(rows);
                {
                    c++;    // Skip {
                    var joltageString = line.AsSpan().Slice(c, line.Length - c - 1);
                    var joltageCount = joltageString.Split(joltageParts, ',');
                    for (var j = 0; j < joltageCount; j++)
                    {
                        var joltage = int.Parse(joltageString[joltageParts[j]], System.Globalization.NumberStyles.None);
                        if (joltages.Any(j => joltage == j))
                            duplicates.Add(j);
                        else
                            joltages[j] = joltage;
                    }
                }

                // Do linear algebra. Buttons are columns, Joltages is the desired output vector. Button presses is the input vector.
                var matrix = M.Build.Dense(rows - duplicates.Count, columns + 1, 0);
                var rowIndex = 0;

                for (var row = 0; row < rows; row++)
                {
                    if (duplicates.Contains(row))
                        continue;
                    rowData[row][columns] = joltages[row];
                    matrix.SetRow(rowIndex++, rowData[row]);

                }

                Logger.DebugLine("==========================");
                Logger.DebugLine(line);
                Logger.DebugLine("==========================");
                Logger.DebugLine(matrix.ToString());
                GaussianElimination(matrix);
                Logger.DebugLine(matrix.ToString());
            }

            return totalButtonPresses.ToString();
        }

        public void GaussianElimination(M matrix)
        {
            var m = matrix.RowCount;
            var n = matrix.ColumnCount;

            var h = 0;
            var k = 0;
            while (h < m && k < n)
            {
                Logger.DebugLine($"[{h},{k}]\n{matrix}");

                var iMax = ArgMaxRow(matrix, h, m, k);
                if (matrix[iMax, k].AlmostEqual(0))
                {
                    // No pivot, move to next column
                    Logger.DebugLine($"No pivot in column {k}");
                    k++;
                }
                else
                {
                    // Swap rows h and iMax
                    Logger.DebugLine($"> Swapping rows {iMax} and {h}");
                    for (var column = 0; column < matrix.ColumnCount; column++)
                    {
                        (matrix[h, column], matrix[iMax, column]) = (matrix[iMax, column], matrix[h, column]);
                    }

                    // Loop through all rows below the pivot
                    for (var i = h + 1; i < m; i++)
                    {
                        var f = matrix[i, k] / matrix[h, k];
                        Logger.DebugLine($"> Setting [{i},{k}] to 0 (f = {f})");
                        matrix[i, k] = 0;
                        for (var j = k + 1; j < n; j++)
                            matrix[i, j] = matrix[i, j] - matrix[h, j] * f;
                    }

                    h++;
                    k++;
                }
            }
        }

        private int ArgMaxRow(M matrix, int h, int m, int k)
        {
            var iMax = -1;
            var valueMax = double.MinValue;
            for (var i = h; i < m; i++)
                if (matrix[i, k] > valueMax)
                {
                    valueMax = matrix[i, k];
                    iMax = i;
                }
            Debug.Assert(iMax >= 0);
            return iMax;
        }
    }
}
