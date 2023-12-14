using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day12 : DayBase
    {
        public Day12() : base("Day12") { }

        protected override void Solve()
        {
            List<Tuple<int, char[], int[]>> rows = new List<Tuple<int, char[], int[]>>();

            int i = 0;
            
            foreach(var line in lines)
            {
                var lineSplit = line.Split(' ',StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                rows.Add(new Tuple<int, char[], int[]>(
                    i,
                    lineSplit[0].ToCharArray(),
                    lineSplit[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray()
                    ));

                i++;
            }

            foreach(var row in rows)
            {
                int minRowLength = row.Item3.Sum() + row.Item3.Count() - 1;

                int firstNonOperationalPosition = Array.IndexOf(row.Item2, row.Item2.First(x => x != '.'));

                int lastNonOperationalPosition = Array.LastIndexOf(row.Item2, row.Item2.Reverse().First(x => x != '.'));

                // Length of contiguous groups is the same as the length of the operational row array so only 1 arrangement possible
                if (minRowLength == (lastNonOperationalPosition - firstNonOperationalPosition)+1)
                {
                    total += 1;
                    continue;
                }

                List<Tuple<int,int>> operationalGroups = new List<Tuple<int, int>>();
                int operationalGroupStart = 0;
                int operationalGroupLength = 0;

                for(i = firstNonOperationalPosition; i < lastNonOperationalPosition; i++)
                {
                    if (row.Item2[i] == '.')
                    {
                        if(operationalGroupLength == 0)
                        {
                            operationalGroupStart = i;
                        }
                        operationalGroupLength++;
                    }
                    else if (operationalGroupLength > 0)
                    {
                        operationalGroups.Add(new Tuple<int, int>(operationalGroupStart, operationalGroupLength));
                        operationalGroupLength = 0;
                    }
                }

                if (operationalGroupLength > 0)
                    operationalGroups.Add(new Tuple<int, int>(operationalGroupStart, operationalGroupLength));

                int skip = firstNonOperationalPosition;

                foreach(var groupLength in row.Item3)
                {
                    int nextOperationalPosition = Array.IndexOf(row.Item2, '.', skip);

                }
            }
        }
    }
}
