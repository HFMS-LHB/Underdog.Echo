using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Model.Models.RootTkey.Interface
{
    /// <summary>
    /// 时间过滤器
    /// </summary>
    public interface ITimeFilter
    {
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
