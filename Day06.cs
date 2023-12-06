using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day06 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day06.txt";
        string outputFile = "../../../Outputs/Day06.csv";

        public Day06()
        {
            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            Console.WriteLine("Are you solving Part 1 or Part 2?");
            var partNo = Convert.ToInt32(Console.ReadLine());

            List<Race> races = new List<Race>();
            
            while (line != null)
            {
                string[] parts = line.Split((partNo == 1 ? new char[] { ':',' ' } : new char[] { ':' }), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                for (int i = 1; i < parts.Length; i++)
                {
                    if (parts[0].StartsWith("Time"))
                    {
                        Race race = new Race
                        {
                            No = i,
                            Duration = Convert.ToInt64(partNo == 1 ? parts[i] : parts[i].Replace(" ",""))
                        };

                        races.Add(race);
                    }
                    else
                    {
                        var race = races.FirstOrDefault(x => x.No == i);
                        race.Distance = Convert.ToInt64(partNo == 1 ? parts[i] : parts[i].Replace(" ", ""));
                    }    
                }

                line = sr.ReadLine();
            }

            sr.Close();

            Int64 total = 0;

            foreach(var race in  races)
            {
                Console.WriteLine("Race " + race.No.ToString());

                for(int i = 1; i < race.Duration; i++)
                {
                    var movingTime = race.Duration - i;
                    var distance = movingTime * i;

                    if(distance > race.Distance)
                    {
                        race.WinningWays++;
                    }
                }
            }

            total = races.Select(x => x.WinningWays).Aggregate((x, y) => x * y);

            Console.WriteLine("Result is: " + total.ToString());
        }

        private class Race
        {
            public Int64 No;
            public Int64 Duration;
            public Int64 Distance;
            public Int64 WinningWays;
        }
    }
}
    