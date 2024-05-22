using Underdog.Model.Models.RootTkey;

using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Model.Models
{
    public class CardBox : RootEntityTkey<long>
    {
        /// <summary>
        /// 卡盒名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string CardBoxName { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [SugarColumn(Length = 50)]
        public string CardBoxCode { get; set; }

        /// <summary>
        /// 卡盒类型 0-护照本盒 1-通行证卡盒
        /// </summary>
        [SugarColumn(DefaultValue = "0")]
        public int CardBoxType { get; set; }

        [SugarColumn(IsNullable = true)]
        public string CardBoxDescription { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(DefaultValue = "true")]
        public bool IsEnabled { get; set; }
    }
}
