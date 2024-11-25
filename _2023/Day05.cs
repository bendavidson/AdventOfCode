using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day05 : IPuzzle
    {
        string inputFile = "../../../Inputs/Day05.txt";
        string outputFile = "../../../Outputs/Day05.csv";

        public Day05()
        {
            List<Item> items = new List<Item>();
            List<ItemMap> itemMaps = new List<ItemMap>();

            StreamReader sr = new StreamReader(inputFile);
            string line = sr.ReadLine();

            int i = 0;
            Tuple<ItemType, ItemType> currentMap = null;

            Console.WriteLine("Are you solving Part 1 or Part 2?");
            var partNo = Convert.ToInt32(Console.ReadLine());

            while (line != null)
            {
                if (line.Length > 0)
                {
                    if (line.StartsWith("seeds:"))
                    {
                        var lineItems = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        for (i = 1; i < lineItems.Length; i += partNo)
                        {
                            items.Add(new Item { Type = ItemType.Seed, Number = Convert.ToInt64(lineItems[i]), RangeLength = partNo == 2 ? Convert.ToInt64(lineItems[i + 1]) : 1 });
                        }
                    }
                    else if (line.EndsWith("map:"))
                    {
                        var mapStrings = line.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        ItemType sourceType, destinationType;
                        if (Enum.TryParse(mapStrings[0], true, out sourceType) && Enum.TryParse(mapStrings[2], true, out destinationType))
                        {
                            currentMap = new Tuple<ItemType, ItemType>(sourceType, destinationType);
                        }
                        else
                        {
                            Console.WriteLine("Unknown map type");
                        }
                    }
                    else if (currentMap != null)
                    {
                        var mapDetails = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        itemMaps.Add(new ItemMap
                        {
                            SourceType = currentMap.Item1,
                            DestinationType = currentMap.Item2,
                            SourceStartNumber = Convert.ToInt64(mapDetails[1]),
                            DestinationStartNumber = Convert.ToInt64(mapDetails[0]),
                            RangeLength = Convert.ToInt64(mapDetails[2])
                        });
                    }

                }

                line = sr.ReadLine();
            }

            sr.Close();

            long total = 0;

            if (partNo == 1)
            {
                foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                {
                    var itemsToProcess = items.Where(x => x.Type == itemType).ToList();

                    foreach (var item in itemsToProcess)
                    {
                        var map =
                            itemMaps.FirstOrDefault(x => x.SourceType == itemType && item.Number >= x.SourceStartNumber && item.Number <= x.SourceEndNumber)
                            ?? new ItemMap
                            {
                                SourceType = itemType,
                                DestinationType = (ItemType)((int)itemType + 1),
                                SourceStartNumber = item.Number,
                                DestinationStartNumber = item.Number,
                                RangeLength = 1
                            };

                        item.ChildNumber = item.Number + map.Translation;

                        items.Add(new Item { Type = map.DestinationType, Number = item.Number + map.Translation });
                    }
                }

                total = items.Where(x => x.Type == ItemType.Location).OrderBy(x => x.Number).First().Number;
            }
            else
            {
                foreach (var item in items.Where(x => x.Type == ItemType.Seed))
                {
                    List<Item> seedItems = new List<Item>();
                    seedItems.Add(item);

                    foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                    {
                        var itemsToProcess = seedItems.Where(x => x.Type == itemType).ToList();

                        foreach (var procItem in itemsToProcess)
                        {
                            var maps = itemMaps.Where(x => x.SourceType == procItem.Type && x.SourceEndNumber >= procItem.Number && x.SourceStartNumber <= procItem.NumberEnd).ToList();

                            if (!maps.Any())
                            {
                                maps.Add(
                                    new ItemMap
                                    {
                                        SourceType = procItem.Type,
                                        DestinationType = (ItemType)((int)procItem.Type + 1),
                                        SourceStartNumber = procItem.Number,
                                        DestinationStartNumber = procItem.Number,
                                        RangeLength = procItem.RangeLength
                                    });
                            }


                            Item seedItem = null;
                            foreach (var map in maps.OrderBy(x => x.SourceStartNumber))
                            {
                                if (map.SourceStartNumber > (seedItem == null ? procItem.Number : seedItem.ChildNumber + 1))
                                {
                                    seedItem = new Item
                                    {
                                        Type = map.DestinationType,
                                        Number = seedItem == null ? procItem.Number : seedItem.ChildNumber + 1,
                                        RangeLength = map.SourceStartNumber - (seedItem == null ? procItem.Number : seedItem.ChildNumber + 1),
                                        ChildNumber = map.SourceStartNumber - 1
                                    };
                                    seedItems.Add(seedItem);
                                }

                                seedItem = new Item
                                {
                                    Type = map.DestinationType,
                                    Number = Math.Max(procItem.Number, map.SourceStartNumber) + map.Translation,
                                    RangeLength = Math.Min(procItem.NumberEnd, map.SourceEndNumber) + map.Translation - (Math.Max(procItem.Number, map.SourceStartNumber) + map.Translation) + 1,
                                    ChildNumber = Math.Min(procItem.NumberEnd, map.SourceEndNumber)
                                };

                                seedItems.Add(seedItem);
                            }

                            if (seedItem.ChildNumber < procItem.NumberEnd)
                            {
                                seedItem = new Item
                                {
                                    Type = (ItemType)((int)procItem.Type + 1),
                                    Number = seedItem.ChildNumber + 1,
                                    RangeLength = procItem.NumberEnd - seedItem.ChildNumber
                                };

                                seedItems.Add(seedItem);
                            }
                        }
                    }

                    if (total == 0)
                        total = seedItems.Where(x => x.Type == ItemType.Location).OrderBy(x => x.Number).First().Number;
                    else
                        total = Math.Min(total, seedItems.Where(x => x.Type == ItemType.Location).OrderBy(x => x.Number).First().Number);
                }
            }

            Console.WriteLine("Result is: " + total.ToString());

        }

        private class Item
        {
            public ItemType Type;
            public long Number;
            public long RangeLength;
            public long ChildNumber;
            public long NumberEnd { get { return Number + RangeLength - 1; } }
        }

        private class ItemMap
        {
            public ItemType SourceType;
            public ItemType DestinationType;
            public long SourceStartNumber;
            public long DestinationStartNumber;
            public long RangeLength;
            public long SourceEndNumber { get { return SourceStartNumber + RangeLength - 1; } }
            public long DestinationEndNumber { get { return DestinationStartNumber + RangeLength - 1; } }
            public long Translation { get { return DestinationStartNumber - SourceStartNumber; } }
        }

        private enum ItemType
        {
            None = 0,
            Seed = 1,
            Soil = 2,
            Fertilizer = 3,
            Water = 4,
            Light = 5,
            Temperature = 6,
            Humidity = 7,
            Location = 8
        }
    }
}
