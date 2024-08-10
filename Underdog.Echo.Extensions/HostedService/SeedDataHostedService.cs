using Underdog.Echo.Common.Helper;
using Underdog.Echo.Common.Seed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Extensions.HostedService
{
    public sealed class SeedDataHostedService : IHostedService
    {
        private readonly MyContext _myContext;
        private readonly ILogger<SeedDataHostedService> _logger;
        private readonly string _contentRootPath;

        public SeedDataHostedService(
            MyContext myContext,
            IHostEnvironment hostEnvironment,
            ILogger<SeedDataHostedService> logger)
        {
            _myContext = myContext;
            _logger = logger;
            _contentRootPath = hostEnvironment.ContentRootPath;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Initialization Db Seed Service!");
            await DoWork();
        }

        private async Task DoWork()
        {
            try
            {
                if (AppSettings.app("AppSettings", "SeedDBEnabled").ObjToBool() || AppSettings.app("AppSettings", "SeedDBDataEnabled").ObjToBool())
                {
                    await DBSeed.SeedAsync(_myContext, _contentRootPath);

                    // //迁移日志数据
                    // DBSeed.MigrationLogs(_myContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured seeding the Database.");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop Initialization Db Seed Service!");
            return Task.CompletedTask;
        }
    }
}
