using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe;
using Knapcode.GroupMe.Models;
using Knapcode.PoGoNotifications.Models;
using Microsoft.Extensions.Options;
using GroupMeImageAttachment = Knapcode.GroupMe.Models.ImageAttachment;
using GroupMeLocationAttachment = Knapcode.GroupMe.Models.LocationAttachment;

namespace Knapcode.PoGoNotifications.Logic
{
    public class GroupMeNotificationService : INotificationService
    {
        private readonly IBotService _botService;
        private readonly IImageService _imageService;
        private readonly IOptions<NotificationOptions> _options;

        public GroupMeNotificationService(IOptions<NotificationOptions> options, IImageService imageService, IBotService botService)
        {
            _options = options;
            _imageService = imageService;
            _botService = botService;
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            var botMessage = new BotMessage
            {
                Text = notification.Text
            };

            if (notification.Image != null)
            {
                var image = await _imageService.UploadImageUrlAsync(notification.Image.Url, CancellationToken.None);
                botMessage.Image = new GroupMeImageAttachment
                {
                    Url = image.Url
                };
            }

            if (notification.Location != null)
            {
                botMessage.Location = new GroupMeLocationAttachment
                {
                    Latitude = notification.Location.Latitude,
                    Longitude = notification.Location.Longitude,
                    Name = notification.Location.Name
                };
            }

            await _botService.PostAsync(_options.Value.GroupMeOptions.BotId, botMessage, CancellationToken.None);
        }
    }
}
