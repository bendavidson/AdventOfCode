using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day14 : DayBase
    {
        private Hashtable _tiltedColumns = new Hashtable();

        public Day14() : base("Day14") { }

        protected override void Solve()
        {
            List<char[]> columns = new List<char[]>();

            foreach (var line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (columns.Count() < i + 1)
                    {
                        columns.Add(new char[lines.Count()]);
                    }

                    columns[i][line.Length - lines.IndexOf(line) - 1] = line[i];

                }
            }

            var columnHeight = columns[0].Length;

            if (partNo == 1)
            {
                foreach (var column in columns)
                {
                    Console.WriteLine(new String(column));

                    var tiltedColumn = TiltColumn(column);

                    total += tiltedColumn.Select((x, i) => new { index = i, x }).Where(r => r.x == 'O').Sum(r => r.index+1);

                    Console.WriteLine(new String(tiltedColumn));
                }
            }
            else
            {
                var tiltedColumns = new List<char[]>();
                var rotatedColumns = new List<char[]>();

                foreach (var column in columns)
                {
                    tiltedColumns.Add(TiltColumn(column));
                }               

                for(int i = 1; i < 4; i++)
                {
                    rotatedColumns = RotateColumns(tiltedColumns);
                    tiltedColumns = new List<char[]>();

                    foreach (var column in rotatedColumns)
                    {
                        tiltedColumns.Add(TiltColumn(column));
                    }
                }

                // Rotate again back to North (but don't tilt)
                rotatedColumns = RotateColumns(tiltedColumns);

                var cyclesDone = new List<List<string>>();
                int cycles = 1;

                while (!cyclesDone.Any(c => c.SequenceEqual(rotatedColumns.Select(r => new String(r)).ToList())))
                {
                    var cycleStrings = rotatedColumns.Select(c => new String(c)).ToList();
                    cyclesDone.Add(cycleStrings);

                    for (int i = 1; i <= 4; i++)
                    {
                        tiltedColumns = new List<char[]>();

                        foreach (var column in rotatedColumns)
                        {
                            tiltedColumns.Add(TiltColumn(column));
                        }

                        rotatedColumns = RotateColumns(tiltedColumns);
                    }

                    cycles++;
                }

                var matchingCycle = cyclesDone.IndexOf(cyclesDone.FirstOrDefault(c => c.SequenceEqual(rotatedColumns.Select(r => new String(r)).ToList())));

                var endCycle = ((1000000000 - matchingCycle)%(cyclesDone.Count() - matchingCycle))+matchingCycle-1;

                var cycleToSum = cyclesDone[endCycle].Select(c => c.ToCharArray()).ToList();

                foreach (var column in cycleToSum)
                {
                    columnHeight = column.Length;

                    total += column.Select((x, i) => new { index = i, x }).Where(r => r.x == 'O').Sum(r => r.index + 1);
                }
            }

        }
        private class Boulder
        {
            public int Index { get; set; }
            public char BoulderType { get; set; }
        }

        private char[] TiltColumn(char[] column)
        {
            column = column.Reverse().ToArray();
            var tiltedColumn = Enumerable.Repeat('.',column.Length).ToArray();

            var columnHeight = column.Length;

            if (_tiltedColumns.Contains(column))
                return (char[])_tiltedColumns[column];
            
            var rocks = column.Select((x, i) => new { index = i, x }).Where(r => r.x == 'O').ToList();
            var cubes = column.Select((x, i) => new { index = i, x }).Where(r => r.x == '#').ToList();

            foreach( var c in cubes )
            {
                tiltedColumn[c.index] = c.x;
            }
            
            foreach (var rock in rocks)
            {
                var nextCube = Array.FindLastIndex(column, rock.index, x => x == '#');

                var rocksBetween = rocks.Where(r => r.index > nextCube && r.index < rock.index).Count();

                tiltedColumn[rock.index - ((rock.index - (nextCube + 1))) + rocksBetween] = rock.x;
            }

            _tiltedColumns.Add(column,tiltedColumn);

            return tiltedColumn.Reverse().ToArray();
        }

        private List<char[]> RotateColumns(List<char[]> columns)
        {
            var rotatedColumns = new List<char[]>();

            foreach( var column in columns )
            {
                for (int i = 0; i < column.Length; i++)
                {
                    if (rotatedColumns.Count() < i + 1)
                        rotatedColumns.Add(new char[columns.Count()]);

                    rotatedColumns[i][column.Length - columns.IndexOf(column) - 1] = column[i];
                }
            }

            return rotatedColumns;
        }
    }
}
