using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.EventBus;

using Underdog.Extensions.RabbitMQ;

namespace Underdog.Main.Extensions.HostedService
{
    public class RabbitmqHostedService : IHostedService
    {
        private readonly ILogger<RabbitmqHostedService> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IEnumerable<IRabbitMQListener> _messageListeners;
        public RabbitmqHostedService(ILogger<RabbitmqHostedService> logger,
                                   IHostApplicationLifetime applicationLifetime,
                                   IRabbitMQPersistentConnection persistentConnection,
                                   IEnumerable<IRabbitMQListener> messageListeners)
        {
            _logger = logger;
            _persistentConnection = persistentConnection;
            _messageListeners = messageListeners;

            applicationLifetime?.ApplicationStarted.Register(OnListenerStarted);
            applicationLifetime?.ApplicationStopped.Register(OnListenerStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnListenerStarted()
        {
            if (!_persistentConnection.IsConnected)
            {
                if (!_persistentConnection.TryConnect())
                {
                    _logger.LogWarning("RabbitMQ 无法连接");
                    return;
                }
            }

            _persistentConnection.CreateModel();

            foreach (var listener in _messageListeners)
            {
                listener.StartListening();
            }
        }

        private void OnListenerStopped()
        {
            if (!_persistentConnection.IsConnected)
            {
                return;
            }

            foreach (var listener in _messageListeners)
            {
                listener.StopListening();
            }

            _persistentConnection.Dispose();
        }
    }
}
