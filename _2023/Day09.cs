using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day09 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day09.txt";
        string outputFile = "../../../Outputs/Day09.csv";
        long partNo = 0;

        public Day09()
        {
            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            Console.WriteLine("Are you solving Part 1 or Part 2?");
            partNo = Convert.ToInt64(Console.ReadLine());

            long total = 0;

            List<long[]> patterns = new List<long[]>();

            while (line != null)
            {
                var pattern = line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => Convert.ToInt64(x))
                    .ToArray();

                patterns.Add(pattern);

                line = sr.ReadLine();
            }

            sr.Close();

            foreach (var pattern in patterns)
            {
                Dictionary<long, long[]> differences = new Dictionary<long, long[]>();

                long i = 0;

                var difference = pattern.Zip(pattern.Skip(1), (x, y) => y - x).ToArray();
                differences.Add(i, difference);

                while (difference.Any(x => x != 0))
                {
                    i++;

                    difference = difference.Zip(difference.Skip(1), (x, y) => y - x).ToArray();
                    differences.Add(i, difference);
                }

                long lastDiff = 0;

                for (long j = differences.Count() - 2; j >= 0; j--)
                {
                    if (partNo == 1)
                        lastDiff += differences[j][differences[j].Length - 1];
                    else
                        lastDiff = differences[j][0] - lastDiff;
                }

                if (partNo == 1)
                    total += pattern[pattern.Length - 1] + lastDiff;
                else
                    total += pattern[0] - lastDiff;

                string outLine = string.Join(',', pattern) + ","
                    + (partNo == 1 ? (pattern[pattern.Length - 1] + lastDiff).ToString() : (pattern[0] - lastDiff).ToString())
                    + Environment.NewLine;
                File.AppendAllText(outputFile, outLine);
            }

            Console.WriteLine("Result is: " + total.ToString());

        }
    }
}
