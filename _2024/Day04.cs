using AdventOfCode.Algorithms.DijkstraCalculator;
using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day04 : DayBase
    {
        GridHelper _gridHelper = new GridHelper();
        
        public Day04() : base("2024", "Day04") { }

        protected override void Solve()
        {
            int x = 0;
            int y = 0;

            var nodes = new List<Node>();
            
            foreach(var line in lines)
            {
                var lineArray = line.ToArray();

                for(y = 0; y < lineArray.Length; y++)
                {
                    nodes.Add(new Node(lineArray[y].ToString(), new Tuple<int,int>(x,y)));
                }
                x++;
            }

            foreach(var node in nodes)
            {
                var neighbours = _gridHelper.FindNeighbours(node, nodes, true);

                foreach (var neighbour in neighbours)
                {
                    node.AddNeighbour(neighbour, 0, _gridHelper.CalculateDirection(node, neighbour));
                }
            }

            if (partNo == 1)
            {
                var potentialX = nodes.Where(n => n.Name == "X" && n.Neighbours.Any(m => m.Key.Name == "M")).ToList();

                total = potentialX.SelectMany(x => x.Neighbours.Where(m => m.Key.Name == "M" &&
                    m.Key.Neighbours.Any(a => a.Key.Name == "A" && a.Value.Direction == m.Value.Direction
                        && a.Key.Neighbours.Any(s => s.Key.Name == "S" && s.Value.Direction == m.Value.Direction)))
                    .Select(m => new { X = x, Direction = m.Value.Direction, M = m.Key })).ToList().Count;
            }
            else
            {
                total = nodes.Where(a => a.Name == "A" 
                                &&
                                (
                                    (a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.UpLeft).Key?.Name == "M"
                                    && a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.DownRight).Key?.Name == "S")
                                    ||
                                    (a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.UpLeft).Key?.Name == "S"
                                    && a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.DownRight).Key?.Name == "M")

                                )
                                &&
                                (
                                    (a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.UpRight).Key?.Name == "M"
                                    && a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.DownLeft).Key?.Name == "S")
                                    ||
                                    (a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.UpRight).Key?.Name == "S"
                                    && a.Neighbours.FirstOrDefault(n => n.Value.Direction == Enums.Direction.DownLeft).Key?.Name == "M")

                                )
                                ).ToList().Count;
            }
        }
    }
}
