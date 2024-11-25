using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day10 : DayBase
    {
        public Day10() : base("Day10") { }

        protected override void Solve()
        {
            int y = 0;

            List<Pipe> pipeMap = new List<Pipe>();

            foreach (var line in lines)
            {
                var pipes = line.ToCharArray();

                for (int x = 0; x < pipes.Length; x++)
                {
                    var pipe = new Pipe { Position = new Tuple<int, int>(x, y), PipeChar = pipes[x] };
                    CalculatePipeConnections(pipe);

                    pipeMap.Add(pipe);
                }

                y++;
            }

            List<Pipe> pipesInLoop = new List<Pipe>();

            var startingPipe = pipeMap.FirstOrDefault(p => p.PipeChar == 'S');

            var startingPipes = pipeMap.Where(p => p.Connections != null).Where(p => p.Connections.Item1.Equals(startingPipe.Position) || p.Connections.Item2.Equals(startingPipe.Position)).ToArray();

            startingPipe.Connections = new Tuple<Tuple<int, int>, Tuple<int, int>>(startingPipes[0].Position, startingPipes[1].Position);

            var connectedPipes = pipeMap.Where(p => p.Connections != null)
                                        .Where(p => pipeMap.Where(c => c.Connections != null).Any(
                                            c => (c.Position.Equals(p.Connections.Item1) || c.Position.Equals(p.Connections.Item2))
                                                && (c.Connections.Item1.Equals(p.Position) || c.Connections.Item2.Equals(p.Position))
                                        ))
                                        .ToList();

            pipesInLoop.Add(startingPipe);

            Pipe nextPipe = connectedPipes.FirstOrDefault(p => p.Position.Equals(startingPipe.Connections.Item1) || p.Position.Equals(startingPipe.Connections.Item2));
            Pipe previousPipe = startingPipe;

            int steps = 1;

            while (nextPipe != startingPipe)
            {
                var currentPipe = nextPipe;

                nextPipe = connectedPipes.Where(p => p != previousPipe)
                    .FirstOrDefault(p => p.Position.Equals(currentPipe.Connections.Item1) || p.Position.Equals(currentPipe.Connections.Item2));

                previousPipe = currentPipe;

                pipesInLoop.Add(currentPipe);
                steps++;
            }

            if (partNo == 1)
            {
                total = steps / 2;
            }
            else
            {
                bool inside = false;

                for (y = 0; y < 139; y++)
                {
                    for (var x = 0; x < 139; x++)
                    {
                        if (x == 0 || y == 0 || x == 139 || y == 139)
                            inside = false;

                        var pipe = pipeMap.FirstOrDefault(p => p.Position.Equals(new Tuple<int, int>(x, y)));
                        if (pipesInLoop.Contains(pipe))
                        {
                            if (pipe.PipeChar == '|' || pipe.PipeChar == 'J' || pipe.PipeChar == 'L')
                                inside = !inside;
                        }
                        else if (inside)
                        {
                            total++;
                        }
                    }
                }
            }
        }

        private class Pipe
        {
            public Tuple<int, int> Position;
            public char PipeChar;
            public Tuple<Tuple<int, int>, Tuple<int, int>> Connections;
        }

        private void CalculatePipeConnections(Pipe pipe)
        {
            if (pipe.PipeChar == '.' || pipe.PipeChar == 'S')
                return;

            Tuple<int, int> connection1 = null, connection2 = null;

            switch (pipe.PipeChar)
            {
                case '|':
                case 'L':
                case 'J':
                    connection1 = new Tuple<int, int>(pipe.Position.Item1, pipe.Position.Item2 - 1);    // North
                    break;
                case '7':
                case 'F':
                    connection1 = new Tuple<int, int>(pipe.Position.Item1, pipe.Position.Item2 + 1);    // South
                    break;
                case '-':
                    connection1 = new Tuple<int, int>(pipe.Position.Item1 + 1, pipe.Position.Item2);    // East
                    break;
            }

            switch (pipe.PipeChar)
            {
                case '-':
                case 'J':
                case '7':
                    connection2 = new Tuple<int, int>(pipe.Position.Item1 - 1, pipe.Position.Item2);    // West
                    break;
                case 'L':
                case 'F':
                    connection2 = new Tuple<int, int>(pipe.Position.Item1 + 1, pipe.Position.Item2);    // East
                    break;
                case '|':
                    connection2 = new Tuple<int, int>(pipe.Position.Item1, pipe.Position.Item2 + 1);    // South
                    break;
            }

            if (connection1.Item1 < 0 || connection1.Item2 < 0
                || connection1.Item1 > 139 || connection1.Item2 > 139
                || connection2.Item1 < 0 || connection2.Item1 < 0
                || connection2.Item1 > 139 || connection2.Item2 > 139)
                return;

            pipe.Connections = new Tuple<Tuple<int, int>, Tuple<int, int>>(connection1, connection2);
        }

    }
}
