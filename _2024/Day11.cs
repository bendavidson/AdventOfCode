using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day11 : DayBase
    {
        public Day11() : base("2024", "Day11") { }

        protected override void Solve()
        {
            var stones = new Dictionary<long, long>();

            foreach (var line in lines)
            {
                foreach (var item in line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)))
                {
                    AddStone(stones, item, 1);
                };
            }

            for(int i = 0; i < (partNo == 1 ? 25 : 75);  i++)
            {
                Blink(stones);

                //Console.WriteLine(string.Join(" ", stones.Where(x => x.Value > 0).Select(x => x.ToString()).ToArray()));
            }

            total = stones.Values.Sum();
        }

        private void Blink(Dictionary<long, long> stones)
        {
            var blinkStones = new Dictionary<long, long>(stones);

            foreach (var stone in blinkStones.Where(x => x.Value != 0))
            {
                stones[stone.Key] -= stone.Value;
                
                if (stone.Key == 0)
                {
                    AddStone(stones, 1, stone.Value);
                }
                else if (IsEvenDigits(stone.Key))
                {
                    var stoneSplit = SplitStoneValue(stone.Key);

                    AddStone(stones, stoneSplit.Item1, stone.Value);
                    AddStone(stones, stoneSplit.Item2, stone.Value);
                }
                else
                {
                    AddStone(stones, stone.Key * 2024, stone.Value);
                }
            }
        }

        private void AddStone(Dictionary<long, long> stones, long stone, long count)
        {
            if(!stones.ContainsKey(stone))
                stones.Add(stone, count);
            else
                stones[stone] += count;
        }

        private bool IsEvenDigits(long value)
        {
            var digits = Math.Floor(Math.Log10(value) + 1);

            return digits % 2 == 0;
        }

        private (long, long) SplitStoneValue(long value)
        {
            var digits = Math.Floor(Math.Log10(value) + 1);

            var leftValue = Math.Floor(value / Math.Pow(10, digits / 2));

            var rightValue = value - (leftValue * Math.Pow(10, digits / 2));

            return (Convert.ToInt64(leftValue), Convert.ToInt64(rightValue));
        }
    }
}
