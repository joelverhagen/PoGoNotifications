namespace Knapcode.PoGoNotifications.Models
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
        }

        public double X { get; set; }
        public double Y { get; set; }        
    }
}
