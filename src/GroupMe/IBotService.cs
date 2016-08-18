using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;

namespace Knapcode.GroupMe
{
    public interface IBotService
    {
        Task PostAsync(string botId, BotMessage message, CancellationToken token);
    }
}