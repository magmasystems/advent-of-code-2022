namespace AdventOfCode2022
{
    internal static class Program
    {
        private class NodeInfo
        {
            public string Name { get; set; }
            public int Height { get; }
            public readonly List<NodeInfo> Adjacent = new();
            public int Index { get; }
            private static int Counter;

            public NodeInfo(char name)
            {
                this.Name = name.ToString();
                this.Index = Counter++;
                this.Height = name switch
                {
                    'S' => 0,
                    'E' => 25,
                    _ => name - 'a'
                };
            }
        }
        
        private static NodeInfo[,] Matrix;
        private static NodeInfo StartNode;
        private static NodeInfo EndNode;
        private static readonly HashSet<string> FoundPaths = new();
        
        private static void Main(string[] args)
        {
            ParseInput(args);

            foreach (var node2 in StartNode.Adjacent)
            {
                var visited = new int[Matrix.GetLength(0) * Matrix.GetLength(1)];
                FindPaths(node2, visited, new string(StartNode.Name));
            }

            var minLength = int.MaxValue;
            var minPath = string.Empty;

            foreach (var path in FoundPaths)
            {
                var numNodes = path.Count(ch => ch == '|');
                if (numNodes < minLength)
                {
                    minLength = numNodes;
                    minPath = path;
                }
            }

            // Part 1
            Console.WriteLine($"Part 1: Length is {minLength} and path is {minPath}"); // 
        }
        
        private static void FindPaths(NodeInfo node, int[] visited, string path)
        {
            // Success if we reached the end node
            if (node == EndNode)
            {
                path += $"|{node.Name}";
                FoundPaths.Add(path);
                //Console.WriteLine(path);
                return;
            }
            
            // Don't visit a node twice
            if (node == StartNode || visited[node.Index] == 1)
            {
                return;
            }

            path += $"|{node.Name}";
            visited[node.Index] = 1;

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var node2 in node.Adjacent)
            {
                var oldVisited = new int[visited.Length];
                Array.Copy(visited, 0, oldVisited, 0, visited.Length);

                FindPaths(node2, visited, path);
                
                Array.Copy(oldVisited, 0, visited, 0, visited.Length);    
            }
        }

        private static void ParseInput(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input2.txt");

            Matrix = new NodeInfo[input.Length, input[0].Length];

            for (var row = 0; row < input.Length; row++)
            {
                var line = input[row];
                for (var col = 0; col < line.Length; col++)
                {
                    var node = new NodeInfo(line[col]);
                    node.Name = $"{node.Name}:[{row}, {col}]";
                    Matrix[row, col] = node;

                    switch (line[col])
                    {
                        case 'S':
                            StartNode = node;
                            break;
                        case 'E':
                            EndNode = node;
                            break;
                    }

                    // Up
                    if (row > 0)
                    {
                        AddNeighbor(node, Matrix[row - 1, col]);
                    }

                    // Left
                    if (col > 0)
                    {
                        AddNeighbor(node, Matrix[row, col - 1]);
                    }
                }
            }
        }

        private static void AddNeighbor(NodeInfo node, NodeInfo neighbor)
        {
            if (Math.Abs(neighbor.Height - node.Height) <= 1)
            {
                node.Adjacent.Add(neighbor);
                neighbor.Adjacent.Add(node);
            }
        }
    }
}
