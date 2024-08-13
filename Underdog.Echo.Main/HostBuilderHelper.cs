using Autofac.Extensions.DependencyInjection;
using Autofac;
using Underdog.Echo.Common.Helper;
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

using Underdog.Echo.Extensions.ServiceExtensions;
using Underdog.Echo.Extensions;
using Underdog.Echo.Common.Core;
using Underdog.Echo.Common.Helper.Console;
using Underdog.Echo.Main.Views;
using Underdog.Echo.Main.ViewModels;
using Underdog.Echo.Main.Extensions.ServiceExtensions;
using Underdog.Wpf.Dialogs;
using Underdog.Wpf.Ioc;
using Underdog.Wpf.Extensions;
using System.Reflection;
using Underdog.Echo.Common.UI;
using Underdog.Echo.Main.Common.UI;

namespace Underdog.Echo.Main
{
    public class HostBuilderHelper
    {
        private readonly string[] _args;
        private readonly bool _isProduction;

        public HostBuilderHelper(string[] args, bool isProduction = true)
        {
            _args = args;
            _isProduction = isProduction;
            SetEnvironment();
        }

        /// <summary>
        /// create host builder
        /// </summary>
        /// <param name="args"></param>
        /// <param name="isProduction"></param>
        /// <returns></returns>
        public IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder(_args)
                .UseContentRoot(AppContext.BaseDirectory)
                .UseEnvironment(Environment.GetEnvironmentVariable("environment") ?? Environments.Development)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigurationCommonService)
                .ConfigureServices(ConfigurationClientService)
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacModuleRegister());
                });

            builder.AddSerilogSetup();

            return builder;
        }

        /// <summary>
        /// 初始化环境
        /// </summary>
        private void SetEnvironment()
        {
            if (_isProduction)
            {
                Environment.SetEnvironmentVariable("environment", Environments.Production);
            }
            else
            {
                Environment.SetEnvironmentVariable("environment", Environments.Development);
            }
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
        /// Redis、Rabbitmq、EventBus、AutoMapper、Cache、SqlSugar等
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        private static void ConfigurationCommonService(HostBuilderContext context, IServiceCollection services)
        {
            // common
            context.ConfigureApplication(services);
            services.AddSingleton(new AppSettings(context.Configuration));
            services.AddHttpClientSetup();
            services.AddAllOptionRegister();
            services.AddCacheSetup();
            services.AddSqlsugarSetup();
            services.AddDbSetup();
            services.AddInitializationHostServiceSetup();
            services.AddAutoMapperSetup();
            services.AddClientAutoMapperSetup(); // 客户端AutoMapper配置
            services.AddJobSetup();
            if (ConsoleHelper.IsConsoleApp())
            {
                services.AddAppTableConfigSetup(context.HostingEnvironment);
            }
            services.AddRedisInitMqSetup();
            services.AddRabbitMQSetup();
            services.AddEventBusSetup();
            // 注册rabbitmq监听者
            services.AddRabbitMQListenerSetup();
            services.AddUIOpperationSetup();
        }

        /// <summary>
        /// 配置客户端服务
        /// view、viewmodel、region等
        /// </summary>
        /// <param name="context"></param>
        /// <param name="services"></param>
        private static void ConfigurationClientService(HostBuilderContext context, IServiceCollection services)
        {
            // 当前程序集
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            services.AddSingleton<App>();
            services.AddSingleton<IDispatcher, WpfDispatcher>();
            services.AddScoped<MainWindow>();
            services.AddScoped<MainWindowViewModel>();
            services.AddRegion();
            services.AddDialog();
            services.AddMvvm();
            services.AddModules();
            services.AddRegionViewScanner(currentAssembly);
            services.AddViewsAndViewModels(currentAssembly); // 通过程序集自动注册
            services.AddDialogVMMapping();
            services.AddClientHostedSetup();
        }
    }
}
