using Underdog.Core.Navigation.Regions;
using Underdog.ViewModels;
using Underdog.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Underdog.Common.GlobalVar;

namespace Underdog.Extensions
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
        }
    }
}
