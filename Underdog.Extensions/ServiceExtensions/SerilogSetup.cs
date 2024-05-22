using Underdog.Common.Helper;
using Underdog.Common.LogHelper;
using Underdog.Serilog.Extensions;

using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Debugging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Underdog.Serilog.Configuration;

namespace Underdog.Extensions.ServiceExtensions
{
    public static class SerilogSetup
    {
        public static IHostBuilder AddSerilogSetup(this IHostBuilder host)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            _ = new AppSettings();
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(AppSettings.Configuration)
                .Enrich.FromLogContext()
                //输出到控制台
                .WriteToConsole()
                //将日志保存到文件中
                .WriteToFile()
                //配置日志库
                .WriteToLogBatching();

            Log.Logger = loggerConfiguration.CreateLogger();

            //Serilog 内部日志
            var file = File.CreateText(LogContextStatic.Combine($"SerilogDebug{DateTime.Now:yyyyMMdd}.txt"));
            SelfLog.Enable(TextWriter.Synchronized(file));

            host.UseSerilog();
            return host;
        }
    }
}
