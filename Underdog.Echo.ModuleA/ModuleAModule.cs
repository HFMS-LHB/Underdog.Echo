using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using Underdog.Core.Modularity;
using Underdog.Core.Navigation.Regions;
using Underdog.Echo.Common.GlobalVar;
using Underdog.Echo.ModuleA.Views;
using Underdog.Wpf.Extensions;

namespace Underdog.Echo.ModuleA
{
    public class ModuleAModule : IModule
    {
        public void Config(IServiceProvider services)
        {
            var regionManager = services.GetService<IRegionManager>();
            regionManager?.RegisterViewWithRegion(RegionKey.Root, typeof(ViewA));
        }

        public void ConfigService(IServiceCollection services)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            services.AddRegionViewScanner(currentAssembly);
            services.AddViewsAndViewModels(currentAssembly);
        }
    }
}
