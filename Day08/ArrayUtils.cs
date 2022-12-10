namespace AdventOfCode2022;

public static class ArrayUtils
{
    public static IEnumerable<T> SliceRow<T>(this T[,] array, int row, int startCol, int endCol = -1)
    {
        if (endCol == -1)
            endCol = array.GetLength(1);
        
        for (var col = startCol; col < endCol; col++)
        {
            yield return array[row, col];
        }
    }

    public static IEnumerable<T> SliceColumn<T>(this T[,] array, int col, int startRow, int endRow = -1)
    {
        if (endRow == -1)
            endRow = array.GetLength(0);
        
        for (var row = startRow; row < endRow; row++)
        {
            yield return array[row, col];
        }
    }
}