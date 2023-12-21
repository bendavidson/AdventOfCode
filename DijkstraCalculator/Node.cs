using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2023.Enums;

namespace AdventOfCode2023.DijkstraCalculator
{
    internal class Node
    {
        private string _name;
        private Tuple<int, int> _coords;
        private Dictionary<Node, NeighbourAttributes> _neighbours;

        public string Name { get { return _name; } }
        public Tuple<int,int> Coords { get { return _coords; } }
        public Dictionary<Node, NeighbourAttributes> Neighbours {  get { return _neighbours; } }
                
        private Node()
        {
            _neighbours = new Dictionary<Node, NeighbourAttributes>();
        }

        public Node(string nodeName) : this()
        {
            _name = nodeName;
        }

        public Node(Tuple<int,int> nodeCoords) : this()
        {
            _coords = nodeCoords;
        }

        public void AddNeighbour(Node n, int cost)
        {
            _neighbours.Add(n, new NeighbourAttributes { Cost = cost});
        }

        public void AddNeighbour(Node n, int cost, Direction direction)
        {
            _neighbours.Add(n, new NeighbourAttributes { Cost = cost, Direction = direction });
        }
    }
}
