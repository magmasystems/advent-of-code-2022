namespace AdventOfCode2022
{
    internal static class Program
    {
        private struct Accumulator
        {
            public int Cycle { get; set; }
            public int V { get; set; }

            public override string ToString()
            {
                return $"{nameof(Cycle)}: {Cycle}, {nameof(V)}: {V}";
            }
        }
        
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            // Part 1
            var cycle = 0;
            var idxInstruction = 0;
            var cycles = new Accumulator[input.Length];
            
            foreach (var line in input)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                switch (parts[0].ToLower())
                {
                    case "noop":
                        cycle++;
                        break;
                    case "addx":
                        cycle += 2;
                        cycles[idxInstruction].Cycle = cycle;
                        cycles[idxInstruction].V = Convert.ToInt32(parts[1]);
                        idxInstruction++;
                        break;
                }
            }

            // Find the signal strength during the 20th, 60th, 100th, 140th, 180th, and 220th cycles.
            // What is the sum of these six signal strengths?
            var cyclesToTest = new[] { 20, 60, 100, 140, 180, 220 };
            var register = 1;
            var sumOfSignalStrengths = 0;
            var prevCycleToTest = 0;
            
            foreach (var cycleToTest in cyclesToTest)
            {
                register += cycles.Where(c => c.Cycle >= prevCycleToTest && c.Cycle < cycleToTest).Sum(C => C.V);
                sumOfSignalStrengths += register * cycleToTest;
                prevCycleToTest = cycleToTest;
                // Console.WriteLine($"Cycle: {testPoint}, Register: {register}, Accumulator: {acc}");
            }
            
            Console.WriteLine($"Part 1: The sum is {sumOfSignalStrengths}"); // 15140
            
            // Part 2
        }
    }
}
