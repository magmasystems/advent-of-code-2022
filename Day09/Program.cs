using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode2022
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Point PointH = new(0, 0);
            Point PointT = new(0, 0);
            HashSet<Point> PointsVisited = new();
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            
            // Part 1
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
                    PointT = MoveTail(PointH, PointT);
                    PointsVisited.Add(PointT);
                    // Console.WriteLine($"Head [{PointH.X}, {PointH.Y}] - Tail [{PointT.X}, {PointT.Y}]");
                }
                // Console.WriteLine("----------------------------------\n");
            }
            
            Console.WriteLine($"Part 1: The number of positions the tail visited is {PointsVisited.Count}");  // 5981
            
            // Part 2
            
            // Reset some of the variables
            PointsVisited.Clear();
            PointsVisited.Add(new Point(0, 0));
            PointH = new Point(0, 0);
            
            // We now have 9 segments of the snake's coil
            const int NUMCOILS = 9;
            var PointCoil = new Point[NUMCOILS];
            foreach (var i in Enumerable.Range(0, NUMCOILS))
                PointCoil[i] = new Point(0, 0);

            foreach (var line in input)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var numMoves = Convert.ToInt32(parts[1]);

                var offset = GetOffsetFromDirection(parts[0][0]);
                foreach (var _ in Enumerable.Range(0, numMoves))
                {
                    PointH.Offset(offset);
                    PointCoil[0] = MoveTail(PointH, PointCoil[0]);

                    for (var i = 1; i < NUMCOILS; i++)
                    {
                        PointCoil[i] = MoveTail(PointCoil[i-1], PointCoil[i]);
                    }

                    PointsVisited.Add(PointCoil[NUMCOILS-1]);
                }
            }
            
            Console.WriteLine($"Part 2: The number of positions the tail visited is {PointsVisited.Count}");  // 2352
        }

        private static Point MoveTail(Point pointH, Point pointT)
        {
            // See if the tail is still touching the head
            if (pointH.IsTouching(pointT))
                return pointT;
            
            // Otherwise, if the head and tail aren't touching and aren't in the same row or column, 
            // the tail always moves one step diagonally to keep up.
            if (pointT.X == pointH.X)
            {
                pointT.Y += pointH.Y > pointT.Y ? 1 : -1;
            }
            else if (pointT.Y == pointH.Y)
            {
                pointT.X += pointH.X > pointT.X ? 1 : -1;
            }
            else
            {
                pointT.Offset(GetDiagonalMove(pointH, pointT));
            }

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
