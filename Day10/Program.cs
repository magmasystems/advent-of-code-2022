using System.Collections;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private struct Instruction
        {
            public int Cycle { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return $"{nameof(Cycle)}: {Cycle}, {nameof(Value)}: {Value}";
            }
        }
        
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            // Part 1
            var cycle = 0;
            var idxInstruction = 0;
            var instructions = new Instruction[input.Length];
            
            foreach (var line in input)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                switch (parts[0].ToLower())
                {
                    case "noop":
                        instructions[idxInstruction].Cycle = ++cycle;
                        break;
                    case "addx":
                        cycle += 2;
                        instructions[idxInstruction].Cycle = cycle;
                        instructions[idxInstruction].Value = Convert.ToInt32(parts[1]);
                        break;
                }
                idxInstruction++;
            }

            // Find the signal strength during the 20th, 60th, 100th, 140th, 180th, and 220th cycles.
            // What is the sum of these six signal strengths?
            var cyclesToTest = new[] { 20, 60, 100, 140, 180, 220 };
            var register = 1;
            var sumOfSignalStrengths = 0;
            var prevCycleToTest = 0;
            
            foreach (var cycleToTest in cyclesToTest)
            {
                register += instructions.Where(c => c.Cycle >= prevCycleToTest && c.Cycle < cycleToTest).Sum(C => C.Value);
                sumOfSignalStrengths += register * cycleToTest;
                prevCycleToTest = cycleToTest;
                // Console.WriteLine($"Cycle: {testPoint}, Register: {register}, Accumulator: {acc}");
            }
            
            Console.WriteLine($"Part 1: The sum is {sumOfSignalStrengths}"); // 15140
            
            // Part 2
            const int MAXCYCLES = 240;
            const int NUMCOLUMNS = 40;
            
            var crt = new BitArray(MAXCYCLES, false);
            var spritePosition = 1;
            idxInstruction = 0;
            register = 1;
            
            for (cycle = 1; cycle <= MAXCYCLES; cycle++)
            {
                // If the column that we are drawing in overlaps with the 3-column sprite, then turn on the pixel in the CRT
                var col = (cycle - 1) % NUMCOLUMNS;
                if (col >= spritePosition - 1 && col <= spritePosition + 1)
                {
                    crt[cycle - 1] = true;
                }
                
                if (cycle >= instructions[idxInstruction].Cycle)
                {
                    register += instructions[idxInstruction].Value;
                    spritePosition = register;
                    idxInstruction++;
                }
            }
            
            Console.WriteLine("Part 2: The letters are");

            for (var i = 0; i < 240; i++)
            {
                if (i % 40 == 0 && i > 0)
                {
                    Console.WriteLine();
                }
                if (i % 5 == 0)
                    Console.Write("     ");
                Console.Write(crt[i] ? "█" : '.');
            }
        }
    }
}
