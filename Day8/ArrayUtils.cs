namespace AdventOfCode2022;

public static class ArrayUtils
{
    public static IEnumerable<T> SliceRow<T>(this T[,] array, int row, int colToExclude = -1)
    {
        for (var col = 0; col < array.GetLength(1); col++)
        {
            if (colToExclude == -1 || col != colToExclude)
                yield return array[row, col];
        }
    }

    public static IEnumerable<T> SliceColumn<T>(this T[,] array, int col, int rowToExclude = -1)
    {
        for (var row = 0; row < array.GetLength(0); row++)
        {
            if (rowToExclude == -1 || row != rowToExclude)
                yield return array[row, col];
        }
    }
}