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
}