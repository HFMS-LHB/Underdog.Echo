using Underdog.Echo.Common.Core;
using Underdog.Echo.Common.Helper;
using Underdog.Echo.Common.LogHelper;
using Underdog.Echo.IServices;

using Microsoft.Extensions.Hosting;

using Quartz;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Underdog.Echo.Common.UI;

namespace Underdog.Echo.Tasks
{
    public class Job_Simple_Quartz : JobBase, IJob
    {
        private readonly IHostEnvironment _environment;
        private readonly IUIOperationService _uiOperationService;

        public Job_Simple_Quartz(IHostEnvironment environment,
                                         ITasksQzServices tasksQzServices,
                                         ITasksLogServices tasksLogServices,
                                         IUIOperationService uiOperationService)
            : base(tasksQzServices, tasksLogServices)
        {
            _environment = environment;
            _uiOperationService = uiOperationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }

        public async Task Run(IJobExecutionContext context)
        {
            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;


            _uiOperationService.LockUI();

            _uiOperationService.UnlockUI();
        }
    }
}
