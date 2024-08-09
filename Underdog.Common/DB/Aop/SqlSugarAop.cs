﻿using Underdog.Common.Helper;
using Underdog.Common.LogHelper;
using Underdog.Model.Models.RootTkey;

using Serilog;
using SqlSugar;

using StackExchange.Profiling;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Underdog.Common.Utility;

namespace Underdog.Common.DB.Aop
{
    public static class SqlSugarAop
    {
        public static void OnLogExecuting(ISqlSugarClient sqlSugarScopeProvider, string user, string table, string operate, string sql,
            SugarParameter[] p, ConnectionConfig config)
        {
            try
            {
                MiniProfiler.Current.CustomTiming($"ConnId:[{config.ConfigId}] SQL：", GetParas(p) + "【SQL语句】：" + sql);

                if (!AppSettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool()) return;

                if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToConsole", "Enabled" }).ObjToBool() ||
                    AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToFile", "Enabled" }).ObjToBool() ||
                    AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToDB", "Enabled" }).ObjToBool())
                {
                    using (LogContextExtension.Create.SqlAopPushProperty(sqlSugarScopeProvider))
                    {
                        Log.Information(
                            "------------------ \r\n User:[{User}]  Table:[{Table}]  Operate:[{Operate}] ConnId:[{ConnId}]【SQL语句】: \r\n {Sql}",
                            user, table, operate, config.ConfigId, UtilMethods.GetNativeSql(sql, p));
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error occured OnLogExcuting:" + e);
            }
        }

        public static void DataExecuting(object oldValue, DataFilterModel entityInfo)
        {
            if (entityInfo.EntityValue is RootEntityTkey<long> rootEntity)
            {
                if (rootEntity.Id == 0)
                {
                    rootEntity.Id = IdGeneratorUtility.NextId();
                }
            }

            if (entityInfo.EntityValue is BaseEntity baseEntity)
            {
                // 新增操作
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    if (baseEntity.CreateTime == DateTime.MinValue)
                    {
                        baseEntity.CreateTime = DateTime.Now;
                    }
                }

                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    baseEntity.ModifyTime = DateTime.Now;
                }


                //if (App.User?.ID > 0)
                //{
                //    if (baseEntity is ITenantEntity tenant && App.User.TenantId > 0)
                //    {
                //        if (tenant.TenantId == 0)
                //        {
                //            tenant.TenantId = App.User.TenantId;
                //        }
                //    }

                //    switch (entityInfo.OperationType)
                //    {
                //        case DataFilterType.UpdateByObject:
                //            baseEntity.ModifyId = App.User.ID;
                //            baseEntity.ModifyBy = App.User.Name;
                //            break;
                //        case DataFilterType.InsertByObject:
                //            if (baseEntity.CreateBy.IsNullOrEmpty() || baseEntity.CreateId is null or <= 0)
                //            {
                //                baseEntity.CreateId = App.User.ID;
                //                baseEntity.CreateBy = App.User.Name;
                //            }

                //            break;
                //    }
                //}
            }
            else
            {
                //兼容以前的表 
                //这里要小心 在AOP里用反射 数据量多性能就会有问题
                //要么都统一使用基类
                //要么考虑老的表没必要兼容老的表
                //

                var getType = entityInfo.EntityValue.GetType();

                switch (entityInfo.OperationType)
                {
                    case DataFilterType.InsertByObject:
                        var dyCreateBy = getType.GetProperty("CreateBy");
                        var dyCreateId = getType.GetProperty("CreateId");
                        var dyCreateTime = getType.GetProperty("CreateTime");

                        //if (App.User?.ID > 0 && dyCreateBy != null && dyCreateBy.GetValue(entityInfo.EntityValue) == null)
                        //    dyCreateBy.SetValue(entityInfo.EntityValue, App.User.Name);

                        //if (App.User?.ID > 0 && dyCreateId != null && dyCreateId.GetValue(entityInfo.EntityValue) == null)
                        //    dyCreateId.SetValue(entityInfo.EntityValue, App.User.ID);

                        if (dyCreateTime != null && dyCreateTime.GetValue(entityInfo.EntityValue) != null &&
                            (DateTime)dyCreateTime.GetValue(entityInfo.EntityValue) == DateTime.MinValue)
                            dyCreateTime.SetValue(entityInfo.EntityValue, DateTime.Now);

                        break;
                    case DataFilterType.UpdateByObject:
                        var dyModifyBy = getType.GetProperty("ModifyBy");
                        var dyModifyId = getType.GetProperty("ModifyId");
                        var dyModifyTime = getType.GetProperty("ModifyTime");

                        //if (App.User?.ID > 0 && dyModifyBy != null)
                        //    dyModifyBy.SetValue(entityInfo.EntityValue, App.User.Name);

                        //if (App.User?.ID > 0 && dyModifyId != null)
                        //    dyModifyId.SetValue(entityInfo.EntityValue, App.User.ID);

                        if (dyModifyTime != null)
                            dyModifyTime.SetValue(entityInfo.EntityValue, DateTime.Now);
                        break;
                }
            }
        }

        private static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql = sql.Replace(param.ParameterName, $@"'{param.Value.ObjToString()}'");
            }

            return sql;
        }

        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }
    }
}
