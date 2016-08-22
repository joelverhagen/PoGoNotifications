using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;
using Xunit;

namespace Knapcode.GroupMe.Test
{
    public class BotServiceTest
    {
        [Fact]
        public async Task BotService_SendsMessageWithoutAttachments()
        {
            // Arrange
            using (var httpClient = new HttpClient())
            {
                var target = new BotService(
                    Configuration.AccessToken,
                    httpClient);

                var messageService = new MessageService(
                    Configuration.AccessToken,
                    httpClient);

                var message = new BotMessage
                {
                    Text = "Text " + Guid.NewGuid()
                };

                // Act
                await target.PostAsync(Configuration.BotId, message, CancellationToken.None);

                // Assert
                var messages = await messageService.GetMessagesAsync(
                    Configuration.GroupId,
                    CancellationToken.None);

                var output = messages.FirstOrDefault(x => x.Text == message.Text);
                Assert.NotNull(output);
                Assert.Equal("bot", output.SenderType);
                Assert.Equal(Configuration.BotName, output.Name);
                Assert.Equal(Configuration.GroupId, output.GroupId);
                Assert.Empty(output.Images);
                Assert.Empty(output.Locations);
            }
        }

        [Fact]
        public async Task BotService_SendsMessageWithLocationAttachment()
        {
            // Arrange
            using (var httpClient = new HttpClient())
            {
                var target = new BotService(
                    Configuration.AccessToken,
                    httpClient);

                var messageService = new MessageService(
                    Configuration.AccessToken,
                    httpClient);

                var message = new BotMessage
                {
                    Text = "Text " + Guid.NewGuid(),
                    Location = new LocationAttachment
                    {
                        Name = "Location " + Guid.NewGuid(),
                        Latitude = 38.8976763,
                        Longitude = -77.0365298
                    }
                };

                // Act
                await target.PostAsync(Configuration.BotId, message, CancellationToken.None);

                // Assert
                var messages = await messageService.GetMessagesAsync(
                    Configuration.GroupId,
                    CancellationToken.None);

                var output = messages.FirstOrDefault(x => x.Text == message.Text);
                Assert.NotNull(output);
                Assert.Equal("bot", output.SenderType);
                Assert.Equal(Configuration.BotName, output.Name);
                Assert.Equal(Configuration.GroupId, output.GroupId);
                Assert.Empty(output.Images);
                Assert.Equal(1, output.Locations.Count);
                var location = output.Locations.First();
                Assert.Equal(message.Location.Name, location.Name);
                Assert.Equal(message.Location.Latitude, location.Latitude);
                Assert.Equal(message.Location.Longitude, location.Longitude);
            }
        }

        [Fact]
        public async Task BotService_SendsMessageWithImageAttachment()
        {
            // Arrange
            using (var httpClient = new HttpClient())
            {
                var target = new BotService(
                    Configuration.AccessToken,
                    httpClient);

                var messageService = new MessageService(
                    Configuration.AccessToken,
                    httpClient);

                var message = new BotMessage
                {
                    Text = "Text " + Guid.NewGuid(),
                    Image = new ImageAttachment
                    {
                        Url = "https://i.groupme.com/300x300.png.e8ec5793a332457096bc9707ffc9ac37"
                    }
                };

                // Act
                await target.PostAsync(Configuration.BotId, message, CancellationToken.None);

                // Assert
                var messages = await messageService.GetMessagesAsync(
                    Configuration.GroupId,
                    CancellationToken.None);

                var output = messages.FirstOrDefault(x => x.Text == message.Text);
                Assert.NotNull(output);
                Assert.Equal("bot", output.SenderType);
                Assert.Equal(Configuration.BotName, output.Name);
                Assert.Equal(Configuration.GroupId, output.GroupId);
                Assert.Empty(output.Locations);
                Assert.Equal(1, output.Images.Count);
                var image = output.Images.First();
                Assert.Equal(message.Image.Url, image.Url);
            }
        }
    }
}
