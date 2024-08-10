﻿using Underdog.Echo.Model.Models.RootTkey.Interface;
using Underdog.Echo.Model.Models.RootTkey;
using SqlSugar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Common.DB
{
    public class RepositorySetting
    {
        private static readonly Lazy<IEnumerable<Type>> AllEntitys = new(() =>
        {
            return typeof(BaseEntity).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseEntity)))
                .Where(it => it.FullName != null && it.FullName.StartsWith("Underdog.Echo.Model.Models"));
        });

        public static IEnumerable<Type> Entitys => AllEntitys.Value;

        /// <summary>
        /// 配置实体软删除过滤器<br/>
        /// 统一过滤 软删除 无需自己写条件
        /// </summary>
        public static void SetDeletedEntityFilter(SqlSugarScopeProvider db)
        {
            db.QueryFilter.AddTableFilter<IDeleteFilter>(it => it.IsDeleted == false);
        }
    }
}
