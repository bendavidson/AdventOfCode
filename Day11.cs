using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day11 : DayBase
    {
        List<int> emptyColumns = new List<int>();
        List<int> emptyRows = new List<int>();

        public Day11() : base("Day11") { }

        protected override void Solve()
        {
            List<List<char>> rows = new List<List<char>>();
            
            foreach(var line in lines)
            {
                rows.Add(line.ToList());
            }

            
            
            
            for(int i = 0; i < rows[0].Count; i++)
            {
                if(rows.Where(r => r[i] == '.').Count() == rows.Count())
                {
                    emptyColumns.Add(i);
                }
            }

            for(int i = 0; i < rows.Count; i++)
            {
                if (!rows[i].Any(r => r == '#'))
                {
                    emptyRows.Add(i);
                }
            }

            Dictionary<int, Tuple<int, int>> galaxies = new Dictionary<int, Tuple<int, int>>();

            int g = 0;
            
            for(int i = 0; i < rows.Count; i++)
            {
                for(int j = 0; j < rows[i].Count(); j++)
                {
                    if (rows[i][j] == '#')
                    {
                        galaxies.Add(g, new Tuple<int, int>(i, j));
                        g++;
                    }
                }
            }

            var pairs = from a in galaxies
                        from b in galaxies
                        select new Tuple<KeyValuePair<int,Tuple<int,int>>,KeyValuePair<int, Tuple<int, int>>>( a, b ) into temp
                        where temp.Item1.Key < temp.Item2.Key 
                        select temp;


            foreach(var galaxyPair in pairs.OrderBy(p => p.Item1.Key).ThenBy(p => p.Item2.Key))
            {
                total += ShortestPath(galaxyPair.Item1.Value, galaxyPair.Item2.Value);
            }
        }

        private Int64 ShortestPath(Tuple<int,int> a, Tuple<int,int> b)
        {
            Int64 path = 0;
            
            int x1 = a.Item1;
            int x2 = b.Item1;

            int itemsToAdd = 0;

            if (x1 < x2)
            {
                path = x2 - x1;
                itemsToAdd = emptyRows.Where(r => r > x1 && r < x2).Count();
            }
            else
            {
                path = x1 - x2;
                itemsToAdd = emptyRows.Where(r => r > x2 && r < x1).Count();
            }
            path += (itemsToAdd * (partNo == 1 ? 1 : 999999));

            x1 = a.Item2;
            x2 = b.Item2;
            itemsToAdd = 0;

            if (x1 < x2)
            {
                path += (x2 - x1);
                itemsToAdd = emptyColumns.Where(r => r > x1 && r < x2).Count();
            }
            else
            {
                path += (x1 - x2);
                itemsToAdd = emptyColumns.Where(r => r > x2 && r < x1).Count();
            }
            path += (itemsToAdd * (partNo == 1 ? 1 : 999999));

            return path;
        }
    }
}
