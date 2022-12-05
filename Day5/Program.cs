using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private class Direction
        {
            public int Quantity { get; set; }
            public int Source { get; set; }
            public int Dest { get; set; }
        }
        
        private static void Main(string[] args)
        {
            var stacks = new List<Stack<char>>();    // Stacks used for part 1
            var stacks2 = new List<ConcurrentStack<char>>();   // An exact clone of the stacks used in part 1, but this is for part 2
            var directions = new List<Direction>();  // The list of directions for the rearrangement
            var lineNumberOfBottomOfStack = 0;
            var rearrangementSection = false;
            
            #region Parse the Input
            // move 1 from 2 to 1
            var regexMove = new Regex(@"^move (?<quantity>\d+) from (?<sourceStack>\d+) to (?<destStack>\d+)$");
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var lineNum = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                
                if (line.StartsWith("move", StringComparison.OrdinalIgnoreCase))
                {
                    rearrangementSection = true;
                }
                else if (char.IsDigit(line.TrimStart()[0]))
                {
                    lineNumberOfBottomOfStack = lineNum - 1;
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var _ in Enumerable.Range(0, parts.Length))
                    {
                        stacks.Add(new Stack<char>());
                        stacks2.Add(new ConcurrentStack<char>());
                    }
                }

                if (rearrangementSection)
                {
                    var matches = regexMove.Matches(line);
                    foreach (Match m in matches)
                    {
                        var direction = new Direction
                        {
                            Quantity = Convert.ToInt32(m.Groups["quantity"].Value),
                            Source = Convert.ToInt32(m.Groups["sourceStack"].Value),
                            Dest = Convert.ToInt32(m.Groups["destStack"].Value),
                        };
                        directions.Add(direction);
                    }
                }

                lineNum++;
            }

            // See which column a container is in and push it onto the corresponding stack
            while (lineNumberOfBottomOfStack >= 0)
            {
                var line = input[lineNumberOfBottomOfStack--];
                for (var col = 0; col < line.Length; col++)
                {
                    if (line[col] == '[')
                    {
                        stacks[col / 4].Push(line[col+1]);
                        stacks2[col / 4].Push(line[col+1]);
                    }
                }
            }
            #endregion

            // Execute the moving of the containers
            
            // Part1
            foreach (var direction in from direction in directions from _ in Enumerable.Range(0, direction.Quantity) select direction)
            {
                stacks[direction.Dest - 1].Push(stacks[direction.Source - 1].Pop());
            }
            
            // Part2
            foreach (var direction in directions)
            {
                // Use a temporary list to hold the reverse order of the popping
                var temp = new char[direction.Quantity];
                stacks2[direction.Source - 1].TryPopRange(temp, 0, direction.Quantity);  // [D][N][Z] => { Z N D }
                stacks2[direction.Dest - 1].PushRange(temp.Reverse().ToArray());
            }

            // Create the messages by looking at the top container in each stack
            Console.WriteLine($"Part 1: The message is {TopOfStacksToMessage(stacks)}");   // QPJPLMNNR
            Console.WriteLine($"Part 2: The message is {TopOfStacksToMessage(stacks2)}");  // BQDNWJPVJ
        }

        private static string TopOfStacksToMessage(List<Stack<char>> stacks)
        {
            var message = string.Empty;
            foreach (var stack in stacks)
            {
                if (stack.TryPeek(out var ch))
                {
                    message += ch;
                }
            }

            return message;
        }
        
        private static string TopOfStacksToMessage(List<ConcurrentStack<char>> stacks)
        {
            var message = string.Empty;
            foreach (var stack in stacks)
            {
                if (stack.TryPeek(out var ch))
                {
                    message += ch;
                }
            }

            return message;
        }
    }
}