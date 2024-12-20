using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day08 : DayBase
    {
        private GridHelper gridHelper = new GridHelper();
        
        public Day08() : base("2024", "Day08"){}

        protected override void Solve()
        {
            var nodes = gridHelper.GetGridFromLines(lines);

            var maxXY = gridHelper.MaxXY(nodes);

            var signals = nodes.Where(n => n.Name != ".").Select(n => n.Name).Distinct().ToList();

            var antinodes = new HashSet<(int x, int y)>();
            
            foreach (var signal in signals)
            {
                var antennas = nodes.Where(n => n.Name == signal).ToList();

                var antennaPairs = antennas.SelectMany((f, i) => antennas.Skip(i + 1).Select(s => (f,s))).ToList();

                foreach (var pair in antennaPairs)
                {
                    var diffX = pair.s.Coords.Item1 - pair.f.Coords.Item1;
                    var diffY = pair.s.Coords.Item2 - pair.f.Coords.Item2;
                    var antinode = (pair.f.Coords.Item1 - diffX, pair.f.Coords.Item2 - diffY);

                    while (gridHelper.IsInGrid(antinode, maxXY))
                    {
                        antinodes.Add((antinode.Item1, antinode.Item2));

                        if(partNo == 1)
                        {
                            break;
                        }
                        else
                        {
                            antinode = (antinode.Item1 - diffX, antinode.Item2 - diffY);
                        }
                    }

                    diffX = pair.f.Coords.Item1 - pair.s.Coords.Item1;
                    diffY = pair.f.Coords.Item2 - pair.s.Coords.Item2;

                    antinode = (pair.s.Coords.Item1 - diffX, pair.s.Coords.Item2 - diffY);

                    while (gridHelper.IsInGrid(antinode, maxXY))
                    {
                        antinodes.Add((antinode.Item1, antinode.Item2));

                        if (partNo == 1)
                        {
                            break;
                        }
                        else
                        {
                            antinode = (antinode.Item1 - diffX, antinode.Item2 - diffY);
                        }
                    }

                    if (partNo == 2)
                    {
                        antinodes.Add((pair.f.Coords.Item1, pair.f.Coords.Item2));
                        antinodes.Add((pair.s.Coords.Item1, pair.s.Coords.Item2));
                    }
                }
            }

            total = antinodes.Count();
        }
    }
}
