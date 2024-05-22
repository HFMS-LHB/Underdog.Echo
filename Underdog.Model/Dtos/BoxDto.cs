using Underdog.Model.Models.RootTkey;

using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Model.Dtos
{
    public class BoxDto
    { 
        /// <summary>
        /// 卡盒名称
        /// </summary>
        public string CardBoxName { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string CardBoxCode { get; set; }

        /// <summary>
        /// 卡盒类型 0-护照本盒 1-通行证卡盒
        /// </summary>
        public int CardBoxType { get; set; }

        public string CardBoxDescription { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
