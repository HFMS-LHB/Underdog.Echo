using Microsoft.Extensions.Hosting;
using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.HostedService.Client
{
    /// <summary>
    /// 使用时要保证rabbitmq处于可连接状态
    /// </summary>
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
            Common.App.IsRun = true;
        }

        private void AppIsStop()
        {
            Common.App.IsRun = false;
            //清除日志
            Log.CloseAndFlush();
        }
    }
}
