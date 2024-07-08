using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

using Underdog.Model.ViewMessageModels;

namespace Underdog.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger<App> _logger;
        public App()
        {
            _logger = Program.AppHost!.Services.GetRequiredService<ILogger<App>>();

            WeakReferenceMessenger.Default.Register<ShutDownMessage>(this, async (r, m) =>
            {
                await Program.AppHost!.StopAsync();
                App.Current.Shutdown();
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await Program.AppHost!.StopAsync();

            base.OnExit(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // 未处理的异常
            // e.Handled = true; 表示异常已处理
            _logger.LogError("UnhandledException{ExceptionObject}", e.ExceptionObject.ToString());
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // ui异常
            _logger.LogError(e.Exception, "UI线程异常");
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            // 多线程异常
            _logger.LogError(e.Exception, "异步任务异常");
        }
    }
}
