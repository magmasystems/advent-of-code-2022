using System.Collections;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static int CharToIndex(char ch) => char.IsLower(ch) ? ch - 'a' : ch - 'A' + 26;
        
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            // This is part 1
            var sumOfPriorities = input.Select(line => CalculatePriority(new[] { line[..(line.Length / 2)], line[(line.Length / 2)..] }, 0, 2)).Sum();
            Console.WriteLine($"Part 1: The sum of priorities is {sumOfPriorities}"); // 8401

            // This is part 2
            sumOfPriorities = 0;
            for (var idxLine = 0; idxLine < input.Length; idxLine += 3)
            {
                sumOfPriorities += CalculatePriority(input, idxLine, 3);
            }
            Console.WriteLine($"Part 2: The sum of priorities is {sumOfPriorities}"); // 2641
        }

        private static int CalculatePriority(string[] input, int startingIndex, int chunkSize)
        {
            var maps = new BitArray[chunkSize];
            for (var i = 0; i < maps.Length; i++)
            {
                maps[i] = input[startingIndex + i].CreateCharMap();
            }

            // Note: I probably could have used a single map in the solution, but I like and'ing chains of maps
            var resultMap = maps.And();

            // Find the first entry that is non-zero
            var idxNonZero = resultMap.FindIndexOf(element => element);
            return idxNonZero >= 0 ? idxNonZero + 1 : 0;
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
