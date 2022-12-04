using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var numCompleteOverlaps = 0;
            var numDoesNotOverlap = 0;
            
            var regex = new Regex(@"(?<min1>\d+)-(?<max1>\d+),(?<min2>\d+)-(?<max2>\d+)");
            
            foreach (var line in input)
            {
                var matches = regex.Matches(line);
                foreach (Match m in matches)
                {
                    var min1 = Convert.ToInt32(m.Groups["min1"].Value);
                    var max1 = Convert.ToInt32(m.Groups["max1"].Value);
                    var min2 = Convert.ToInt32(m.Groups["min2"].Value);
                    var max2 = Convert.ToInt32(m.Groups["max2"].Value);

                    if (min1 >= min2 && max1 <= max2 || min2 >= min1 && max2 <= max1)
                        numCompleteOverlaps++;
                    if (min1 > max2 || max1 < min2 || min2 > max1 || max2 < min1)
                        numDoesNotOverlap++;
                }
            }
            
            Console.WriteLine($"Part 1: The number of complete overlaps is {numCompleteOverlaps}");   // 483
            Console.WriteLine($"Part 2: The number of overlaps is {input.Length-numDoesNotOverlap}"); // 874
        }
    }
}
