using AdventOfCode2023.DijkstraCalculator;
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

            switch(direction)
            {
                case Direction.Left: return "<";
                case Direction.Right: return ">";
                case Direction.Up: return "^";
                case Direction.Down: return "V";
                default: return ".";
            }
        }
    }
}
