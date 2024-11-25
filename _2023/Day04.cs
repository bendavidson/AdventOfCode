using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day04 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day04.txt";
        string outputFile = "../../../Outputs/Day04.csv";

        public Day04()
        {
            //Dictionary<int, Tuple<List<int>, List<int>>> lines = new Dictionary<int, Tuple<List<int>, List<int>>>();
            List<Card> cards = new List<Card>();

            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            int i = 0;

            while (line != null)
            {
                Console.WriteLine(line);
                var topSplit = line.Split(':');
                int cardNo = Convert.ToInt32(topSplit[0].Replace("Card ", "").Trim());

                var numberLists = topSplit[1].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var winningNumbers = numberLists[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var myNumbers = numberLists[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                cards.Add(new Card
                {
                    CardNumber = cardNo,
                    WinningNumbers = winningNumbers.Select(x => Convert.ToInt32(x)).ToList(),
                    MyNumbers = myNumbers.Select(x => Convert.ToInt32(x)).ToList(),
                    Instances = 1
                });

                line = sr.ReadLine();

                i++;
            }

            int total = 0;

            foreach (var card in cards)
            {
                var myWinningNumbers = card.MyNumbers.Intersect(card.WinningNumbers).Count();

                Console.WriteLine(card.CardNumber.ToString() + ": " + myWinningNumbers.ToString());

                card.MyWinningNumbers = myWinningNumbers;

                for (i = card.CardNumber; i <= card.CardNumber + myWinningNumbers - 1 && i <= cards.Count(); i++)
                {
                    cards[i].Instances += card.Instances;
                }

                if (myWinningNumbers > 0)
                {
                    total += (int)Math.Pow(2, myWinningNumbers - 1);
                }
            }

            Console.WriteLine("Result is: " + total.ToString());

            Console.WriteLine("Result for Part 2 is: " + cards.Sum(x => x.Instances));
        }

        private class Card
        {
            public int CardNumber;
            public List<int> WinningNumbers;
            public List<int> MyNumbers;
            public int MyWinningNumbers;
            public int Instances;
        }
    }
}
