using System.Drawing;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static Point PointH = new(0, 0);
        private static Point PointT = new(0, 0);
        private static readonly HashSet<Point> PointsVisited = new();
        
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            
            // Parse the input
            foreach (var line in input)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var numMoves = Convert.ToInt32(parts[1]);
                
                // The tail visited the starting point
                PointsVisited.Add(PointT);

                var offset = GetOffsetFromDirection(parts[0][0]);
                foreach (var _ in Enumerable.Range(0, numMoves))
                {
                    PointH.Offset(offset);
                    PointT = MoveTail(PointH, PointT, offset);
                    if (!PointsVisited.Contains(PointT))
                        PointsVisited.Add(PointT);
                    // Console.WriteLine($"Head [{PointH.X}, {PointH.Y}] - Tail [{PointT.X}, {PointT.Y}]");
                }
                // Console.WriteLine("----------------------------------\n");
            }
            
            Console.WriteLine($"Part 1: The number of positions the tail visited is {PointsVisited.Count}");  // 5981
        }

        private static Point MoveTail(Point pointH, Point pointT, Point offset)
        {
            // See if the tail is still touching the head
            if (pointH.IsTouching(pointT))
                return pointT;
            
            // Otherwise, if the head and tail aren't touching and aren't in the same row or column, 
            // the tail always moves one step diagonally to keep up.
            if (pointT.X == pointH.X || pointT.Y == pointH.Y)
                pointT.Offset(offset);
            else
                pointT.Offset(GetDiagonalMove(PointH, PointT));

            return pointT;
        }

        private static Point GetOffsetFromDirection(char direction)
        {
            return char.ToUpper(direction) switch
            {
                'L' => new Point(-1, 0),
                'R' => new Point(1, 0),
                'U' => new Point(0, -1),
                'D' => new Point(0, 1),
                _ => new Point(0, 0)
            };
        }
        
        private static Point GetDiagonalMove(Point pointH, Point pointT)
        {
            return new Point(pointH.X < pointT.X ? -1 : 1, pointH.Y < pointT.Y ? -1 : 1);
        }
    }
}
