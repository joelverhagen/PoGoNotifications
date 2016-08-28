using System.Threading;
using System.Threading.Tasks;

namespace Knapcode.PoGoNotifications.Logic
{
    public interface ICheckRepublicService
    {
        Task SendHeartbeatAsync(CancellationToken token);
    }
}