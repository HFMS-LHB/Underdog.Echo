using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using Underdog.Core.Modularity;
using Underdog.Core.Navigation.Regions;
using Underdog.Echo.Common.GlobalVar;
using Underdog.Echo.ModuleB.Views;
using Underdog.Wpf.Extensions;

namespace Underdog.Echo.ModuleB
{
    public class ModuleBModule : IModule
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
