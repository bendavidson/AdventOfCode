using AdventOfCode.Algorithms.DijkstraCalculator;
using AdventOfCode.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day10 : DayBase
    {
        private GridHelper gridHelper = new GridHelper();
        
        public Day10() : base("2024", "Day10") { }

        protected override void Solve()
        {
            var grid = gridHelper.GetGridFromLines(lines);

            foreach(var node in grid)
            {
                // Only include neighbours that are +1 in value
                node.Neighbours = gridHelper.FindNeighbours(node, grid, false).Where(x => x.Value - node.Value == 1)
                                    .ToDictionary(x => x, x => new NeighbourAttributes { Direction = gridHelper.CalculateDirection(node, x) });
            }

            // Get all the trailheads that have a neighbour
            var trailHeads = grid.Where(n => n.Value == 0 && n.Neighbours.Any()).ToList();
            
            foreach(var node in trailHeads)
            {
                var trailQueue = new Queue<Node>();
                var visitedEnds = new HashSet<Node>();

                trailQueue.Enqueue(node);

                while(trailQueue.Count > 0)
                {
                    var nodeToProcess = trailQueue.Dequeue();
                    if(nodeToProcess.Neighbours.Any())
                    {
                        foreach (var item in nodeToProcess.Neighbours)
                        {
                            trailQueue.Enqueue(item.Key);
                        }
                    }
                    else if(nodeToProcess.Value == 9)
                    {
                        visitedEnds.Add(nodeToProcess);
                        if(partNo == 2)
                        {
                            total++;
                        }
                    }
                }

                if (partNo == 1)
                {
                    total = total + visitedEnds.Count;
                }
            }
        }
    }
}
