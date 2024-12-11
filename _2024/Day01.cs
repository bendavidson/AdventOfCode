using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day01 : DayBase
    {
        public Day01() : base("2024", "Day01") { }

        protected override void Solve()
        {
            List<long> listLeft = new List<long>();
            List<long> listRight = new List<long>();
            
            foreach(var line in lines)
            {
                var lineSplit = line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                listLeft.Add(Convert.ToInt64(lineSplit[0]));
                listRight.Add(Convert.ToInt64(lineSplit[1]));
            }

            if (partNo == 1)
            {
                for (int i = 0; i < listLeft.Count; i++)
                {
                    total = total + Math.Abs(listRight.OrderBy(x => x).Skip(i).First() - listLeft.OrderBy(x => x).Skip(i).First());
                }
            }
            else
            {
                foreach (long locationId in listLeft)
                {
                    total = total + (locationId * listRight.Where(x => x == locationId).Count());
                }
            }
        }
    }
}
