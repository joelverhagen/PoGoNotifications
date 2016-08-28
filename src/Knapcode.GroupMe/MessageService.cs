using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;
using Newtonsoft.Json.Linq;

namespace Knapcode.GroupMe
{
    public class MessageService : IMessageService
    {
        private const string MessagesUrlFormat = "https://api.groupme.com/v3/groups/{0}/messages?access_token={1}";

        private readonly string _accessToken;
        private readonly HttpClient _httpClient;

        public MessageService(string accessToken, HttpClient httpClient)
        {
            _accessToken = accessToken;
            _httpClient = httpClient;
        }

        public async Task<IList<Message>> GetMessagesAsync(string groupId, CancellationToken token)
        {
            var url = string.Format(MessagesUrlFormat, groupId, _accessToken);
            var responseJson = await _httpClient.GetStringAsync(url);
            var responseJToken = JToken.Parse(responseJson);

            var messages = new List<Message>();

            foreach (var messageJToken in responseJToken["response"]["messages"])
            {
                var message = GetMessage(messageJToken);

                messages.Add(message);
            }

            return messages;
        }

        private Message GetMessage(JToken message)
        {
            var groupId = (string)message["group_id"];
            var senderType = (string)message["sender_type"];
            var senderId = (string)message["sender_id"];
            var name = (string)message["name"];
            var text = (string)message["text"];

            var images = new List<ImageAttachment>();
            var locations = new List<LocationAttachment>();

            foreach (var attachment in message["attachments"])
            {
                var type = (string)attachment["type"];

                if (type == "image")
                {
                    var image = new ImageAttachment
                    {
                        Url = (string) attachment["url"]
                    };

                    images.Add(image);
                }
                else if (type == "location")
                {
                    var location = new LocationAttachment
                    {
                        Name = (string)attachment["name"],
                        Latitude = double.Parse((string)attachment["lat"]),
                        Longitude = double.Parse((string)attachment["lng"])
                    };

                    locations.Add(location);
                }
            }

            return new Message
            {
                GroupId = groupId,
                SenderType = senderType,
                SenderId = senderId,
                Name = name,
                Text = text,
                Images = images,
                Locations = locations
            };
        }
    }
}
