using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Echo.IServices.BASE;
using Underdog.Echo.Model.Models;
using Underdog.Echo.Model;

namespace Underdog.Echo.IServices
{
    /// <summary>
    /// ITasksLogServices
    /// </summary>	
    public interface ITasksLogServices : IBaseServices<TasksLog>
    {
        public Task<PageModel<TasksLog>> GetTaskLogs(long jobId, int page, int intPageSize, DateTime? runTime, DateTime? endTime);
        public Task<object> GetTaskOverview(long jobId, DateTime? runTime, DateTime? endTime, string type);
    }
}
