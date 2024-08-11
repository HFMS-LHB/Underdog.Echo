using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Model.Models.RootTkey
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class SysUserInfoRoot<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// Id
        /// 泛型主键Tkey
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public Tkey Id { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<Tkey> RIDs { get; set; }

    }
}
