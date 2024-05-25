﻿using Autofac.Extensions.DependencyInjection;
using Autofac;
using Underdog.Common.Helper;
using Underdog.Core.Extensions;
using Underdog.Wpf.Mvvm;
using Underdog.Wpf.Navigation.Regions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Underdog.Extensions.ServiceExtensions;
using Underdog.Extensions;
using Underdog.Common.Core;
using Underdog.Common.Helper.Console;
using Underdog.Main.Views;
using Underdog.Main.ViewModels;
using Underdog.Main.Extensions.ServiceExtensions;
using Underdog.Main.Common.HostDispatcher;
using Underdog.Main.Extensions.HostedService;
using Underdog.Wpf.Dialogs;


namespace Underdog.Main
{
    public class Program
    {
        public static IHost? AppHost { get; private set; }
        public static IServiceProvider ServiceProvider => AppHost!.Services;

        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            #region set environment
#if DEBUG
            Environment.SetEnvironmentVariable("environment", "Development");
#else
            Environment.SetEnvironmentVariable("environment", "Production");
#endif
            #endregion

            // create builder
            var builder = Host.CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
                .UseEnvironment(Environment.GetEnvironmentVariable("environment") ?? "Developement")
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigurationCommonService)
                .ConfigureServices(ModularityExtension.AddModularity)
                .ConfigureServices(ConfigurationClientService)
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacModuleRegister());
                });

            builder.AddSerilogSetup();

            AppHost = builder.Build();
            // 加载App.xaml资源
            var app = AppHost.Services.GetRequiredService<App>();
            app.InitializeComponent();
            AppHost.ConfigureApplication();
            AppHost.UseRegion<MainWindow>();
            AppHost.UseMainRegion();
            AppHost.UseModularity();
            AppHost.Run();
        }

        /// <summary>
        /// 配置文件
        /// </summary>
        /// <param name="hostingContext"></param>
        /// <param name="config"></param>
        private static void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            config.Sources.Clear();
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                config.AddJsonFile($"appsettings.{Environments.Development}.json", optional: true, reloadOnChange: false);
            }

            config.AddEnvironmentVariables();

            hostingContext.Configuration.ConfigureApplication(config.Build());
        }

        /// <summary>
        /// 配置通用服务 
        /// Redis、EventBus、AutoMapper、Cache、SqlSugar等
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        private static void ConfigurationCommonService(HostBuilderContext context, IServiceCollection services)
        {
            // common
            context.ConfigureApplication(services);
            services.AddSingleton(new AppSettings(context.Configuration));
            services.AddAllOptionRegister();
            services.AddCacheSetup();
            services.AddSqlsugarSetup();
            services.AddDbSetup();
            services.AddInitializationHostServiceSetup();
            services.AddAutoMapperSetup();
            services.AddClientAutoMapperSetup(); // 客户端AutoMapper配置
            if (ConsoleHelper.IsConsoleApp())
            {
                services.AddAppTableConfigSetup(context.HostingEnvironment);
            }
            services.AddRedisInitMqSetup();
            services.AddEventBusSetup();
        }

        /// <summary>
        /// 配置客户端服务
        /// view、viewmodel、region等
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        private static void ConfigurationClientService(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<App>();
            services.AddSingleton<IDispatcher, WpfDispatcher>();
            services.AddScoped<MainWindow>();
            services.AddScoped<MainWindowViewModel>();
            services.AddHostedService<MainHostService<App, MainWindow>>();
            services.AddViewAndViewModel();
            services.AddRegion();
            services.AddDialog();
            services.AddMvvm();
        }
    }
}
