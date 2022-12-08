namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var grid = new int[input.Length,input[0].Length];
            
            // Part 1
            for (var row = 0; row < input.Length; row++)
            {
                var line = input[row];
                for (var col = 0; col < line.Length; col++)
                {
                    grid[row, col] = line[col] - '0';
                }
            }

            var numRows = grid.GetLength(0);
            var numCols = grid.GetLength(1);

            // All trees at the edge of the grid are visible
            var numVisible = input.Length * 2 + (input[0].Length - 2) * 2;

            for (var row = 1; row < numRows - 1; row++)
            {
                for (var col = 1; col < numCols - 1; col++)
                {
                    var isVisible = IsTreeVisible(grid, row, col, numRows, numCols);
                    if (isVisible)
                        numVisible++;
                }
            }

            Console.WriteLine($"Part 1: The number of trees that are visible are {numVisible}");  // 1854
        }

        private static bool IsTreeVisible(int[,] grid, int row, int col, int numRows, int numCols)
        {
            var tree = grid[row,col];

            // Check up - it's visible if all trees are smaller
            var isVisible = true;
            for (var r = 0; r < row && isVisible;  r++)
                if (grid[r, col] >= tree)
                    isVisible = false;
            if (isVisible)
                return true;
            
            // Check down
            isVisible = true;
            for (var r = row+1; r < numRows && isVisible;  r++)
                if (grid[r, col] >= tree)
                    isVisible = false;
            if (isVisible)
                return true;
            
            // Check left
            isVisible = true;
            for (var c = 0; c < col && isVisible;  c++)
                if (grid[row, c] >= tree)
                    isVisible = false;
            if (isVisible)
                return true;
            
            // Check right
            isVisible = true;
            for (var c = col+1; c < numCols && isVisible;  c++)
                if (grid[row, c] >= tree)
                    isVisible = false;
            if (isVisible)
                return true;

            return false;
        }
        
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
}
