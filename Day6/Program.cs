﻿using System.Collections;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            
            // Part 1
            foreach (var line in input)
            {
                Console.WriteLine($"Part 1: The maker is at position {GetStartOfPacketMarkerPosition(line, 4)}");  // 1300
            }
            
            // Part 2
            foreach (var line in input)
            {
                Console.WriteLine($"Part 2: The maker is at position {GetStartOfPacketMarkerPosition(line, 14)}"); // 3986
            }
        }

        private static int GetStartOfPacketMarkerPosition(string line, int markerLength)
        {
            var queue = new char[markerLength];
            var bv = new BitArray(128);
            
            for (var index = 0; index < line.Length; index++)
            {
                // Shift the array over one character to the right
                Array.Copy(queue, 0, queue, 1, markerLength - 1);
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
