using System.Collections.Generic;

namespace Knapcode.GroupMe.Models
{
    public class Message
    {
        public string GroupId { get; set; }
        public string SenderType { get; set; }
        public string SenderId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public IList<ImageAttachment> Images { get; set; }
        public IList<LocationAttachment> Locations { get; set; }
    }
}
