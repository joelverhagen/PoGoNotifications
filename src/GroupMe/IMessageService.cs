using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;

namespace Knapcode.GroupMe
{
    public interface IMessageService
    {
        Task<IList<Message>> GetMessagesAsync(string groupId, CancellationToken token);
    }
}