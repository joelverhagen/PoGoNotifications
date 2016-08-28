using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Test
{
    public class TestOptions
    {
        public GeoPoint PointInsideSmallestArea { get; set; }
        public GeoPoint PointOutsideSmallestArea { get; set; }
        public GeoPoint FarAwayPoint { get; set; }
    }
}
