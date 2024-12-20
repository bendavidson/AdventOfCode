using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    internal class Day09 : DayBase
    {
        public Day09() : base("2024", "Day09") { }

        protected override void Solve()
        {
            var inputArray = lines[0].Select(x => Convert.ToInt32(Char.GetNumericValue(x))).ToArray();

            int nextStart = 0;
            bool isFreeSpace = false;
            int currentId = 0;

            var blocks = new List<Block>();
            
            foreach (var set in inputArray)
            {
                
                var block = new Block
                {
                    Id = currentId,
                    IsFreeSpace = isFreeSpace,
                    Size = set,
                    Positions = Enumerable.Range(nextStart, set).ToArray()
                };

                blocks.Add(block);

                nextStart = nextStart + set;
                currentId = isFreeSpace ? currentId + 1 : currentId;
                isFreeSpace = !isFreeSpace;
            }

            foreach (var freeSpace in blocks.Where(x => x.IsFreeSpace && x.Size > 0).OrderBy(x => x.Id))
            {
                var currentSize = freeSpace.Size;

                while (currentSize > 0)
                {
                    var fileBlock = blocks.Where(x => !x.IsFreeSpace 
                                        && x.Positions.Any(p => p > freeSpace.Positions[freeSpace.Positions.Length - currentSize])
                                        && (partNo == 1 || x.Size <= currentSize))
                                                .OrderByDescending(x => x.Positions[0]).FirstOrDefault();

                    if (fileBlock == null)
                        break;

                    var firstPositionToSwap = Array.IndexOf(fileBlock.Positions,fileBlock.Positions.Where(p => p > freeSpace.Positions[freeSpace.Positions.Length - currentSize]).OrderDescending().First());
                    var positionsToSwap = Math.Min(fileBlock.Positions.Where(p => p > freeSpace.Positions[freeSpace.Positions.Length - currentSize]).ToArray().Length,currentSize);
                    
                    for(int i = 0 ; i <= positionsToSwap - 1; i++)
                    {
                        int filePositionToSwap = fileBlock.Positions[firstPositionToSwap - i];
                        fileBlock.Positions[firstPositionToSwap - i] = freeSpace.Positions[freeSpace.Size - currentSize + i];
                        freeSpace.Positions[i] = filePositionToSwap;
                    }

                    currentSize = currentSize - positionsToSwap;
                }
            }

            foreach (var block in blocks.Where(x => !x.IsFreeSpace))
            { 
                foreach(var position  in block.Positions)
                {
                    total = total + (position * block.Id);
                }
            }
        }

        private class Block
        {
            public int Id { get; set; }
            public bool IsFreeSpace { get; set; }
            public int Size {  get; set; }
            public int[] Positions { get; set; }
        }
    }
}
