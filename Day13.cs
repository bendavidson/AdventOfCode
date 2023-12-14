using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day13 : DayBase
    {
        public Day13() : base("Day13") { }

        protected override void Solve()
        {
            List<char[]> pattern = new List<char[]>();

            foreach(var line in lines)
            {
                if (line.Length > 0)
                {
                    pattern.Add(line.ToCharArray());
                }
                else
                {
                    total += SolvePattern(pattern);
                    pattern = new List<char[]>();
                }
            }

            total += SolvePattern(pattern);
        }

        private int SolvePattern(List<char[]> pattern)
        {
            // Check for horizontal reflection first
            int result = FindReflection(pattern)*100;

            // No horizontal reflection found - twist the pattern and try againif (result == 0)
            if (result == 0)
            {
                var twistedPattern = new List<char[]>();

                for (int i = 0; i < pattern[0].Length; i++)
                {
                    twistedPattern.Add(pattern.Select(p => p[i]).Reverse().ToArray());
                }

                result = FindReflection(twistedPattern);
            }

            return result;
        }

        private int FindReflection(List<char[]> pattern)
        {
            if (partNo == 1)
            {
                var potentialReflections = pattern
                    .Zip(pattern.Skip(1), (x, y) => new { x, y })
                    .Select((p, i) => new { i, p.x, p.y })
                    .Where(p => new string(p.x) == new String(p.y));

                foreach (var p in potentialReflections)
                {
                    if (CheckReflection(p.i, pattern))
                    {
                        return p.i + 1;
                    }
                }
            }
            else
            {
                var potentialSmudges = from a in pattern.Select((row, index) => new { row, index })
                                       from b in pattern.Select((row, index) => new { row, index })
                                       select (a, b) into allPairs
                                       where allPairs.a.index < allPairs.b.index
                                       select (allPairs.a, allPairs.b) into distinctPairs
                                       where distinctPairs.a.row.Zip(distinctPairs.b.row, (y, z) => y != z).Count(f => f) == 1 
                                                && (distinctPairs.b.index - distinctPairs.a.index)%2 != 0
                                       select new { indexA = distinctPairs.a.index, a = distinctPairs.a.row, indexB = distinctPairs.b.index, b = distinctPairs.b.row };

                foreach(var p in potentialSmudges)
                {
                    List<char[]> desmudgedPattern = new List<char[]>(pattern);
                    desmudgedPattern[p.indexA] = p.b;

                    var potentialReflectionPoint = (int)Math.Floor((double)(p.indexB - p.indexA) / 2) + p.indexA;

                    if (CheckReflection(potentialReflectionPoint, desmudgedPattern))
                    {
                        return potentialReflectionPoint + 1;
                    }    
                }
            }

            return 0;
        }

        private bool CheckReflection(int p, List<char[]> pattern)
        {
            var patternHeight = pattern.Count();

            for (int i = p; i >= Math.Max(0, p - (patternHeight - (p + 1)) + 1); i--)
            {
                if (new String(pattern[i]) != new String(pattern[p + (p - i) + 1]))
                    return false;
            }

            return true;
        }
    }
}
