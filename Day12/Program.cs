namespace AdventOfCode2022
{
    internal static class Program
    {
        private static NodeInfo[,] Matrix;
        private static NodeInfo StartNode;
        private static NodeInfo EndNode;
        private static readonly HashSet<string> FoundPaths = new();

        private static readonly bool UseBFS = true;
        
        private static void Main(string[] args)
        {
            Matrix = Parser.ParseInput(args, out StartNode, out EndNode);
            //Parser.DumpMatrix(Matrix);
            
            var minLength = FindMinimumPath(out var minPath);
            Console.WriteLine($"Part 1: Length is {minLength} and path is {minPath}"); // 
        }

        private static int FindMinimumPath(out string minPath)
        {
            if (UseBFS)
            {
                BFS();
            }
            else
            {
                foreach (var node in StartNode.Adjacent)
                {
                    var visited = new int[Matrix.GetLength(0) * Matrix.GetLength(1)];
                    visited[StartNode.Index] = 1;

                    FindPaths(node, visited, new string(StartNode.Name));
                }
            }

            var minLength = int.MaxValue;
            minPath = string.Empty;

            foreach (var path in FoundPaths)
            {
                var numNodes = path.Count(ch => ch == '|');
                if (numNodes < minLength)
                {
                    minLength = numNodes;
                    minPath = path;
                }
            }

            return minLength;
        }

        private static void BFS()
        {
            var visited = new bool[Matrix.GetLength(0) * Matrix.GetLength(1)];
            var path = string.Empty;
            var queue = new Queue<NodeInfo>();
            queue.Enqueue(StartNode);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (visited[node.Index])
                    continue;
                
                path += $"|{node.Name}";
                visited[node.Index] = true;
                
                if (node == EndNode)
                {
                    FoundPaths.Add(path);
                    Console.WriteLine(path);
                    return;
                }
                
                foreach (var child in node.Adjacent.Where(child => !visited[child.Index]))
                {
                    queue.Enqueue(child);
                }
            }
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
            if (visited[node.Index] == 1)
            {
                return;
            }

            path += $"|{node.Name}";
            visited[node.Index] = 1;
            //Console.WriteLine($"{path}");

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var node2 in node.Adjacent)
            {
                var oldVisited = new int[visited.Length];
                Array.Copy(visited, 0, oldVisited, 0, visited.Length);

                FindPaths(node2, visited, path);
                
                Array.Copy(oldVisited, 0, visited, 0, visited.Length);    
            }
        }
    }
}
