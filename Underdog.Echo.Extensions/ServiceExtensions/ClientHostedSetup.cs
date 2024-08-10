using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Echo.Extensions.HostedService.Client;

namespace Underdog.Echo.Extensions.ServiceExtensions
{
    public static class ClientHostedSetup
    {
        /// <summary>
        /// 注册客户端的IHostedService
        /// </summary>
        /// <param name="services"></param>
        public static void AddClientHostedSetup(this IServiceCollection services)
        {
            // services.AddHostedService<RabbitmqHostedService>();
            services.AddHostedService<MainHostedService>();
        }
    }
}
