using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day02 : DayBase
    {
        public Day02() : base("2024", "Day02") { }

        protected override void Solve()
        {
            foreach(var line in lines)
            {
                var lineArray = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();

                if (IsSafe(Report(lineArray)))
                {
                    total = total + 1;                   
                }
                else
                {
                    if(partNo == 2)
                    {
                        for (int i = 0; i < lineArray.Count; i++)
                        {
                            if(IsSafe(Report(lineArray.Where((x, y) => y != i).ToList())))
                            { 
                                total = total + 1;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private List<int> Report(List<int> lineArray)
        {
            List<int> report = new List<int>();

            for (int i = 0; i < lineArray.Count - 1; i++)
            {
                report.Add(lineArray[i + 1] - lineArray[i]);
            }

            return report;
        }
        
        private bool IsSafe(List<int> report)
        {
            if ((report.Where(x => x > 0).Count() == report.Count
                    || report.Where(x => x < 0).Count() == report.Count)
                    && !report.Any(x => Math.Abs(x) > 3)
                    && !report.Any(x => Math.Abs(x) < 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
