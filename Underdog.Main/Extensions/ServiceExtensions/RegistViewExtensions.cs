﻿using Underdog.Common.GlobalVar;
using Underdog.Main.ViewModels;
using Underdog.Main.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Windows.Threading;

using Underdog.Core.Navigation.Regions;
using Underdog.Wpf.Ioc;
using System.Windows;
using Underdog.Wpf.Navigation.Regions;

namespace Underdog.Main.Extensions.ServiceExtensions
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
            regionManager?.RegisterViewWithRegion(RegionKey.Root, typeof(AdminHome));
            regionManager?.RegisterViewWithRegion(RegionKey.Root, typeof(AdminLogin));
            regionManager?.RegisterViewWithRegion(RegionKey.Root, typeof(LockLogin));
        }

        /// <summary>
        /// 注册视图和视图模型
        /// </summary>
        /// <param name="services"></param>
        public static void AddViewAndViewModel(this IServiceCollection services)
        {
            services.AddTransient<Home>();
            services.AddTransient<AdminHome>();
            services.AddTransient<AdminLogin>();
            services.AddTransient<LockLogin>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<AdminHomeViewModel>();
            services.AddTransient<AdminLoginViewModel>();
            services.AddTransient<LockLoginViewModel>();

            // add dialog
            // services.RegisterDialog<V, VM>();
        }

        public static void AddDialogVMMapping(this IServiceCollection services)
        {
            // add dialog
            // services.RegisterDialog<V, VM>();
            // services.RegisterDialogWindow<MessageBoxC>(nameof(MessageBoxC));
            // services.RegisterDialog<NotificationDialog1, NotificationDialog1ViewModel>();
            // services.RegisterDialog<NotificationDialog2, NotificationDialog2ViewModel>();
        }
    }
}
