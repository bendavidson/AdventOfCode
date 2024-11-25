using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode._2023
{
    internal class Day03 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day03.txt";
        string outputFile = "../../../Outputs/Day03.csv";
        List<LineContent> lineContents = new List<LineContent>();

        public Day03()
        {
            Dictionary<int, char[]> lines = new Dictionary<int, char[]>();

            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            int i = 0;

            while (line != null)
            {
                Console.WriteLine(line);
                lines.Add(i, line.ToCharArray());

                line = sr.ReadLine();

                i++;
            }


            foreach (var lineArray in lines)
            {
                LineContent lineContent = null;

                for (i = 0; i < lineArray.Value.Length; i++)
                {
                    // Ignore periods
                    if (lineArray.Value[i] == '.')
                    {
                        if (lineContent != null)
                        {
                            lineContents.Add(lineContent);
                            lineContent = null;
                        }
                        continue;
                    }

                    // If a number
                    if (char.IsNumber(lineArray.Value[i]))
                    {
                        // No current line content - start a new one
                        if (lineContent == null)
                        {
                            lineContent = new LineContent { LineNo = lineArray.Key, Content = lineArray.Value[i].ToString(), StartingIndex = i };
                            continue;
                        }

                        // Current line content is a symbol - save and start a new one
                        if (!lineContent.IsNumber)
                        {
                            lineContents.Add(lineContent);
                            lineContent = new LineContent { LineNo = lineArray.Key, Content = lineArray.Value[i].ToString(), StartingIndex = i };
                            continue;
                        }

                        // If we get here, current line content is already a number - add this digit to it
                        lineContent.Content += lineArray.Value[i].ToString();
                        continue;
                    }

                    // If we get her, the content must be a symbol - if we already have a content then it must be a number - save and create a new one
                    if (lineContent != null)
                    {
                        lineContents.Add(lineContent);
                        lineContent = new LineContent { LineNo = lineArray.Key, Content = lineArray.Value[i].ToString(), StartingIndex = i };
                        continue;
                    }

                    // If we get here - add a new single length content, save it and move on
                    lineContent = new LineContent { LineNo = lineArray.Key, Content = lineArray.Value[i].ToString(), StartingIndex = i };
                    lineContents.Add(lineContent);
                    lineContent = null;
                }

                if (lineContent != null)
                {
                    lineContents.Add(lineContent);
                }
            }

            var numbers = lineContents.Where(x => x.IsNumber);

            int total = 0;

            foreach (var number in numbers)
            {
                if (lineContents.Any(x => !x.IsNumber
                                    && x.LineNo >= number.LineNo - 1
                                    && x.LineNo <= number.LineNo + 1
                                    && x.StartingIndex >= number.searchStartIndex
                                    && x.StartingIndex <= number.searchEndIndex
                                    ))
                {
                    total += Convert.ToInt32(number.Content);
                }
            }

            Console.WriteLine("Result is: " + total.ToString());

            var gears = lineContents.Where(x => x.Content == "*");
            total = 0;

            foreach (var gear in gears)
            {
                var parts = lineContents.Where(x => x.IsNumber
                                            && x.LineNo >= gear.LineNo - 1
                                            && x.LineNo <= gear.LineNo + 1
                                            && x.searchStartIndex <= gear.StartingIndex
                                            && x.searchEndIndex >= gear.StartingIndex
                );

                if (parts.Count() == 2)
                {
                    total += parts.Select(x => Convert.ToInt32(x.Content))
                                    .Aggregate((x, y) => x * y);
                }
            }

            Console.WriteLine("Result for Part 2 is: " + total.ToString());
        }

        private class LineContent
        {
            public int LineNo;
            public string Content;
            public int StartingIndex;
            public bool IsNumber { get { return int.TryParse(Content, out _); } }
            public int searchStartIndex { get { return Math.Max(0, StartingIndex - 1); } }
            public int searchEndIndex { get { return Math.Min(139, StartingIndex + Content.Length); } }
        }
    }
}
