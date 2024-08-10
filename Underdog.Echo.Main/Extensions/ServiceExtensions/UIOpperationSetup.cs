using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Echo.Common.UI;

using Underdog.Echo.Main.Common.UI;

namespace Underdog.Echo.Main.Extensions.ServiceExtensions
{
    public static class UIOpperationSetup
    {
        public static void AddUIOpperationSetup(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<IUIOperationService, UIOperationService>();
        }
    }
}
