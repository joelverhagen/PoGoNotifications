namespace Knapcode.PoGoNotifications.Models
{
    public class NotificationAreaOptions
    {
        public GeoPoint[] Polygon { get; set; }
        public IgnoredPokemon[] Pokemon { get; set; }
    }
}
