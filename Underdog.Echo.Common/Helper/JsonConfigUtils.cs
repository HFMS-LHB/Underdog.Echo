﻿using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Echo.Common.Helper
{
    /// <summary>
    /// Json 配置文件通用类
    /// </summary>
    public static class JsonConfigUtils
    {
        #region 变量

        /// <summary>
        /// 锁
        /// </summary>
        private static object __Lock__ = new object();

        #endregion

        /// <summary>
        /// 读取配置文件的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">要读取json的名称</param>
        /// <param name="key">要读取的json节点名称</param>
        /// <returns></returns>
        public static T GetAppSettings<T>(IConfiguration config, string AppSettingsFileName, string key) where T : class, new()
        {
            lock (__Lock__)
            {
                if (config == null)
                {
                    config = new ConfigurationBuilder()
                        .Add(new JsonConfigurationSource
                        {
                            Path = AppSettingsFileName,
                            Optional = false,
                            ReloadOnChange = true
                        })
                        .Build();
                }
                var appconfig = new ServiceCollection()
                    .AddOptions()
                    .Configure<T>(config.GetSection(key))
                    .BuildServiceProvider()
                    .GetService<IOptions<T>>()
                    .Value;

                return appconfig;
            }
        }


        public static string GetJson(string jsonPath, string key)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile(jsonPath).Build(); //json文件地址
            string s = config.GetSection(key).Value; //json某个对象
            return s;
        }
    }
}
