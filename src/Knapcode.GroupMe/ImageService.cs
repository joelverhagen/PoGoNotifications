using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;
using Newtonsoft.Json.Linq;

namespace Knapcode.GroupMe
{
    public class ImageService : IImageService
    {
        private const string UploadUrlFormat = "https://image.groupme.com/upload?access_token={0}";

        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

        public ImageService(string accessToken, HttpClient httpClient)
        {
            _accessToken = accessToken;
            _httpClient = httpClient;
        }

        public async Task<Image> UploadImageUrlAsync(string imageUrl, CancellationToken token)
        {
            using (var imageStream = await _httpClient.GetStreamAsync(imageUrl))
            {
                return await UploadStreamAsync(imageStream, token);
            }
        }

        /// <summary>
        /// The image service in the developer documentation is only returing HTTP 500 for me. Therefore, use this
        /// implementation inspired by:
        /// https://github.com/njoubert/node-groupme/blob/cf34f4e63b4b2d5d471b57a8a3fe117245858287/lib/ImageService.js
        /// </summary>
        /// <param name="stream">The image stream.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The uploaded image.</returns>
        public async Task<Image> UploadStreamAsync(Stream stream, CancellationToken token)
        {
            // Note the it's not strictly necessary to add the access token, but this at least
            // allows the server side to identify the caller. I have no idea if they are even
            // looking for this query parameter.
            var requestUrl = string.Format(UploadUrlFormat, _accessToken);

            var imageContent = new StreamContent(stream);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Content = imageContent;

            JToken responseJToken;
            using (var response = await _httpClient.SendAsync(request, token))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                responseJToken = JToken.Parse(responseString);
            }

            var imageUrl = (string)responseJToken["payload"]["url"];

            return new Image
            {
                Url = imageUrl,
                AvatarUrl = imageUrl + ".avatar",
                PreviewUrl = imageUrl + ".preview",
                LargeUrl = imageUrl + ".large"
            };
        }
    }
}
