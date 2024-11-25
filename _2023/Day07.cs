using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day07 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day07.txt";
        string outputFile = "../../../Outputs/Day07.csv";
        int partNo = 0;

        public Day07()
        {
            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            Console.WriteLine("Are you solving Part 1 or Part 2?");
            partNo = Convert.ToInt32(Console.ReadLine());

            List<Hand> hands = new List<Hand>();

            int i = 0;

            while (line != null)
            {
                var lineParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Hand hand = new Hand
                {
                    Id = i,
                    CardString = lineParts[0],
                    Bid = Convert.ToInt32(lineParts[1]),
                    HandType = CalculateHandType(lineParts[0]),
                };

                hands.Add(hand);

                i++;
                line = sr.ReadLine();
            }

            sr.Close();

            long total = 0;
            i = 1;

            foreach (var hand in hands
                        .OrderBy(h => h.HandType)
                        .ThenBy(h => h.Card1 == Card.J && partNo == 2 ? Card.Joker : h.Card1)
                        .ThenBy(h => h.Card2 == Card.J && partNo == 2 ? Card.Joker : h.Card2)
                        .ThenBy(h => h.Card3 == Card.J && partNo == 2 ? Card.Joker : h.Card3)
                        .ThenBy(h => h.Card4 == Card.J && partNo == 2 ? Card.Joker : h.Card4)
                        .ThenBy(h => h.Card5 == Card.J && partNo == 2 ? Card.Joker : h.Card5))
            {
                hand.Rank = i;
                total += hand.Bid * i;
                i++;
                if (hand.CardString.Contains('J'))
                    Console.WriteLine(hand.CardString + ": " + hand.HandType.ToString());
            }

            Console.WriteLine("Result is: " + total.ToString());
        }

        private HandType CalculateHandType(string cardString)
        {
            var distinctCards = cardString.GroupBy(x => x).Select(x => new { Card = x.Key, Occurances = x.Count() });

            if (distinctCards.Any(x => x.Occurances == 5))
                return HandType.FiveOfAKind;

            if (distinctCards.Any(x => x.Occurances == 4))
            {
                if (distinctCards.Any(x => x.Card == 'J') && partNo == 2)
                    return HandType.FiveOfAKind;
                else
                    return HandType.FourOfAKind;
            }

            if (distinctCards.Count() == 2)
            {
                if (distinctCards.Any(x => x.Card == 'J') && partNo == 2)
                    return HandType.FiveOfAKind;
                else
                    return HandType.FullHouse;
            }

            if (distinctCards.Any(x => x.Occurances == 3))
            {
                if (distinctCards.Any(x => x.Card == 'J') && partNo == 2)
                    return HandType.FourOfAKind;
                else
                    return HandType.ThreeOfAKind;
            }

            if (distinctCards.Where(x => x.Occurances == 2).Count() == 2)
            {
                if (distinctCards.Any(x => x.Card == 'J') && partNo == 2)
                {
                    if (distinctCards.FirstOrDefault(x => x.Card == 'J').Occurances == 2)
                        return HandType.FourOfAKind;
                    else
                        return HandType.FullHouse;
                }
                else
                    return HandType.TwoPair;
            }

            if (distinctCards.Any(x => x.Occurances == 2))
            {
                if (distinctCards.Any(x => x.Card == 'J') && partNo == 2)
                    return HandType.ThreeOfAKind;
                else
                    return HandType.OnePair;
            }

            if (distinctCards.Any(x => x.Card == 'J') && partNo == 2)
                return HandType.OnePair;

            return HandType.HighCard;
        }

        private class Hand
        {
            public int Id;
            public string CardString;
            public Card Card1 { get { Enum.TryParse(CardString.Substring(0, 1), true, out Card card); return card; } }
            public Card Card2 { get { Enum.TryParse(CardString.Substring(1, 1), true, out Card card); return card; } }
            public Card Card3 { get { Enum.TryParse(CardString.Substring(2, 1), true, out Card card); return card; } }
            public Card Card4 { get { Enum.TryParse(CardString.Substring(3, 1), true, out Card card); return card; } }
            public Card Card5 { get { Enum.TryParse(CardString.Substring(4, 1), true, out Card card); return card; } }
            public int Bid;
            public HandType HandType;
            public int Rank;
        }

        private enum Card
        {
            Joker = 1,
            [Display(Name = "2")]
            Two = 2,
            [Display(Name = "3")]
            Three = 3,
            [Display(Name = "4")]
            Four = 4,
            [Display(Name = "5")]
            Five = 5,
            [Display(Name = "6")]
            Six = 6,
            [Display(Name = "7")]
            Seven = 7,
            [Display(Name = "8")]
            Eight = 8,
            [Display(Name = "9")]
            Nine = 9,
            T = 10,
            J = 11,
            Q = 12,
            K = 13,
            A = 14
        }

        private enum HandType
        {
            HighCard = 0,
            OnePair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            FullHouse = 4,
            FourOfAKind = 5,
            FiveOfAKind = 6
        }
    }
}
