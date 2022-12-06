using System.Collections;
using System.Collections.Specialized;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            foreach (var line in input)
            {
                var n = GetMarkerPosition(line);
                Console.WriteLine($"Part 1: The maker is at position {n}"); // 1300
            }
        }

        private static int GetMarkerPosition(string line)
        {
            const int MARKERLENGTH = 4;
            
            var queue = new char[MARKERLENGTH];
            var bv = new BitArray(128);
            
            for (var index = 0; index < line.Length; index++)
            {
                // Shift the array over one character to the right
                for (var i = MARKERLENGTH-1; i > 0; i--)
                    queue[i] = queue[i - 1];
                queue[0] = line[index];

                if (index < 3)
                    continue;

                // Test for duplicates
                var duplicates = false;
                bv.SetAll(false);
                for (var i = 0; i < queue.Length && duplicates == false; i++)
                {
                    if (bv[queue[i]])
                        duplicates = true;
                    bv[queue[i]] = true;
                }

                if (duplicates == false)
                    return index+1;
            }

            return -1;
        }
    }
}
