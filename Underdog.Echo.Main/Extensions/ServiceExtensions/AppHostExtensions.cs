using Underdog.Echo.Common.GlobalVar;
using Underdog.Echo.Main.ViewModels;
using Underdog.Echo.Main.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Windows.Threading;

using Underdog.Core.Navigation.Regions;
using Underdog.Wpf.Ioc;
using System.Windows;
using Underdog.Wpf.Navigation.Regions;
using Underdog.Core.Extensions;

namespace Underdog.Echo.Main.Extensions.ServiceExtensions
{
    public static class AppHostExtensions
    {
        /// <summary>
        /// 初始化App.xaml资源
        /// </summary>
        /// <param name="host"></param>
        public static void InitializeComponent(this IHost host)
        {
            var app = host.Services.GetRequiredService<App>();
            app.InitializeComponent();
        }

        public static void RunApplication<HostWindow>(this IHost host) where HostWindow:Window,new()
        {
            Task.Run(async () =>
            {
                try
                {
                    await host.RunAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during IOC container registration: {ex.Message}");
                    Environment.Exit(1);
                }
            });

            var mainWindow = host.Services.GetRequiredService<HostWindow>();
            mainWindow!.ShowDialog();
        }

        /// <summary>
        /// 注册区域
        /// </summary>
        /// <param name="host"></param>
        public static void UseMainRegion(this IHost host)
        {
            var regionManager = host.Services.GetService<IRegionManager>();
            regionManager?.RegisterViewWithRegion(RegionKey.Root, typeof(Home));
        }

        /// <summary>
        /// 手动注册视图和视图模型
        /// </summary>
        /// <param name="services"></param>
        public static void AddViewAndViewModel(this IServiceCollection services)
        {
            services.AddTransient<Home>();

            services.AddTransient<HomeViewModel>();
        }

        /// <summary>
        /// 注册弹窗
        /// </summary>
        /// <param name="services"></param>
        public static void AddDialogVMMapping(this IServiceCollection services)
        {
            // // 自定义弹窗父窗体
            // services.RegisterDialogWindow<MessageBoxC>(nameof(MessageBoxC));
            // // 注册弹窗
            // services.RegisterDialog<NotificationDialog1, NotificationDialog1ViewModel>();
            // services.RegisterDialog<NotificationDialog2, NotificationDialog2ViewModel>();
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="services"></param>
        public static void AddModules(this IServiceCollection services)
        {
            services.AddModule<ModuleA.ModuleAModule>();
            services.AddModule<ModuleB.ModuleBModule>();
        }
    }
}
