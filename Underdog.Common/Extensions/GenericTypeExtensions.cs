﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Common.Helper;

namespace Underdog.Common.Extensions
{
    public static class GenericTypeExtensions
        {
            public static string GetGenericTypeName(this Type type)
            {
                var typeName = string.Empty;

                if (type.IsGenericType)
                {
                    var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                    typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
                }
                else
                {
                    typeName = type.Name;
                }

                return typeName;
            }

            public static string GetGenericTypeName(this object @object)
            {
                return @object.GetType().GetGenericTypeName();
            }

            /// <summary>
            /// 判断类型是否实现某个泛型
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="generic">泛型类型</param>
            /// <returns>bool</returns>
            // public static bool HasImplementedRawGeneric(this Type type, Type generic)
            // {
            //     // 检查接口类型
            //     var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            //     if (isTheRawGenericType) return true;

            //     // 检查类型
            //     while (type != null && type != typeof(object))
            //     {
            //         isTheRawGenericType = IsTheRawGenericType(type);
            //         if (isTheRawGenericType) return true;
            //         type = type.BaseType;
            //     }

            //     return false;

            //     // 判断逻辑
            //     bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
            // }
        }
}
