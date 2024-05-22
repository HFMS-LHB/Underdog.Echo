using Underdog.Extensions.HostedService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Extensions.ServiceExtensions
{
    public static class InitializationHostServiceSetup
    {
        /// <summary>
        /// 应用初始化服务注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddInitializationHostServiceSetup(this IServiceCollection services)
        {
            if (services is null)
            {
                ArgumentNullException.ThrowIfNull(nameof(services));
            }

            // 开发环境才可以注入种子数据
            if (Environment.GetEnvironmentVariable("environment") == Environments.Development) 
            {
                services!.AddHostedService<SeedDataHostedService>();
            }
            services!.AddHostedService<EventBusHostedService>();
        }
    }
}
