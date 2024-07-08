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
    public class MainHostedService : IHostedService
    {
        public MainHostedService(IHostApplicationLifetime applicationLifetime)
        {
            applicationLifetime?.ApplicationStarted.Register(AppIsRun);
            applicationLifetime?.ApplicationStopped.Register(AppIsStop);
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
    }
}
