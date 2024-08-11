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

namespace Underdog.Echo.Main.Extensions.ServiceExtensions
{
    public static class RegistViewExtensions
    {
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

            // add dialog
            // services.RegisterDialog<V, VM>();
        }

        /// <summary>
        /// 注册弹窗
        /// </summary>
        /// <param name="services"></param>
        public static void AddDialogVMMapping(this IServiceCollection services)
        {
            // add dialog
            // services.RegisterDialog<V, VM>();
            // // 自定义弹窗父窗体
            // services.RegisterDialogWindow<MessageBoxC>(nameof(MessageBoxC));
            // services.RegisterDialog<NotificationDialog1, NotificationDialog1ViewModel>();
            // services.RegisterDialog<NotificationDialog2, NotificationDialog2ViewModel>();
        }
    }
}
