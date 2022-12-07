namespace AdventOfCode2022
{
    internal static class Program
    {
        private static readonly Node FileSystem = new() { Name = "/", IsDirectory = true };
        private static Node CurrentNode { get; set; } = null!;

        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            CurrentNode = FileSystem;
            BuildFileSystem(input);
            CalculateSizes(FileSystem);

            // Part 1
            uint sumOfSizes = 0;
            FindDirectoriesWithSizeBelow100001(FileSystem, ref sumOfSizes);
            Console.WriteLine($"Part 1: The sum of the directory sizes is {sumOfSizes}"); // 1743217

            // Part 2
            const uint TOTAL_DISK_SPACE = 70000000;
            const uint FREE_DISK_SPACE_NEEDED = 30000000;
            var diskSpaceToBeFreed = FREE_DISK_SPACE_NEEDED - (TOTAL_DISK_SPACE - FileSystem.DirectorySize);
            var nodeList = GetSmallestDirectoryWithSpaceAbove(FileSystem, diskSpaceToBeFreed, new List<Node>());
            Console.WriteLine($"Part 2: The directory size to be deleted is {nodeList.Min(n => n.DirectorySize)}"); // 8319096
        }

        private static void BuildFileSystem(string[] input)
        {
            for (var lineNum = 0; lineNum < input.Length; lineNum++)
            {
                var line = input[lineNum];
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (!parts[0].StartsWith("$"))
                    continue;

                lineNum = parts[1].ToLower() switch
                {
                    "ls" => ProcessLSCommand(input, lineNum),
                    "cd" => ProcessCDCommand(parts[2], lineNum),
                    _ => lineNum
                };
            }
        }

        private static int ProcessCDCommand(string directory, int lineNum)
        {
            CurrentNode = CurrentNode.FindDirectory(directory, CurrentNode);
            return lineNum;
        }

        private static int ProcessLSCommand(IReadOnlyList<string> input, int lineNum)
        {
            for (lineNum++ ; lineNum < input.Count; lineNum++)
            {
                var line = input[lineNum];
                if (line.StartsWith("$"))
                {
                    break;
                }
                
                // The output can be in one of the following forms:
                //   dir a
                //   14848514 b.txt
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts[0].ToLower() == "dir")
                {
                    // Add a directory node under the current location
                    CurrentNode.Add(new Node { Name = parts[1], IsDirectory = true, Size = 0, Parent = CurrentNode });
                }
                else
                {
                    // Add a file node under the current location
                    CurrentNode.Add(new Node { Name = parts[1], IsDirectory = false, Size = Convert.ToUInt32(parts[0]) });
                }
            }

            return lineNum - 1;
        }

        private static uint CalculateSizes(Node? node)
        {
            if (node == null)
                return 0;
            
            if (node.IsDirectory)
            {
                node.DirectorySize = node.Children.Aggregate<Node?, uint>(0, (current, child) => current + CalculateSizes(child));
                return node.DirectorySize;
            }

            return node.Size;
        }
        
        private static void FindDirectoriesWithSizeBelow100001(Node node, ref uint sum)
        {
            foreach (var child in node.Children.Where(ch => ch.IsDirectory))
            {
                FindDirectoriesWithSizeBelow100001(child, ref sum);
            }

            sum += node.DirectorySize <= 100000 ? node.DirectorySize : 0;
        }
        
        private static List<Node> GetSmallestDirectoryWithSpaceAbove(Node node, uint diskSpaceToBeFreed, List<Node> nodeList)
        {
            foreach (var child in node.Children.Where(ch => ch.IsDirectory))
            {
                GetSmallestDirectoryWithSpaceAbove(child, diskSpaceToBeFreed, nodeList);
            }

            if (node.DirectorySize >= diskSpaceToBeFreed)
                nodeList.Add(node);
            return nodeList;
        }
    }
}