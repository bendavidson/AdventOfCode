using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day05 : DayBase
    {
        public Day05() : base("2024", "Day05") { }

        protected override void Solve()
        {
            List<Tuple<int,int>> rules = new List<Tuple<int,int>>();
            List<int[]> updates = new List<int[]>();
            
            foreach(var line in lines)
            {
                if(line.Contains('|'))
                {
                    var ruleSplit = line.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    rules.Add(new Tuple<int, int>(Convert.ToInt32(ruleSplit[0]), Convert.ToInt32(ruleSplit[1])));
                }

                if(line.Contains(','))
                {
                    updates.Add(line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToArray());
                }
            }

            List<int[]> correctlyOrderedUpdates = new List<int[]>();
            
            foreach(var update in updates)
            {
                if(UpdateCorrectlyOrdered(update, rules))
                {
                    correctlyOrderedUpdates.Add(update);
                }
            }

            if (partNo == 1)
            {
                foreach (var update in correctlyOrderedUpdates)
                {
                    total = total + update[(int)Math.Floor(update.Length / 2.0)];
                }
            }
            else
            {
                foreach(var update in updates.Except(correctlyOrderedUpdates))
                {
                    CorrectUpdateOrder(update, rules);

                    total = total + update[(int)Math.Floor(update.Length / 2.0)];
                }
            }


        }

        private bool UpdateCorrectlyOrdered(int[] update, List<Tuple<int,int>> rules)
        {
            for (int i = 0; i < update.Length; i++)
            {
                if(rules.Exists(r => r.Item2 == update[i] && update.Skip(i+1).Any(u => u == r.Item1)))
                {
                    return false;
                }
            }

            return true;
        }

        private void CorrectUpdateOrder(int[] update, List<Tuple<int,int>> rules)
        {
            for (int i = 0; i < update.Length; i++)
            {
                foreach(var rule in rules.Where(r => r.Item1 == update[i] && update.Any(u => u == r.Item2)))
                {
                    var indexToSwap = Array.IndexOf(update, rule.Item2);

                    if (indexToSwap < i)
                    {
                        SwapPages(ref update[i], ref update[indexToSwap]);

                        if (UpdateCorrectlyOrdered(update, rules))
                        {
                            return;
                        }
                    }
                }
            }

            if(!UpdateCorrectlyOrdered(update, rules))
                CorrectUpdateOrder(update, rules);
        }

        private void SwapPages(ref int x, ref int y)
        {
            (x, y) = (y, x);
        }
    }
}
