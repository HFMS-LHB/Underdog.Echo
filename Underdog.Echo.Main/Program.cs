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
    public class Program
    {
        public static IHost? AppHost { get; private set; }
        public static IServiceProvider ServiceProvider => AppHost!.Services;

        [System.STAThreadAttribute()]
        public static void Main(string[] args)
       {
            // 根据编译模式手动设置环境变量 Development OR Production
            #region set environment
            Environment.SetEnvironmentVariable("environment", Environments.Development);
            // Environment.SetEnvironmentVariable("environment", Environments.Production);
            #endregion

            // create builder
            var builder = Host.CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
                .UseEnvironment(Environment.GetEnvironmentVariable("environment") ?? Environments.Development)
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

            // 加载App.xaml资源 一定要放在操作视图之前
            var app = AppHost.Services.GetRequiredService<App>();
            app.InitializeComponent();

            AppHost.ConfigureApplication();
            AppHost.UseRegion<MainWindow>();
            AppHost.UseMainRegion();
            AppHost.UseModularity();

            // 异步启动Host
            Task.Run(async () =>
            {
                try
                {
                    await AppHost.RunAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during IOC container registration: {ex.Message}");
                    Environment.Exit(1);
                }
            });

            // 启动主程序
            var mainWindow = AppHost.Services.GetService<MainWindow>();
            mainWindow!.ShowDialog();
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
            services.AddRegionViewScanner(currentAssembly);
            services.AddViewsAndViewModels(currentAssembly); // 通过程序集自动注册
            // services.AddViewAndViewModel(); // 手动注册
            services.AddDialogVMMapping();
            services.AddClientHostedSetup();
        }
    }
}
