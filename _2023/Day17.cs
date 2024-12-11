using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Algorithms.DijkstraCalculator;
using AdventOfCode.Enums;
using AdventOfCode.Helpers;

namespace AdventOfCode._2023
{
    internal class Day17 : DayBase
    {
        GridHelper _gridHelper = new GridHelper();

        public Day17() : base("2023", "Day17") { }

        protected override void Solve()
        {
            List<Block> blocks = new List<Block>();

            int x = 0;
            foreach (var line in lines)
            {
                for (int y = 0; y < line.Length; y++)
                {
                    blocks.Add(new Block { Position = new Tuple<int, int>(x, y), HeatLoss = Convert.ToInt32(line[y].ToString()) });
                }

                x++;
            }

            Graph graph = new Graph();

            // Add the nodes
            foreach (var block in blocks)
            {
                Node blockNode = new Node(block.Position);
                graph.Add(blockNode);
            }

            Node firstNode = null, lastNode = null;

            // Add the neighbour for each node
            foreach (var blockNode in graph.Nodes)
            {
                if (firstNode == null)
                    firstNode = blockNode;
                else
                    lastNode = blockNode;

                var neighbours = graph.Nodes.Where(b =>
                    b.Coords.Item1 >= blockNode.Coords.Item1 - 1
                    && b.Coords.Item1 <= blockNode.Coords.Item1 + 1
                    && b.Coords.Item2 >= blockNode.Coords.Item2 - 1
                    && b.Coords.Item2 <= blockNode.Coords.Item2 + 1
                    && b != blockNode
                    && (b.Coords.Item1 == blockNode.Coords.Item1 || b.Coords.Item2 == blockNode.Coords.Item2)
                    )
                    .Join(blocks, n => n.Coords, b => b.Position, (n, b) => new { n, b.HeatLoss });

                foreach (var neighbor in neighbours)
                {
                    blockNode.AddNeighbour(neighbor.n, neighbor.HeatLoss, _gridHelper.CalculateDirection(blockNode, neighbor.n));
                }
            }
            //lastNode = graph.Nodes.FirstOrDefault(n => n.Coords.Equals(new Tuple<int, int>(0, 5)));

            DistanceCalculator c = new DistanceCalculator(graph);
            if (partNo == 1)
                c.Calculate(firstNode, lastNode, 3);
            else
                c.Calculate(firstNode, lastNode, 10, 4);

            var routeSteps = c.RouteSteps;

            var routeMap = from b in blocks
                           join r in routeSteps
                           on b.Position equals r.Item1.Coords into rb
                           from ra in rb.DefaultIfEmpty()
                           select new { b.Position, Code = _gridHelper.DirectionDisplay(ra?.Item2) ?? "#" };

            for (x = 0; x < lines.Count; x++)
            {
                Console.WriteLine(string.Join("", routeMap.Where(b => b.Position.Item1 == x).Select(b => b.Code)));
            }
            total = c.RouteDistance;
        }



        class Block
        {
            public Tuple<int, int> Position { get; set; }
            public int HeatLoss { get; set; }
        }
    }
}
