namespace AdventOfCode2022;

internal class INode
{
    public string Name { get; init; }
    public bool IsDirectory { get; init; }
    public uint Size { get; set; }

    public List<INode> Children { get; } = new();
    public INode Parent { get; init; }

    public void Add(INode node)
    {
        this.Children.Add(node);
    }

    public INode FindDirectory(string directory, INode currentNode)
    {
        if (directory == "..")
        {
            return currentNode.Parent ?? currentNode;
        }

        var directoryNode = currentNode.Children.FirstOrDefault(n => n.Name.Equals(directory));
        return directoryNode ?? currentNode;
    }
}