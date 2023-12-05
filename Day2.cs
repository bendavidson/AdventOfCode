using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day2 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day2.txt";
        string outputFile = "../../../Outputs/Day2.csv";
        int maxRed = 12;
        int maxGreen = 13;
        int maxBlue = 14;

        public Day2() 
        {
            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            Dictionary<int, List<Tuple<string, int>>> lines = new Dictionary<int, List<Tuple<string, int>>>();
            Dictionary<int,int> gameMinCubes = new Dictionary<int,int>();

            while (line != null)
            {
                Console.WriteLine(line);
                
                var cubesShown = new List<Tuple<string, int>>();

                var topSplit = line.Split(':');
                int gameNo = Convert.ToInt32(topSplit[0].Replace("Game ", "").Trim());

                var cubes = topSplit[1].Split(new char[] {',',';'});

                int minRed = 0;
                int minGreen = 0;
                int minBlue = 0;

                bool invalidGame = false;
                
                for(int i = 0; i < cubes.Length; i++)
                {
                    var cubeColours = cubes[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    string colour = cubeColours[1];
                    int noOfCubes = Convert.ToInt32(cubeColours[0]);
                    cubesShown.Add(new Tuple<string, int>( cubeColours[1], Convert.ToInt32(cubeColours[0]) ));

                    int maxCubes = 0;
                    switch (colour.ToLower())
                    {
                        case "red":
                            maxCubes = maxRed; 
                            minRed = noOfCubes > minRed ? noOfCubes : minRed;
                            break;
                        case "green":
                            maxCubes = maxGreen;
                            minGreen = noOfCubes > minGreen ? noOfCubes : minGreen;
                            break;
                        case "blue":
                            maxCubes = maxBlue;
                            minBlue = noOfCubes > minBlue ? noOfCubes : minBlue;
                            break;
                        default: maxCubes = 0;
                            break;
                    }

                    if(noOfCubes > maxCubes)
                    {
                        invalidGame = true;           
                    }
                }

                if (!invalidGame)
                {
                    lines.Add(gameNo, cubesShown);
                }

                gameMinCubes.Add(gameNo, (minRed * minGreen * minBlue));

                line = sr.ReadLine();
            }

            sr.Close();

            int total = 0;
            int totalPart2 = 0;

            foreach( var possibleGame in lines)
            {
                total += possibleGame.Key;
            }

            foreach(var game in gameMinCubes)
            {
                totalPart2 += game.Value;
            }

            Console.WriteLine("Result is: " + total.ToString());

            Console.WriteLine("Result for Part 2 is: " + totalPart2.ToString());

        }
    }
}
