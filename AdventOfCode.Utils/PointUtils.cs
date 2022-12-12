using System.Drawing;

namespace AdventOfCode.Utils;

public static class PointUtils
{
    public static bool IsTouching(this Point p1, Point p2)
    {
        if (p1.X == p2.X && Math.Abs(p1.Y - p2.Y) <= 1)               // Same column
            return true;
        if (p1.Y == p2.Y && Math.Abs(p1.X - p2.X) <= 1)               // Same row
            return true;
        if (Math.Abs(p1.Y - p2.Y) == 1 && Math.Abs(p1.X - p2.X) == 1) // on a diagonal
            return true;
        return false;
    }
}