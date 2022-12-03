using System.Collections;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static int CharToIndex(char ch) => char.IsLower(ch) ? ch - 'a' : ch - 'A' + 26;
        
        private static void Main(string[] args)
        {
            // This is part 1
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var sumOfPriorities = 0;
            foreach (var line in input)
            {
                var halves = new[] { line[..(line.Length / 2)], line[(line.Length / 2)..] };

                var maps = new BitArray[2];
                for (var i = 0; i < maps.Length; i++)
                {
                    maps[i] = halves[i].CreateCharMap();
                }

                // Note: I probably could have used a single map in the solution, but I like and'ing chains of maps
                var resultMap = maps.And();

                // Find the first entry that is non-zero
                var idxNonZero = resultMap.FindIndexOf(element => element);
                if (idxNonZero >= 0)
                    sumOfPriorities += idxNonZero + 1;
            }

            Console.WriteLine($"Part 1: The sum of priorities is {sumOfPriorities}"); // 8401

            // This is part 2
            
            // We need to re-process the lines of input and process the lines 3 at a time.
            // For each set of 3 lines, we need to find the common element in each line.

            sumOfPriorities = 0;
            for (var idxLine = 0; idxLine < input.Length; idxLine += 3)
            {
                var maps = new BitArray[3];
                for (var i = 0; i < maps.Length; i++)
                {
                    maps[i] = input[idxLine + i].CreateCharMap();
                }

                // Note: I probably could have used a single map in the solution, but I like and'ing chains of maps
                var resultMap = maps.And();

                // Find the first entry that is non-zero
                var idxNonZero = resultMap.FindIndexOf(element => element);
                if (idxNonZero >= 0)
                    sumOfPriorities += idxNonZero + 1;
            }

            Console.WriteLine($"Part 2: The sum of priorities is {sumOfPriorities}"); // 2641
        }

        private static BitArray CreateCharMap(this string line)
        {
            var map = new BitArray(52);
            foreach (var chIndex in line.Select(CharToIndex))
            {
                map.Set(chIndex, true);
            }

            return map;
        }
    }
}
