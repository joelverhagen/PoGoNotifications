namespace Knapcode.PoGoNotifications.Models
{
    public class NotificationOptions
    {
        public string GoogleMapsApiKey { get; set; }
        public string PokemonIconUrlFormat { get; set; }
        public GroupMeOptions GroupMeOptions { get; set; }
        public CheckRepublicOptions CheckRepublicOptions { get; set; }
        public NotificationAreaOptions[] NotificationAreas { get; set; }
        public bool UseNotificationImage { get; set; }
        public bool UseNotificationLocation { get; set; }
    }
}
