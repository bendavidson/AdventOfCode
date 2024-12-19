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

            if (partNo == 1)
            {
                total = visitedNodes.Count();
            }
            else
            {
                currentNode = visitedNodes.FirstOrDefault();
                var blockingNodes = new HashSet<Node>();                
                foreach(var node in visitedNodes.Skip(1))
                {
                    currentDirection = Direction.Up;

                    if (!blockingNodes.Contains(node))
                    {
                        node.Name = "#";

                        if (CheckForLoop(currentNode, node, currentDirection, nodes))
                        {
                            blockingNodes.Add(node);
                            Console.WriteLine(node.Coords.ToString());
                        }

                        node.Name = ".";
                    }

                    //currentNode = node;
                }

                total = blockingNodes.Count();
            }

        }

        private bool CheckForLoop(Node startNode, Node testNode, Direction startDirection, List<Node> nodes)
        {
            Node currentNode = startNode;
            Node nextNode = _gridHelper.GetNeighbour(currentNode, startDirection, nodes);
            Direction currentDirection = startDirection;
            HashSet<(Node,Direction)> visitedNodes = new HashSet<(Node,Direction)> ();
            int i = 0;

            visitedNodes.Add((currentNode, currentDirection));

            while(nextNode != null)
            {
                if (nextNode.Name == "#")
                {
                    currentDirection = _gridHelper.TurnRight(currentDirection);
                }
                else
                {
                    currentNode = nextNode;
                    visitedNodes.Add((currentNode, currentDirection));
                }

                nextNode = _gridHelper.GetNeighbour(currentNode, currentDirection, nodes);

                if(visitedNodes.Contains((nextNode, currentDirection)))
                {
                    return true;
                }

                i++;
            }

            return false;
        }
    }
}
