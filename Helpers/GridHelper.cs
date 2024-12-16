using AdventOfCode.Algorithms.DijkstraCalculator;
using AdventOfCode.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Helpers
{
    internal class GridHelper
    {
        public Direction CalculateDirection(Node source, Node destination)
        {
            if (source.Coords.Item1 == destination.Coords.Item1)
            {
                if (source.Coords.Item2 + 1 == destination.Coords.Item2)
                    return Direction.Right;
                else
                    return Direction.Left;
            }

            if (source.Coords.Item2 == destination.Coords.Item2)
            {
                if (source.Coords.Item1 + 1 == destination.Coords.Item1)
                    return Direction.Down;
                else
                    return Direction.Up;
            }

            if(source.Coords.Item1 < destination.Coords.Item1)
            {
                if (source.Coords.Item2 + 1 == destination.Coords.Item2)
                    return Direction.DownRight;
                else
                    return Direction.DownLeft;
            }

            if(source.Coords.Item1 > destination.Coords.Item1)
            {
                if (source.Coords.Item2 + 1 == destination.Coords.Item2)
                    return Direction.UpRight;
                else
                    return Direction.UpLeft;
            }

            return Direction.Default;
        }

        public Direction Opposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.UpLeft: return Direction.DownRight;
                case Direction.UpRight: return Direction.DownLeft;
                case Direction.DownRight: return Direction.UpLeft;
                case Direction.DownLeft: return Direction.UpRight;
                default: return Direction.Default;
            }
        }

        public Direction TurnRight(Direction direction)
        {
            switch(direction)
            {
                case Direction.Left: return Direction.Up;
                case Direction.Right: return Direction.Down;
                case Direction.Up: return Direction.Right;
                case Direction.Down: return Direction.Left;
                default: return Direction.Default;
            }
        }

        public Node? GetNeighbour(Node node, Direction direction, List<Node> nodes)
        {
            return nodes.FirstOrDefault(n => n.Coords.Equals(Transpose(node.Coords, direction, 1)));
        }

        public List<Node> FindNeighbours(Node node, List<Node> grid, bool allowDiagonals)
        {
            return grid.Where(b =>
                    b.Coords.Item1 >= node.Coords.Item1 - 1
                    && b.Coords.Item1 <= node.Coords.Item1 + 1
                    && b.Coords.Item2 >= node.Coords.Item2 - 1
                    && b.Coords.Item2 <= node.Coords.Item2 + 1
                    && b != node
                    && (b.Coords.Item1 == node.Coords.Item1 || b.Coords.Item2 == node.Coords.Item2 || allowDiagonals)
                    ).ToList();
        }
        
        public string? DirectionDisplay(Direction? direction)
        {
            if (!direction.HasValue)
                return null;

            switch (direction)
            {
                case Direction.Left: return "<";
                case Direction.Right: return ">";
                case Direction.Up: return "^";
                case Direction.Down: return "V";
                default: return ".";
            }
        }

        public Direction DirectionFromChar(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
            {
                var lookupTable = Enum.GetValues(typeof(Direction)).Cast<Direction>().Where(x => x != Direction.Default).ToDictionary(x => x.ToString()[0], x => x);

                if (!lookupTable.ContainsKey(ch))
                    return Direction.Default;

                return lookupTable[ch];
            }

            return default;
        }

        public Tuple<int,int> Transpose(Tuple<int,int> source, Direction direction, long moves)
        {
            var coords = Transpose(new Tuple<long,long>(source.Item1,source.Item2), direction, moves);

            return new Tuple<int, int>((int)coords.Item1, (int)coords.Item2);
        }

        public Tuple<long, long> Transpose (Tuple<long, long> source, Direction direction, long moves)
        {
            long x = source.Item1;
            long y = source.Item2;

            if(direction == Direction.Up || direction == Direction.Down)
            {
                x = x + (moves * (direction == Direction.Up ? -1 : 1));
            }

            if(direction == Direction.Left || direction == Direction.Right)
            {
                y = y + (moves * (direction == Direction.Left ? -1 : 1));
            }

            return Tuple.Create(x, y);

        }

    }
}
