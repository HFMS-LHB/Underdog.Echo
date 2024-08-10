using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Model.Models.RootTkey.Interface
{
    /// <summary>
    /// 软删除 过滤器
    /// </summary>
    public interface IDeleteFilter
    {
        public bool IsDeleted { get; set; }
    }
}
