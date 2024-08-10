﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class EnumAttachedAttribute : Attribute
    {
        /// <summary>
        /// 标签类型 样式
        /// </summary>
        public string TagType { get; set; }
        /// <summary>
        /// 中文描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string IconColor { get; set; }
    }
}
