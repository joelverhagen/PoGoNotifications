namespace Knapcode.PoGoNotifications.Models
{
    public class Notification
    {
        public string Text { get; set; }
        public ImageAttachment Image { get; set; }
        public LocationAttachment Location { get; set; }
    }
}
