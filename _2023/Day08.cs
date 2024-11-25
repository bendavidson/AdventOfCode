using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode._2023
{
    internal class Day08 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day08.txt";
        string outputFile = "../../../Outputs/Day08.csv";
        int partNo = 0;

        public Day08()
        {
            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            Console.WriteLine("Are you solving Part 1 or Part 2?");
            partNo = Convert.ToInt32(Console.ReadLine());

            int i = 0;

            char[] directions = { };
            List<Node> nodes = new List<Node>();

            while (line != null)
            {
                if (line.Length > 0)
                {
                    if (i == 0)
                    {
                        directions = line.ToCharArray();
                    }
                    else
                    {
                        var nodeParts = line.Split(new char[] { ' ', '=', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        Node node = new Node
                        {
                            Source = nodeParts[0],
                            DestinationL = nodeParts[1],
                            DestinationR = nodeParts[2]
                        };
                        nodes.Add(node);
                    }
                }

                i++;
                line = sr.ReadLine();
            }

            sr.Close();

            Int128 total = 0;

            i = 0;

            if (partNo == 1)
            {
                Node node = nodes.FirstOrDefault(x => x.Source == "AAA");

                while (node.Source != "ZZZ")
                {
                    int j = i % directions.Length;
                    var direction = directions[j];

                    node = nodes.FirstOrDefault(x => x.Source == node.Destination(direction));

                    i++;
                }

                total = i;
            }
            else if (partNo == 2)
            {
                var nodesToCheck = nodes.Where(x => x.Source.EndsWith('A'));

                List<Tuple<string, string, Int128>> map = new List<Tuple<string, string, Int128>>();

                foreach (var node in nodesToCheck)
                {
                    i = 0;
                    var nodeBeingChecked = node;

                    while (!nodeBeingChecked.Source.EndsWith('Z'))
                    {
                        int j = i % directions.Length;
                        var direction = directions[j];

                        nodeBeingChecked = nodes.FirstOrDefault(x => x.Source == nodeBeingChecked.Destination(direction));

                        i++;
                    }

                    map.Add(new Tuple<string, string, Int128>(node.Source, nodeBeingChecked.Source, i));
                }

                nodesToCheck = nodes.Where(x => x.Source.EndsWith('Z'));

                //foreach (var node in nodesToCheck)
                //{
                //    i = 0;
                //    var nodeBeingChecked = node;

                //    while (!nodeBeingChecked.Source.EndsWith('Z') || i == 0)
                //    {
                //        int j = i % directions.Length;
                //        var direction = directions[j];

                //        nodeBeingChecked = nodes.FirstOrDefault(x => x.Source == nodeBeingChecked.Destination(direction));

                //        i++;
                //    }

                //    map.Add(new Tuple<string, string, int>(node.Source, nodeBeingChecked.Source, i));
                //}

                total = LowestCommonMultiple(map.Select(x => x.Item3).ToArray());
            }

            Console.WriteLine("Result is: " + total.ToString());

        }
        private class Node
        {
            public string Source;
            public string DestinationL;
            public string DestinationR;
            public string Destination(char direction)
            {
                return direction == 'L' ? DestinationL : DestinationR;
            }
        }

        private Int128 GreatestCommonDenominator(Int128 x, Int128 y)
        {
            if (y == 0)
            {
                return x;
            }
            else
            {
                return GreatestCommonDenominator(y, x % y);
            }
        }

        private Int128 LowestCommonMultiple(Int128[] n)
        {
            return n.Aggregate((S, val) => S * val / GreatestCommonDenominator(S, val));
        }
    }
}
