using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Algorithms.DijkstraCalculator
{
    internal class Edge
    {
        public Node Source { get; set; }
        public Node Destination { get; set; }
        public int MovesInStraightLine { get; set; }
    }
}
