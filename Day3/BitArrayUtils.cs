using System.Collections;

namespace AdventOfCode2022;

public static class BitArrayUtils
{
    public static int FindIndexOf(this BitArray bitArray, Predicate<bool> predicate)
    {
        for (var i = 0; i < bitArray.Length; i++)
        {
            if (predicate(bitArray[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static BitArray And(this BitArray[] bitArrays)
    {
        var resultMap = new BitArray(bitArrays[0].Length, true);
        foreach (var bitArray in bitArrays)
        {
            resultMap = resultMap.And(bitArray);
        }

        return resultMap;
    }
}