using AdventOfCode2023.Algorithms.DijkstraCalculator;
using AdventOfCode2023.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Helpers
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
                default: return Direction.Default;
            }
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
