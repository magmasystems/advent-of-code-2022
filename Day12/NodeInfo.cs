namespace AdventOfCode2022;

public class NodeInfo
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