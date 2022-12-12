namespace AdventOfCode2022;

public static class Parser
{
    public static NodeInfo[,] ParseInput(string[] args, out NodeInfo startNode, out NodeInfo endNode)
    {
        startNode = null;
        endNode = null;
        
        var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input2.txt");

        var matrix = new NodeInfo[input.Length, input[0].Length];

        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];
            for (var col = 0; col < line.Length; col++)
            {
                var node = new NodeInfo(line[col]);
                node.Name = $"{node.Name}:[{row}, {col}]";
                matrix[row, col] = node;

                switch (line[col])
                {
                    case 'S':
                        startNode = node;
                        break;
                    case 'E':
                        endNode = node;
                        break;
                }

                // Up
                if (row > 0)
                {
                    AddNeighbor(node, matrix[row - 1, col]);
                }

                // Left
                if (col > 0)
                {
                    AddNeighbor(node, matrix[row, col - 1]);
                }
            }
        }

        return matrix;
    }

    private static void AddNeighbor(NodeInfo node, NodeInfo neighbor)
    {
        if (Math.Abs(neighbor.Height - node.Height) <= 1)
        {
            node.Adjacent.Add(neighbor);
            neighbor.Adjacent.Add(node);
        }
        
        if (neighbor.Height >= node.Height)
        {
            if (!neighbor.Adjacent.Contains(node)) 
                neighbor.Adjacent.Add(node);
        }
    }
}