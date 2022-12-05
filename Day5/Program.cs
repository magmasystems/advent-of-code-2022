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
            var stacks = new List<Stack<char>>();
            var directions = new List<Direction>();
            var lineNumberOfBottomOfStack = 0;
            var rearrangementSection = false;
            
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
                        stacks.Add(new Stack<char>());
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
                    }
                }
            }

            // Execute the moving of the containers
            foreach (var direction in from direction in directions from _ in Enumerable.Range(0, direction.Quantity) select direction)
            {
                stacks[direction.Dest - 1].Push(stacks[direction.Source - 1].Pop());
            }

            // Create the message by looking at the top container in each stack
            var message = string.Empty;
            foreach (var stack in stacks)
            {
                if (stack.TryPeek(out var ch))
                {
                    message += ch;
                }
            }

            Console.WriteLine($"Part 1: The message is {message}");   // QPJPLMNNR
        }
    }
}