using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Knapcode.GroupMe
{
    public class BotService : IBotService
    {
        private const string PostUrlFormat = "https://api.groupme.com/v3/bots/post?access_token={0}";

        private readonly string _accessToken;
        private readonly HttpClient _httpClient;

        public BotService(string accessToken, HttpClient httpClient)
        {
            _accessToken = accessToken;
            _httpClient = httpClient;
        }

        public async Task PostAsync(string botId, BotMessage message, CancellationToken token)
        {
            var messageJObject = BuildMessageJObject(botId, message);
            var messageJson = messageJObject.ToString(Formatting.None);
            var requestContent = new StringContent(messageJson, Encoding.UTF8, "application/json");

            var requestUrl = string.Format(PostUrlFormat, _accessToken);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Content = requestContent;

            using (var response = await _httpClient.SendAsync(request, token))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        private JObject BuildMessageJObject(string botId, BotMessage message)
        {
            var messageJObject = new JObject();

            messageJObject["bot_id"] = botId;

            if (message.Text != null)
            {
                messageJObject["text"] = message.Text;
            }

            var attachments = new JArray();

            // Optionally, attach the image.
            if (message.Image != null)
            {
                var imageAttachment = new JObject();
                imageAttachment["type"] = "image";
                imageAttachment["url"] = message.Image.Url;

                attachments.Add(imageAttachment);
            }

            // Optionally, attach the location.
            if (message.Location != null)
            {
                var locationAttachment = new JObject();
                locationAttachment["type"] = "location";
                locationAttachment["lat"] = message.Location.Latitude;
                locationAttachment["lng"] = message.Location.Longitude;
                locationAttachment["name"] = message.Location.Name;

                attachments.Add(locationAttachment);
            }

            // Add the attachments, if any.
            if (attachments.Any())
            {
                messageJObject["attachments"] = attachments;
            }

            return messageJObject;
        }
    }
}
