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

        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            var helper = new HostBuilderHelper(args, false);
            var builder = helper.CreateHostBuilder();
            AppHost = builder.Build();

            AppHost.InitializeComponent();
            AppHost.ConfigureApplication();
            AppHost.UseRegion<MainWindow>();
            AppHost.UseMainRegion();
            AppHost.UseModularity();
            AppHost.RunApplication<MainWindow>();
        }
    }
}
