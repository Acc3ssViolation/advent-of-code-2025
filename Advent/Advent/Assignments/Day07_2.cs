using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day07_2 : IAssignment
    {
        private static readonly Func<CharGrid, Point, long> SimulateBeamFast = Memoization.Memoized<CharGrid, Point, long>(SimulateBeam);

        private static long SimulateBeam(CharGrid grid, Point beam)
        {
            var timelines = 0L;

            while (beam.y < grid.Height)
            {
                var chr = grid[beam];
                if (chr == '^')
                {
                    // Splitter detected!
                    timelines++;

                    if (beam.x > 0)
                        timelines += SimulateBeamFast(grid, beam + Point.West);
                    if (beam.x < (grid.Width - 1))
                        timelines += SimulateBeamFast(grid, beam + Point.East);
                    return timelines;
                }
                else
                    grid[beam] = '|';
                beam.y++;
            }

            //Debug.Assert(timelines == 0);
            return timelines;
        }

        public string Run(IReadOnlyList<string> input)
        {
            var grid = new CharGrid(input);
            var start = grid.Find('S');
            //Logger.DebugLine(grid.ToString());
            var splitCount = SimulateBeamFast(grid, start) + 1;
            //Logger.DebugLine(grid.ToString());
            return splitCount.ToString();
        }
    }
}
