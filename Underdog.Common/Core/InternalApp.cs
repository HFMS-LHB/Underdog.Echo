using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Common.Core
{
    public static class InternalApp
    {
        internal static IServiceCollection InternalServices;

        /// <summary>根服务</summary>
        internal static IServiceProvider RootServices;

        /// <summary>获取泛型主机环境</summary>
        internal static IHostEnvironment HostEnvironment;

        /// <summary>配置对象</summary>
        internal static IConfiguration Configuration;

        public static void ConfigureApplication(this HostBuilderContext context, IServiceCollection services)
        {
            HostEnvironment = context.HostingEnvironment;
            InternalServices = services;
        }

        public static void ConfigureApplication(this IConfiguration configuration, IConfiguration configuration2)
        {
            Configuration = configuration2 ?? configuration;
        }

        public static void ConfigureApplication(this IHost app)
        {
            RootServices = app.Services;
        }
    }
}
