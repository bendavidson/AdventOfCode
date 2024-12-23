using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Algorithms.DijkstraCalculator
{
    internal class Graph
    {
        private List<Node> _nodes;
        public int NodeCount { get { return _nodes.Count; } }
        public List<Node> Nodes { get { return _nodes; } set { _nodes = value; } }

        public Graph()
        {
            _nodes = new List<Node>();
        }

        public void Add(Node n)
        {
            _nodes.Add(n);
        }

        public void Remove(Node n)
        {
            _nodes.Remove(n);
        }

        public List<Node> GetNodes()
        {
            return _nodes.ToList();
        }
    }
}
