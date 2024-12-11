using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day03 : DayBase
    {
        public Day03() : base("2024", "Day03") {}

        protected override void Solve()
        {

            var multiplications = new List<string>();
            bool doInstruction = true;

            foreach (var line in lines)
            {
                var lineSplit = Regex.Split(line, @"(mul\(\d+,\d+\))");

                foreach (var instruction in lineSplit)
                {
                    if(Regex.IsMatch(instruction, @"(mul\(\d+,\d+\))")
                        && (doInstruction || partNo == 1))
                    {
                        multiplications.Add(instruction);
                    }
                    else if(doInstruction && Regex.IsMatch(instruction, @"don't\(\)"))
                    {
                        doInstruction = false;
                    }
                    else if(!doInstruction && Regex.IsMatch(instruction, @"do\(\)"))
                    {
                        doInstruction = true;
                    }                    
                }
            }

            foreach (var multiplication in multiplications)
            {
                var calcNumbers = multiplication.Replace("mul(", "").Replace(")", "").Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                total = total + (calcNumbers[0] * calcNumbers[1]);
            }
        }
    }
}
