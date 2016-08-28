using System;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Client;
using Knapcode.PoGoNotifications.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Knapcode.PoGoNotifications.Logic
{
    public class CheckRepublicService : ICheckRepublicService
    {
        private static readonly TimeSpan MaximumFrequency = TimeSpan.FromMinutes(5);

        private DateTimeOffset _lastHeartbeat;
        private SemaphoreSlim _heartbeatLock = new SemaphoreSlim(1);
        private readonly HeartGroupClient _client;
        private readonly string _heartGroupName;
        private readonly ILogger<CheckRepublicService> _logger;

        public CheckRepublicService(IOptions<NotificationOptions> notificationOptions, ILogger<CheckRepublicService> logger)
        {
            var options = notificationOptions.Value.CheckRepublicOptions;
            _heartGroupName = options.HeartGroupName;
            _client = new HeartGroupClient(options.Url, options.Password);
            _lastHeartbeat = DateTimeOffset.MinValue;
            _logger = logger;
        }

        public async Task SendHeartbeatAsync(CancellationToken token)
        {
            var now = DateTimeOffset.UtcNow;
            if (now - _lastHeartbeat < MaximumFrequency)
            {
                return;
            }

            bool acquired = false;

            try
            {
                acquired = await _heartbeatLock.WaitAsync(TimeSpan.Zero);

                if (!acquired)
                {
                    return;
                }

                await _client.CreateHeartbeatAsync(_heartGroupName, Environment.MachineName, token);

                _lastHeartbeat = now;
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Sending the heartbeat failed. Exception: {exception}", exception);
            }
            finally
            {
                if (acquired)
                {
                    _heartbeatLock.Release();
                }
            }
        }
    }
}
