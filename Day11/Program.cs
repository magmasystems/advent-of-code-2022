namespace AdventOfCode2022
{
    internal static class Program
    {
        private static int NUM_ROUNDS = 20;
        private static List<Monkey> Monkeys { get; set; } = new();
        private const int SENTINEL = int.MaxValue;
        
        private static void Main(string[] args)
        {
            var inputFile = args.Length > 0 ? args[0] : "Input2.txt";
            
            // Part 1
            ParseInput(inputFile);

            for (var round = 0; round < NUM_ROUNDS; round++)
            {
                foreach (var monkey in Monkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        monkey.NumItemsInspected++;
                        var worryLevel = monkey.Items.Dequeue();
                        var operand = monkey.Operand == SENTINEL ? worryLevel : monkey.Operand;
                        worryLevel = monkey.Op switch
                        {
                            '+' => worryLevel + operand,
                            '*' => worryLevel * operand,
                            _ => throw new ApplicationException($"Unknown operand {monkey.Op}")
                        };
                        worryLevel = Convert.ToInt32(Math.Round((decimal) worryLevel / 3, MidpointRounding.ToNegativeInfinity));
                        if (worryLevel % monkey.Divisor == 0)
                        {
                            var trueMonkey = Monkeys.FirstOrDefault(m => m.Id == monkey.ThrowToIfTrue);
                            trueMonkey?.Items.Enqueue(worryLevel);
                        }
                        else
                        {
                            var falseMonkey = Monkeys.FirstOrDefault(m => m.Id == monkey.ThrowToIfFalse);
                            falseMonkey?.Items.Enqueue(worryLevel);
                        }
                    }
                }
            }

            var topTwo = Monkeys.OrderBy(m => m.NumItemsInspected).Reverse().Take(2).ToList();
            Console.WriteLine($"Part 1: product of the top two is {topTwo[0].NumItemsInspected * topTwo[1].NumItemsInspected}"); // 58056
            
            // Part 2
            
            // Reset the initial state
            Monkeys = new List<Monkey>();
            ParseInput(inputFile);
            NUM_ROUNDS = 1000;

            var BigMonkeys = Monkeys.Select(m => new BigMonkey(m)).ToList();

            for (var round = 1; round <= NUM_ROUNDS; round++)
            {
                foreach (var monkey in BigMonkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        monkey.NumItemsInspected++;
                        var worryLevel = monkey.Items.Dequeue();
                        worryLevel = monkey.Op switch
                        {
                            '+' => worryLevel + (monkey.Operand == SENTINEL ? worryLevel : monkey.Operand),
                            '*' => worryLevel * (monkey.Operand == SENTINEL ? worryLevel : monkey.Operand),
                            _ => throw new ApplicationException($"Unknown operand {monkey.Op}")
                        };
                        
                        if (worryLevel % monkey.Divisor == 0)
                        {
                            var trueMonkey = BigMonkeys.FirstOrDefault(m => m.Id == monkey.ThrowToIfTrue);
                            trueMonkey?.Items.Enqueue(worryLevel);
                        }
                        else
                        {
                            var falseMonkey = BigMonkeys.FirstOrDefault(m => m.Id == monkey.ThrowToIfFalse);
                            falseMonkey?.Items.Enqueue(worryLevel);
                        }
                    }
                }

                if (round % 50 == 0 /*|| round is 1 or 20*/)
                {
                    Console.WriteLine($"== After round {round} ==");
                    foreach (var m in BigMonkeys)
                        Console.WriteLine($"BigMonkey {m.Id} inspected items {m.NumItemsInspected} times");
                    Console.WriteLine();

                    //DumpQueuedItems();
                    Console.WriteLine();
                }
            }

            var topTwoBig = BigMonkeys.OrderBy(m => m.NumItemsInspected).Reverse().Take(2).ToList();
            Console.WriteLine($"Part 2: product of the top two is {topTwoBig[0].NumItemsInspected * topTwoBig[1].NumItemsInspected}"); //
        }

        private static void DumpQueuedItems()
        {
            foreach (var m in Monkeys)
            {
                Console.WriteLine($"Monkey {m.Id} Items:");
                foreach (var item in m.Items)
                {
                    Console.WriteLine($"{item}");
                }
            }
        }

        private static void ParseInput(string fileName)
        {
            var input = File.ReadAllLines(fileName);
            Monkey currMonkey = null;

            // Part 1
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var idxLine = 0; idxLine < input.Length; idxLine++)
            {
                if (string.IsNullOrEmpty(input[idxLine]))
                    continue;
                
                /*
                 Monkey 1:
                  Starting items: 54, 65, 75, 74
                  Operation: new = old + 6
                  Test: divisible by 19
                    If true: throw to monkey 2
                    If false: throw to monkey 0
                 */
                
                var line = input[idxLine].TrimStart();
                var parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.StartsWith("Monkey"))
                {
                    currMonkey = new Monkey();
                    Monkeys.Add(currMonkey);
                    currMonkey.Id = Convert.ToInt32(parts[1].Replace(":", string.Empty));
                }
                else if (line.StartsWith("Starting items") && currMonkey != null)
                {
                    for (var idxItem = 2; idxItem < parts.Length; idxItem++)
                    {
                        currMonkey.Items.Enqueue(Convert.ToInt32(parts[idxItem]));
                    }
                }
                else if (line.StartsWith("Operation") && currMonkey != null)
                {
                    // Operation: new = old + 6
                    currMonkey.Op = parts[4][0];
                    currMonkey.Operand = parts[5] != "old" ? Convert.ToInt32(parts[5]) : SENTINEL;
                }
                else if (line.StartsWith("Test") && currMonkey != null)
                {
                    // Test: divisible by 19
                    currMonkey.Divisor = Convert.ToInt32(parts[3]);
                }
                else if (line.StartsWith("If true") && currMonkey != null)
                {
                    currMonkey.ThrowToIfTrue = Convert.ToInt32(parts[5]);
                }
                else if (line.StartsWith("If false") && currMonkey != null)
                {
                    currMonkey.ThrowToIfFalse = Convert.ToInt32(parts[5]);
                }
                else
                {
                    throw new ApplicationException($"Unparseable line: {line}");
                }
            }
        }
    }
}
