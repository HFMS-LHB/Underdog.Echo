using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Underdog.EventBus;
using Underdog.Extensions.RabbitMQ;
using Underdog.Wpf;

namespace Underdog.Main.Extensions.HostedService
{
    public class MainHostService : IHostedService
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IEnumerable<IRabbitMQListener> _messageListeners;
        public MainHostService(IHostApplicationLifetime applicationLifetime,
                               IRabbitMQPersistentConnection persistentConnection,
                               IEnumerable<IRabbitMQListener> messageListeners)
        {
            _persistentConnection = persistentConnection;
            _messageListeners = messageListeners;

            applicationLifetime?.ApplicationStarted.Register(AppIsRun);
            applicationLifetime?.ApplicationStarted.Register(OnListenerStarted);
            applicationLifetime?.ApplicationStopped.Register(AppIsStop);
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

        private void AppIsRun()
        {
            Underdog.Common.App.IsRun = true;
        }

        private void AppIsStop()
        {
            Underdog.Common.App.IsRun = false;
            //清除日志
            Log.CloseAndFlush();
        }

        private void OnListenerStarted()
        {
            foreach (var listener in _messageListeners)
            {
                listener.StartListening();
            }
        }

        private void OnListenerStopped()
        {
            foreach (var listener in _messageListeners)
            {
                listener.StopListening();
            }
        }
    }
}
