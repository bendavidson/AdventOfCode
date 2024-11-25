using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day15 : DayBase
    {
        public Day15() : base("Day15") { }

        protected override void Solve()
        {
            string[] steps = null;

            foreach (var line in lines)
            {
                steps = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            var boxes = new List<Tuple<string, int>>[256];

            foreach (var step in steps)
            {
                var code = HashCode(step);

                if (partNo == 1)
                    total += code;
                else
                {
                    var box = boxes[code];
                    if (box == null)
                        box = new List<Tuple<string, int>>();
                    var operation = step.FirstOrDefault(x => x == '=' || x == '-');
                    var lensLabel = step.Substring(0, Array.IndexOf(step.ToCharArray(), operation));

                    if (operation == '-')
                    {
                        var lens = box.FirstOrDefault(x => x.Item1 == lensLabel);
                        if (lens != null)
                        {
                            box.Remove(lens);
                        }
                    }
                    else
                    {
                        var focalLength = operation == '=' ? Convert.ToInt32(step.Substring(step.Length - 1)) : 0;

                        var lens = box.FirstOrDefault(x => x.Item1 == lensLabel);
                        if (lens != null)
                        {
                            box[box.IndexOf(lens)] = new Tuple<string, int>(lensLabel, focalLength);
                        }
                        else
                        {
                            box.Add(new Tuple<string, int>(lensLabel, focalLength));
                        }
                    }

                    boxes[code] = box;
                }
            }

            foreach (var box in boxes.Select((b, i) => new { b, boxNo = i + 1 }).Where(x => x.b != null))
            {
                foreach (var lens in box.b.Select((l, i) => new { lensSlot = i + 1, focalLength = l.Item2 }))
                {
                    total += box.boxNo * lens.lensSlot * lens.focalLength;
                }
            }
        }

        private int HashCode(string step)
        {
            var code = 0;

            foreach (var character in step.ToCharArray())
            {
                if (partNo == 2 && (character == '=' || character == '-'))
                    return code;
                else
                {
                    code += character;
                    code = code * 17;
                    code = code % 256;
                }
            }
            return code;
        }
    }
}
