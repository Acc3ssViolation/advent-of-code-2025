using System.Collections.Generic;
using System.Linq;

namespace Advent.Assignments
{
    internal class Day06_2 : IAssignment
    {
        private enum Operation
        {
            None,
            Add,
            Multiply,
        }

        public string Run(IReadOnlyList<string> input)
        {
            // Parsing the input into lines before feeding them into an IAssingment is kind of annoying here,
            // as we have to turn it back into a single array and transpose it... oops
            var width = input.Count;
            var height = input[0].Length;
            var matrix = new char[height * width];

            for (var i = 0; i < input.Count; i++)
            {
                var line = input[i];
                for (var j = line.Length - 1; j >= 0; j--)
                {
                    matrix[(line.Length - 1 - j) * width + i] = line[j];
                }
            }

            var total = 0L;
            var stack = new Stack<int>();
            for (var y = 0; y < height; y++)
            {
                var num = 0;
                var op = Operation.None;

                for (var x = 0; x < width; x++)
                {
                    var chr = matrix[y * width + x];
                    if (chr >= '0' && chr <= '9')
                    {
                        var value = chr - '0';
                        num *= 10;
                        num += value;
                    }
                    else if (chr == '+')
                        op = Operation.Add;
                    else if (chr == '*')
                        op = Operation.Multiply;
                }

                if (num != 0)
                    stack.Push(num);

                switch (op)
                {
                    case Operation.Add:
                        {
                            var acc = 0L;
                            while (stack.TryPop(out var arg))
                                acc += arg;
                            total += acc;
                        }
                        break;
                    case Operation.Multiply:
                        {
                            var acc = 1L;
                            while (stack.TryPop(out var arg))
                                acc *= arg;
                            total += acc;
                        }
                        break;
                }
            }

            return total.ToString();
        }
    }
}
