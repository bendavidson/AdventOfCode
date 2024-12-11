using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the year you are solving");
            var year = Console.ReadLine();
            Console.WriteLine("Enter the day you are solving");
            var dayNo = Console.ReadLine();
            Console.WriteLine("You are solving puzzles from day " + dayNo + ", " + year + ".");
            IPuzzle puzzle;
            
            var dayPuzzle = Type.GetType("AdventOfCode._" + year + ".Day" + dayNo.PadLeft(2,'0'));

            if (dayPuzzle != null)
            {
                puzzle = Activator.CreateInstance(dayPuzzle) as IPuzzle;
            }
            else
            {
                Console.WriteLine("No puzzle solution has been created for day " + dayNo + ", " + year + ".");
                Console.ReadLine();
            }
            
        }
    }
}
