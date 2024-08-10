using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Echo.Common.Helper;
using Underdog.Echo.EventBus;
using Underdog.Echo.Extensions.RabbitMQ;

namespace Underdog.Echo.Extensions.ServiceExtensions
{
    /// <summary>
    /// Db 启动服务
    /// </summary>
    public static class RabbitMQSetup
    {
        public static void AddRabbitMQSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.app(new string[] { "RabbitMQ", "Enabled" }).ObjToBool())
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = AppSettings.app(new string[] { "RabbitMQ", "Connection" }),
                        DispatchConsumersAsync = true
                    };

                    if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "UserName" })))
                    {
                        factory.UserName = AppSettings.app(new string[] { "RabbitMQ", "UserName" });
                    }

                    if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "Password" })))
                    {
                        factory.Password = AppSettings.app(new string[] { "RabbitMQ", "Password" });
                    }

                    if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "Port" })))
                    {
                        factory.Port = AppSettings.app(new string[] { "RabbitMQ", "Port" }).ObjToInt();
                    }

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "RetryCount" })))
                    {
                        retryCount = AppSettings.app(new string[] { "RabbitMQ", "RetryCount" }).ObjToInt();
                    }

                    return new RabbitMQPersistentConnection(factory, logger, retryCount);
                });
            }
        }


        public static void AddRabbitMQListenerSetup(this IServiceCollection services)
        {
            // 注册消息监听服务
            services.AddSingleton<IRabbitMQListener, SelfInspectionListener>();
        }
    }
}
