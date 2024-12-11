using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class DayBase : IPuzzle
    {
        protected string inputFile;
        protected string outputFile;
        protected Int64 partNo;
        protected List<string> lines;
        protected Int64 total = 0;

        public DayBase(string year, string day)
        {
            inputFile = "../../../Inputs/" + year + "/" + day + ".txt";
            outputFile = "../../../Outputs/" + year + "/" + day + ".csv";

            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            Console.WriteLine("Are you solving Part 1 or Part 2?");
            partNo = Convert.ToInt64(Console.ReadLine());

            lines = new List<string>();
            
            while (line != null)
            {
                lines.Add(line);

                line = sr.ReadLine();
            }

            sr.Close();

            Solve();

            DisplayResult();
        }

        protected virtual void Solve()
        {

        }

        private void DisplayResult()
        {
            Console.WriteLine("Result is: " + total.ToString());
        }
    }
}
