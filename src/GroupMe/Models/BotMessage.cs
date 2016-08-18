namespace Knapcode.GroupMe.Models
{
    public class BotMessage
    {
        public string Text { get; set; }
        public ImageAttachment Image { get; set; }
        public LocationAttachment Location { get; set; }
    }
}
