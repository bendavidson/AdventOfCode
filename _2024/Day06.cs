using AdventOfCode.Algorithms.DijkstraCalculator;
using AdventOfCode.Enums;
using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day06 : DayBase
    {
        private GridHelper _gridHelper = new GridHelper();
        
        public Day06() : base("2024", "Day06") { }

        protected override void Solve()
        {
            int x = 0;
            int y = 0;

            var nodes = new List<Node>();

            foreach (var line in lines)
            {
                var lineArray = line.ToArray();

                for (y = 0; y < lineArray.Length; y++)
                {
                    nodes.Add(new Node(lineArray[y].ToString(), new Tuple<int, int>(x, y)));
                }
                x++;
            }

            HashSet<Node> visitedNodes = new HashSet<Node>();
            
            var currentNode = nodes.FirstOrDefault(n => n.Name == "^");
            Direction currentDirection = Direction.Up;

            visitedNodes.Add(currentNode);

            var nextNode = _gridHelper.GetNeighbour(currentNode, currentDirection, nodes);

            while (nextNode != null)
            {
                if (nextNode.Name == "#")
                {
                    currentDirection = _gridHelper.TurnRight(currentDirection);
                }
                else
                {
                    currentNode = nextNode;
                    visitedNodes.Add(currentNode);
                }

                nextNode = _gridHelper.GetNeighbour(currentNode, currentDirection, nodes);
            }

            total = visitedNodes.Count;

        }
    }
}
