using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day07_1 : IAssignment
    {
        private int SimulateBeam(CharGrid grid, Point beam)
        {
            var splitCount = 0;

            while (beam.y < grid.Height)
            {
                var chr = grid[beam];
                if (chr == '^')
                {
                    // Splitter detected!
                    splitCount++;
                    grid[beam] = 'v';

                    if (beam.x > 0)
                        splitCount += SimulateBeam(grid, beam + Point.West);
                    if (beam.x < (grid.Width - 1))
                        splitCount += SimulateBeam(grid, beam + Point.East);
                    return splitCount;
                }
                else if (chr == 'v')
                    return splitCount;
                else
                    grid[beam] = '|';
                beam.y++;
            }

            Debug.Assert(splitCount == 0);
            return splitCount;
        }

        public string Run(IReadOnlyList<string> input)
        {
            var grid = new CharGrid(input);
            var start = grid.Find('S');
            //Logger.DebugLine(grid.ToString());
            var splitCount = SimulateBeam(grid, start);
            //Logger.DebugLine(grid.ToString());
            return splitCount.ToString();
        }
    }
}
