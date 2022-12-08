namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var grid = new int[input.Length,input[0].Length];
            
            // Parse the input
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
            
            // Part 1
            
            // All trees at the edge of the grid are visible
            var numVisible = input.Length * 2 + (input[0].Length - 2) * 2;

            for (var row = 1; row < numRows - 1; row++)
            {
                for (var col = 1; col < numCols - 1; col++)
                {
                    numVisible += IsTreeVisible(grid, row, col) ? 1 : 0;
                }
            }

            Console.WriteLine($"Part 1: The number of trees that are visible are {numVisible}");  // 1854
            
            // Part 2
            var scenicScores = new List<int>();
            for (var row = 1; row < numRows - 1; row++)
            {
                for (var col = 1; col < numCols - 1; col++)
                {
                    scenicScores.Add(GetViewingArea(grid, row, col, numRows, numCols));
                }
            }
            
            Console.WriteLine($"Part 2: The max viewing area is {scenicScores.Max()}");         // 527340
        }

        private static bool IsTreeVisible(int[,] grid, int row, int col)
        {
            var tree = grid[row,col];
            
            if (grid.SliceColumn(col, 0, row).Max() < tree)
                return true;
            if (grid.SliceColumn(col, row+1).Max() < tree)
                return true;
            if (grid.SliceRow(row, 0, col).Max() < tree)
                return true;
            if (grid.SliceRow(row, col+1).Max() < tree)
                return true;

            return false;
        }
        
        private static int GetViewingArea(int[,] grid, int row, int col, int numRows, int numCols)
        {
            var tree = grid[row,col];
            var scores = new int[4];
            
            // Check up
            var score = 0;
            for (var r = row-1; r >= 0; r--)
            {
                score++;
                if (grid[r, col] >= tree)
                    break;
            }
            scores[0] = score;
            
            // Check down
            score = 0;
            for (var r = row+1; r < numRows; r++)
            {
                score++;
                if (grid[r, col] >= tree)
                    break;
            }
            scores[1] = score;
            
            // Check left
            score = 0;
            for (var c = col-1; c >= 0; c--)
            {
                score++;
                if (grid[row, c] >= tree)
                    break;
            }
            scores[2] = score;
            
            // Check right
            score = 0;
            for (var c = col+1; c < numCols; c++)
            {
                score++;
                if (grid[row, c] >= tree)
                    break;
            }
            scores[3] = score;

            score = 1;
            foreach (var t in scores)
                score *= t;

            return score;
        }
    }
}
