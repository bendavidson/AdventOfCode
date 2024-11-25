using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day01 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day01.txt";
        string outputFile = "../../../Outputs/Day01.csv";
        Dictionary<string, int> numberStrings;
        Dictionary<string, int> numberStringsReverse;

        public Day01()
        {

            numberStrings = new Dictionary<string, int>();
            numberStrings.Add("one", 1);
            numberStrings.Add("two", 2);
            numberStrings.Add("three", 3);
            numberStrings.Add("four", 4);
            numberStrings.Add("five", 5);
            numberStrings.Add("six", 6);
            numberStrings.Add("seven", 7);
            numberStrings.Add("eight", 8);
            numberStrings.Add("nine", 9);

            numberStringsReverse = numberStrings.Select(x => new KeyValuePair<string, int>(new string(x.Key.Reverse().ToArray()), x.Value)).ToDictionary();

            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            List<char[]> lines = new List<char[]>();

            while (line != null)
            {
                Console.WriteLine(line);
                lines.Add(line.ToCharArray());
                line = sr.ReadLine();
            }

            sr.Close();

            int total = 0;

            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (var lineArray in lines)
            {
                string numberString = "";
                int i = 0;

                int? firstNumber = null;
                while (firstNumber == null)
                {
                    if (char.IsNumber(lineArray[i]))
                    {
                        firstNumber = Convert.ToInt32(lineArray[i].ToString());
                    }
                    else
                    {
                        numberString = NumberString(numberString, lineArray[i], false);
                        if (numberString != "")
                        {
                            if (numberStrings.ContainsKey(numberString))
                            {
                                firstNumber = numberStrings[numberString];
                            }
                        }
                        else
                        {
                            numberString = NumberString(numberString, lineArray[i], false);
                        }
                    }

                    i++;
                }

                numberString = "";
                i = lineArray.Length - 1;

                int? lastNumber = null;
                while (lastNumber == null)
                {
                    if (char.IsNumber(lineArray[i]))
                    {
                        lastNumber = Convert.ToInt32(lineArray[i].ToString());
                    }
                    else
                    {
                        numberString = NumberString(numberString, lineArray[i], true);
                        if (numberString != "")
                        {
                            if (numberStringsReverse.ContainsKey(numberString))
                            {
                                lastNumber = numberStringsReverse[numberString];
                            }
                        }
                        else
                        {
                            numberString = NumberString(numberString, lineArray[i], true);
                        }
                    }

                    i--;
                }

                Console.WriteLine(new string(lineArray) + " : " + ((int)(firstNumber * 10) + (int)(lastNumber ?? firstNumber)).ToString());

                result.Add(new string(lineArray), (int)(firstNumber * 10) + (int)(lastNumber ?? firstNumber));

                total = total + (int)(firstNumber * 10) + (int)(lastNumber ?? firstNumber);
            }

            string csv = string.Join(Environment.NewLine, result.Select(x => $"{x.Key},{x.Value.ToString()}"));
            File.WriteAllText(outputFile, csv);

            Console.WriteLine("Result is: " + total.ToString());
        }

        public string NumberString(string currentNumberString, char currentChar, bool isReverse)
        {
            if ((isReverse ? numberStringsReverse : numberStrings)
                .Where(y => y.Key.Length > currentNumberString.Length)
                .Any(x => x.Key.Substring(0, currentNumberString.Length + 1) == currentNumberString + currentChar.ToString())
                )
            {
                return currentNumberString + currentChar.ToString();
            }
            else if (currentNumberString.Length > 1)
            {
                return NumberString(currentNumberString.Substring(1), currentChar, isReverse);
            }
            else
            {
                return "";
            }
        }
    }
}
