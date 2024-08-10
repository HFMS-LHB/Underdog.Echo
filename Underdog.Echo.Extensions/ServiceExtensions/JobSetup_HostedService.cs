using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.ServiceExtensions
{
    /// <summary>
    /// 任务调度 启动服务
    /// </summary>
    public static class JobSetup_HostedService
    {
        public static void AddJobSetup_HostedService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 单独的任务
            // services.AddHostedService<Job1TimedService>();
            // services.AddHostedService<Job2TimedService>();

        }
    }
}
