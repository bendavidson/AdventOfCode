using AdventOfCode2023.Algorithms;
using AdventOfCode2023.Enums;
using AdventOfCode2023.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day18 : DayBase
    {
        private GridHelper _gridHelper = new GridHelper();

        public Day18() : base("Day18") { }

        protected override void Solve()
        {
            List<Tuple<Direction, long, string>> instructions = new List<Tuple<Direction, long, string>>();
            
            foreach(var line in lines)
            {
                var instruction = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                instructions.Add(new Tuple<Direction, long, string>(_gridHelper.DirectionFromChar(instruction[0][0]), Convert.ToInt64(instruction[1]), instruction[2]));
            }

            List<Tuple<long, long>> vectors = new List<Tuple<long, long>>();

            var seed = new Tuple<long,long>(0, 0);
            
            Dictionary<int,Direction> directionFromHex = new Dictionary<int,Direction>();
            directionFromHex.Add(0, Direction.Right);
            directionFromHex.Add(1, Direction.Down);
            directionFromHex.Add(2, Direction.Left);
            directionFromHex.Add(3, Direction.Up);

            foreach(var instruction in instructions)
            {
                vectors.Add(seed);
                var numFromHex = long.Parse(instruction.Item3.Substring(2,5),System.Globalization.NumberStyles.HexNumber);
                var dirFromHex = directionFromHex[Convert.ToInt32(instruction.Item3.Substring(instruction.Item3.Length - 2, 1))];
                seed = _gridHelper.Transpose(seed, partNo == 2 ? dirFromHex : instruction.Item1, partNo == 2 ? numFromHex : instruction.Item2);
            }

            var xArray = vectors.Select(x => (double)x.Item1).ToArray();
            var yArray = vectors.Select(x => (double)x.Item2).ToArray();

            var area = Shoelace.PolygonArea(xArray, yArray, vectors.Count());

            long perimeter = instructions.Sum(i => partNo == 2 ? long.Parse(i.Item3.Substring(2, 5), System.Globalization.NumberStyles.HexNumber) : i.Item2);

            total = (long)(area + (perimeter/2)+1);
        }
    }
}
