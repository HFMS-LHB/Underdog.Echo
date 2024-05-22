using Underdog.Wpf;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Underdog.HostedService
{
    public class MainHostService<TApplication, TWindow> : WPFHostedService<TApplication, TWindow>
        where TApplication : Application
        where TWindow : Window
    {
        public MainHostService(TApplication app, TWindow window, IHostApplicationLifetime applicationLifetime) : base(app, window, applicationLifetime)
        {
            applicationLifetime?.ApplicationStopped.Register(() =>
            {
                Common.App.IsRun = false;

                //清除日志
                Log.CloseAndFlush();
            });
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // app.Run(window);是以同步方式运行，直到程序关闭才会触发 applicationLifetime.ApplicationStarted
            // 只能重写 StartAsync 方法，并在之前标记 IsRun 为 true
            Common.App.IsRun = true;

            return base.StartAsync(cancellationToken);
        }
    }
}
