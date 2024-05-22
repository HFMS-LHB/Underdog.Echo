﻿using Underdog.Common.Helper;
using Underdog.EventBus.Eventbus;
using Underdog.EventBus.EventBusSubscriptions;
using Underdog.Extensions.EventHandling;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Extensions.ServiceExtensions
{
    /// <summary>
    /// EventBus 事件总线服务
    /// </summary>
    public static class EventBusSetup
    {
        public static void AddEventBusSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.app(new string[] { "EventBus", "Enabled" }).ObjToBool())
            {
                var subscriptionClientName = AppSettings.app(new string[] { "EventBus", "SubscriptionClientName" });

                services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
                services.AddTransient<CardQueryIntegrationEventHandler>();

                //if (AppSettings.app(new string[] { "RabbitMQ", "Enabled" }).ObjToBool())
                //{
                //    services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                //    {
                //        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                //        var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                //        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                //        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                //        var retryCount = 5;
                //        if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "RetryCount" })))
                //        {
                //            retryCount = int.Parse(AppSettings.app(new string[] { "RabbitMQ", "RetryCount" }));
                //        }

                //        return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
                //    });
                //}
                //if (AppSettings.app(new string[] { "Kafka", "Enabled" }).ObjToBool())
                //{
                //    services.AddHostedService<KafkaConsumerHostService>();
                //    services.AddSingleton<IEventBus, EventBusKafka>();
                //}
            }
        }
    }
}
