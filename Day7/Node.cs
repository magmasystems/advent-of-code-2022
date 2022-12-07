namespace AdventOfCode2022;

internal class Node
{
    public string Name { get; init; }
    public bool IsDirectory { get; init; }
    public uint Size { get; init; }
    public uint DirectorySize { get; set; }

    public List<Node> Children { get; } = new();
    public Node? Parent { get; init; }

    public void Add(Node node)
    {
        this.Children.Add(node);
    }

    public Node FindDirectory(string directory, Node currentNode)
    {
        if (directory == "..")
        {
            return currentNode.Parent ?? currentNode;
        }

        var directoryNode = currentNode.Children.FirstOrDefault(n => n.Name.Equals(directory));
        return directoryNode ?? currentNode;
    }
}