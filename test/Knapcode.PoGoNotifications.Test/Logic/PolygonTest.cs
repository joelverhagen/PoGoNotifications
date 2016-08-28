using Knapcode.PoGoNotifications.Logic;
using Knapcode.PoGoNotifications.Models;
using Xunit;

namespace Knapcode.PoGoNotifications.Test.Logic
{
    public class PolygonTest
    {
        [Theory]
        [MemberData(nameof(PointsAroundASquare))]
        public void ContainsPoint_PointsAroundASquare(Point point, bool expected)
        {
            // Arrange
            var polygon = new Polygon(new[]
            {
                new Point(0, 0),
                new Point(0, 10),
                new Point(10, 10),
                new Point(10, 0)
            });

            // Act
            var actual = polygon.ContainsPoint(point);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(PointsAroundAConcavePolygon))]
        public void ContainsPoint_ConcavePolygon(Point point, bool expected)
        {
            // Arrange
            var polygon = new Polygon(new[]
            {
                new Point(0, 0),
                new Point(10, 0),
                new Point(10, 10),
                new Point(7, 10),
                new Point(7, 3),
                new Point(3, 3),
                new Point(3, 10),
                new Point(0, 10)
            });

            // Act
            var actual = polygon.ContainsPoint(point);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static object[] PointsAroundASquare => new object[][]
        {
            new object[] { new Point(-5, 15), false },
            new object[] { new Point(5, 15), false },
            new object[] { new Point(15, 15), false },
            new object[] { new Point(-5, 5), false },
            new object[] { new Point(5, 5), true },
            new object[] { new Point(15, 5), false },
            new object[] { new Point(-5, -5), false },
            new object[] { new Point(5, -5), false },
            new object[] { new Point(15, -5), false }
        };

        public static object[] PointsAroundAConcavePolygon => new object[][]
        {
            new object[] { new Point(-5, 15), false },
            new object[] { new Point(5, 15), false },
            new object[] { new Point(15, 15), false },
            new object[] { new Point(-5, 5), false },
            new object[] { new Point(5, 5), false },
            new object[] { new Point(15, 5), false },
            new object[] { new Point(-5, -5), false },
            new object[] { new Point(5, -5), false },
            new object[] { new Point(2, 5), true },
            new object[] { new Point(8, 5), true }
        };
    }
}
