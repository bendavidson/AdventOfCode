using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Enums;
using AdventOfCode.Helpers;

namespace AdventOfCode.Algorithms.DijkstraCalculator
{
    internal class DistanceCalculator
    {
        private GridHelper _gridHelper = new GridHelper();

        private Graph _graph;
        private List<Node> _allNodes;
        private List<Tuple<Node, Direction>> _routeSteps;
        private PriorityQueue<Tuple<Node, Direction, int>, int> _nodesToProcess;
        private Dictionary<Tuple<Node, Direction, int>, int> _visitedNodes;

        public int RouteDistance { get; private set; }
        public List<Tuple<Node, Direction>> RouteSteps { get { return _routeSteps; } }

        public DistanceCalculator(Graph graph)
        {
            _graph = graph;
            _allNodes = graph.GetNodes();
            _nodesToProcess = new PriorityQueue<Tuple<Node, Direction, int>, int>();
            _visitedNodes = new Dictionary<Tuple<Node, Direction, int>, int>();
        }

        public void Calculate(Node source, Node destination, int maxStraight = int.MaxValue, int minStraight = 0)
        {
            _nodesToProcess.Enqueue(new Tuple<Node, Direction, int>(source, Direction.Default, 0), 0);

            Tuple<Node, Direction, int> nodeToProcess = null;
            int cost = 0;

            while (_nodesToProcess.TryDequeue(out nodeToProcess, out cost))
            {
                if (nodeToProcess.Item1 == destination)
                    break;

                ProcessNode(nodeToProcess, cost, maxStraight, minStraight);
            }

            RouteDistance = cost;
            _routeSteps = new List<Tuple<Node, Direction>>();
            SetRouteSteps(nodeToProcess, source);
        }

        private void ProcessNode(Tuple<Node, Direction, int> nodeToProcess, int cost, int maxStraight, int minStraight)
        {
            foreach (var neighbour in nodeToProcess.Item1.Neighbours)
            {
                if (
                    neighbour.Value.Direction == _gridHelper.Opposite(nodeToProcess.Item2)
                    || neighbour.Value.Direction == nodeToProcess.Item2 && nodeToProcess.Item3 == maxStraight
                    || neighbour.Value.Direction != nodeToProcess.Item2 && nodeToProcess.Item3 < minStraight && nodeToProcess.Item2 != Direction.Default
                    )
                    continue;
                else
                {
                    var nodeToAdd = new Tuple<Node, Direction, int>
                    (
                        neighbour.Key,
                        neighbour.Value.Direction,
                        neighbour.Value.Direction == nodeToProcess.Item2 ? nodeToProcess.Item3 + 1 : 1
                    );

                    if (!_visitedNodes.ContainsKey(nodeToAdd))
                    {
                        int vistCost = cost + neighbour.Value.Cost;
                        _nodesToProcess.Enqueue(nodeToAdd, vistCost);
                        _visitedNodes.Add(nodeToAdd, vistCost);
                    }
                }
            }
        }

        private void SetRouteSteps(Tuple<Node, Direction, int> node, Node source)
        {
            if (!_visitedNodes.ContainsKey(node))
                return;

            var nodeVisit = _visitedNodes[node];

            _routeSteps.Add(new Tuple<Node, Direction>(node.Item1, node.Item2));

            var previousNode = node.Item1.Neighbours.FirstOrDefault(n => n.Value.Direction == _gridHelper.Opposite(node.Item2));

            if (previousNode.Key.Equals(source))
                return;

            var previousNodeVisit = _visitedNodes.Where(
                v => v.Key.Item1 == previousNode.Key
                && v.Key.Item2 != _gridHelper.Opposite(node.Item2)
                && (v.Key.Item3 == node.Item3 - 1 && v.Key.Item2 == node.Item2
                    || v.Key.Item2 != node.Item2)).ToList().OrderBy(v => v.Value).FirstOrDefault();

            if (previousNodeVisit.Key != null)
                SetRouteSteps(previousNodeVisit.Key, source);
        }

        private Dictionary<Node, Tuple<Node, Direction>> SetRoutes()
        {
            var routes = new Dictionary<Node, Tuple<Node, Direction>>();

            foreach (var node in _allNodes)
            {
                routes.Add(node, null);
            }

            return routes;
        }
    }
}
