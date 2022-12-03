using System.Collections;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static int CharToIndex(char ch) => char.IsLower(ch) ? ch - 'a' : ch - 'A' + 26;
        
        private static void Main(string[] args)
        {
            // This is part 1

            var sumOfPriorities = 0;

            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var map = new BitArray(52);
                var halves = new[] { line[..(line.Length / 2)], line[(line.Length / 2)..] };

                foreach (var chIndex in halves[0].Select(CharToIndex))
                {
                    map.Set(chIndex, true);
                }

                foreach (var chIndex in halves[1].Select(CharToIndex))
                {
                    if (map[chIndex])
                    {
                        sumOfPriorities += chIndex + 1;
                        break;
                    }
                }
            }

            Console.WriteLine($"Part 1: The sum of priorities is {sumOfPriorities}"); // 8401

            // This is part 2
            
            // We need to re-process the lines of input and process the lines 3 at a time.
            // For each set of 3 lines, we need to find the common element in each line.

            sumOfPriorities = 0;
            for (var idxLine = 0; idxLine < input.Length && !string.IsNullOrEmpty(input[idxLine]); idxLine += 3)
            {
                var map = new BitArray[3];

                for (var idxElf = 0; idxElf < 3; idxElf++)
                {
                    map[idxElf] = new BitArray(52);
                    foreach (var chIndex in input[idxLine + idxElf].Select(CharToIndex))
                    {
                        map[idxElf].Set(chIndex, true);
                    }
                }

                // Note: I probably could have used a single map in the solution, but I like and'ing chains of maps
                var resultMap = map[0].And(map[1]).And(map[2]);

                // Find the first entry that is non-zero
                var idxNonZero = resultMap.FindIndexOf(element => element);
                if (idxNonZero >= 0)
                    sumOfPriorities += idxNonZero + 1;
            }

            Console.WriteLine($"Part 2: The sum of priorities is {sumOfPriorities}"); // 2641
        }
    }
}
