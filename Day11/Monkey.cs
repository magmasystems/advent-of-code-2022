using System.Numerics;

namespace AdventOfCode2022;

public abstract class MonkeyBase
{
    public int Id { get; set; }
    public char Op { get; set; }
    public int Operand { get; set; }
    public int Divisor { get; set; }
    public int ThrowToIfTrue { get; set; }
    public int ThrowToIfFalse { get; set; }
    public ulong NumItemsInspected { get; set; }

    protected void Copy(MonkeyBase src)
    {
        this.Id = src.Id;
        this.Op = src.Op;
        this.Operand = src.Operand;
        this.Divisor = src.Divisor;
        this.ThrowToIfTrue = src.ThrowToIfTrue;
        this.ThrowToIfFalse = src.ThrowToIfFalse;
    }
}

public class Monkey : MonkeyBase
{
    public Queue<int> Items { get; } = new();
}

public class BigMonkey : MonkeyBase
{
    public Queue<BigInteger> Items { get; } = new();

    public BigMonkey(Monkey src)
    {
        this.Copy(src);
        foreach (var i in src.Items)
            this.Items.Enqueue(new BigInteger(i));
    }
}