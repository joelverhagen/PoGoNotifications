using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Knapcode.GroupMe.Test
{
    public class ImageServiceTest
    {
        [Fact]
        public async Task ImageService_UploadsFromUrl()
        {
            // Arrange
            using (var httpClient = new HttpClient())
            {
                var target = new ImageService(Configuration.AccessToken, httpClient);

                // Act
                var image = await target.UploadImageUrlAsync(
                    "http://placehold.it/350x150",
                    CancellationToken.None);

                // Assert
                Assert.StartsWith("https://i.groupme.com/", image.Url);
                Assert.Equal(image.Url + ".avatar", image.AvatarUrl);
                Assert.Equal(image.Url + ".preview", image.PreviewUrl);
                Assert.Equal(image.Url + ".large", image.LargeUrl);
            }
        }

        [Fact]
        public async Task ImageService_UploadsFromStream()
        {
            // Arrange
            using (var httpClient = new HttpClient())
            using (var stream = await httpClient.GetStreamAsync("http://placehold.it/350x150"))
            {
                var target = new ImageService(Configuration.AccessToken, httpClient);

                // Act
                var image = await target.UploadStreamAsync(
                    stream,
                    CancellationToken.None);

                // Assert
                Assert.StartsWith("https://i.groupme.com/", image.Url);
                Assert.Equal(image.Url + ".avatar", image.AvatarUrl);
                Assert.Equal(image.Url + ".preview", image.PreviewUrl);
                Assert.Equal(image.Url + ".large", image.LargeUrl);
            }
        }
    }
}
