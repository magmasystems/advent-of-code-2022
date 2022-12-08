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
            for (var i = 0; i < 4; i++)
                score *= scores[i];
            return score;
        }

    }
}
