using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day07 : DayBase
    {
        public Day07() : base("2024", "Day07") { }

        protected override void Solve()
        {
            List<(long, int[])> equations = new List<(long, int[])>();

            foreach(var line in lines)
            {
                var equationSplit = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                equations.Add((Convert.ToInt64(equationSplit[0]), 
                    equationSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt32(x)).ToArray()));
            }

            foreach(var equation in equations)
            {
                if (SolveEquation(equation.Item1, equation.Item2))
                    total = total + equation.Item1;
            }
        }

        private bool SolveEquation(long answer, int[] numbers)
        {
            if (numbers.Length > 2)
            {
                return (SolveEquation(answer - numbers[numbers.Length - 1], numbers.SkipLast(1).ToArray()) && (answer - numbers[numbers.Length - 1] > 0))
                    || (SolveEquation(answer / numbers[numbers.Length - 1], numbers.SkipLast(1).ToArray()) && (answer % numbers[numbers.Length - 1] == 0));
            }
            else
            {
                return (answer == numbers[0] +  numbers[1]) || (answer == numbers[0] * numbers[1]);
            }
        }
    }
}
