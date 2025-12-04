using Advent.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Assignments
{
    internal class Day04_1 : IAssignment
    {
        public string Run(IReadOnlyList<string> input)
        {
            const int MaxNeighbourRolls = 4;

            var grid = new CharGrid(input);

            var accessibleRolls = 0;

            var dyMin = 0;

            for (var y = 0; y < grid.Height; y++)
            {
                var dxMin = 0;

                for (var x = 0; x < grid.Width; x++)
                {
                    if (grid[new Point(x, y)] != '@')
                        continue;

                    var neighbourRolls = 0;

                    var dxMax = (x + 1 < grid.Width) ? 1 : 0;
                    var dyMax = (y + 1 < grid.Height) ? 1 : 0;

                    for (var dx = dxMin; dx <= dxMax && (neighbourRolls < MaxNeighbourRolls); dx++)
                    {
                        for (var dy = dyMin; dy <= dyMax && (neighbourRolls < MaxNeighbourRolls); dy++)
                        {
                            if (dx != 0 || dy != 0)
                            {
                                var point = new Point(x + dx, y + dy);
                                if (grid[point] == '@')
                                    neighbourRolls++;
                            }
                        }
                    }

                    if (neighbourRolls < MaxNeighbourRolls)
                        accessibleRolls++;

                    dxMin = -1;
                }

                dyMin = 1;
            }

            return accessibleRolls.ToString();
        }
    }
}
