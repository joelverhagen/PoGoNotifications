using System;
using System.Collections.Generic;
using System.Linq;
using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Logic
{
    /// <summary>
    /// Source: http://csharphelper.com/blog/2014/07/determine-whether-a-point-is-inside-a-polygon-in-c/
    /// </summary>
    public class Polygon
    {
        private readonly Point[] _points;

        public Polygon(IEnumerable<Point> points)
        {
            _points = points.ToArray();
        }

        public bool ContainsPoint(Point point)
        {
            // Get the angle between the point and the first and last vertices.
            var totalAngle = GetAngle(_points[_points.Length - 1], point,_points[0]);

            // Add the angles from the point to each other pair of vertices.
            for (int i = 0; i < _points.Length - 1; i++)
            {
                totalAngle += GetAngle(
                    _points[i],
                    point,
                    _points[i + 1]);
            }

            // The total angle should be 2 * PI or -2 * PI if the point is in the polygon and close
            // to zero if the point is outside the polygon.
            return Math.Abs(totalAngle) > 0.000001;
        }

        private static double GetAngle(Point a, Point b, Point c)
        {
            // Get the dot product.
            var dot_product = DotProduct(a, b, c);

            // Get the cross product.
            var cross_product = CrossProductLength(a, b, c);

            // Calculate the angle.
            return Math.Atan2(cross_product, dot_product);
        }

        private static double DotProduct(Point a, Point b, Point c)
        {
            // Get the vectors' coordinates.
            var BAx = a.X - b.X;
            var BAy = a.Y - b.Y;
            var BCx = c.X - b.X;
            var BCy = c.Y - b.Y;

            // Calculate the dot product.
            return BAx * BCx + BAy * BCy;
        }

        private static double CrossProductLength(Point a, Point b, Point c)
        {
            // Get the vectors' coordinates.
            var BAx = a.X - b.X;
            var BAy = a.Y - b.Y;
            var BCx = c.X - b.X;
            var BCy = c.Y - b.Y;

            // Calculate the Z coordinate of the cross product.
            return BAx * BCy - BAy * BCx;
        }
    }
}
