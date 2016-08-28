using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Logic
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Notification notification);
    }
}
