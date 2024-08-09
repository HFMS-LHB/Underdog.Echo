using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Common.UI;

using Underdog.Main.Common.UI;

namespace Underdog.Main.Extensions.ServiceExtensions
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
