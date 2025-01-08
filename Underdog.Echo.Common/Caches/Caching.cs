﻿using Underdog.Echo.Common.Const;

using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Collections.Concurrent;
using Underdog.Echo.Common.Helper;
using Underdog.Echo.Common.Option;
using Underdog.Echo.Common.Caches.Interface;
using Underdog.Echo.Common.Extensions;

namespace Underdog.Echo.Common.Caches
{
    public class Caching(
        ILogger<Caching> logger,
        IDistributedCache cache,
        IOptions<RedisOptions> redisOptions)
        : ICaching
    {
        private static readonly ConcurrentDictionary<string, bool> _loggedWarnings = new();
        private readonly RedisOptions _redisOptions = redisOptions.Value;
        private const string WarningMessage = "注入的缓存服务不是MemoryCacheManager,请检查注册配置,无法获取所有KEY";
        public IDistributedCache Cache => cache;

        public void DelByPattern(string key)
        {
            var allkeys = GetAllCacheKeys(key);
            if (allkeys == null) return;

            foreach (var u in allkeys)
            {
                cache.Remove(u);
            }
        }

        /// <summary>
        /// 删除某特征关键字缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DelByPatternAsync(string key)
        {
            var allkeys = GetAllCacheKeys(key);
            if (allkeys == null) return;

            foreach (var s in allkeys) await cache.RemoveAsync(s);
        }

        public bool Exists(string cacheKey)
        {
            var res = cache.Get(cacheKey);
            return res != null;
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string cacheKey)
        {
            var res = await cache.GetAsync(cacheKey);
            return res != null;
        }

        public List<string> GetAllCacheKeys(string pattern = default)
        {
            if (_redisOptions.Enable)
            {
                var redis = App.GetService<IConnectionMultiplexer>(false);
                var endpoints = redis.GetEndPoints();
                var server = redis.GetServer(endpoints[0]);
                // 使用 SCAN 命令来增量获取符合条件的键，避免 KEYS 的性能问题
                return server.Keys(pattern: pattern, pageSize: 100).Select(key => key.ToString()).ToList();
            }

            var memoryCache = App.GetService<IMemoryCache>();
            if (memoryCache is not MemoryCacheManager memoryCacheManager)
            {
                if (_loggedWarnings.TryAdd(WarningMessage, true))
                {
                    logger.LogWarning(WarningMessage);
                }

                return [];
            }

            return memoryCacheManager.GetAllKeys().WhereIf(!pattern.IsNullOrEmpty(), s => s.StartsWith(pattern!)).ToList();
        }

        public T Get<T>(string cacheKey)
        {
            var res = cache.Get(cacheKey);
            return res == null ? default : JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(res));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var res = await cache.GetAsync(cacheKey);
            return res == null ? default : JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(res));
        }

        public object Get(Type type, string cacheKey)
        {
            var res = cache.Get(cacheKey);
            return res == null ? default : JsonConvert.DeserializeObject(Encoding.UTF8.GetString(res), type);
        }

        public async Task<object> GetAsync(Type type, string cacheKey)
        {
            var res = await cache.GetAsync(cacheKey);
            return res == null ? default : JsonConvert.DeserializeObject(Encoding.UTF8.GetString(res), type);
        }

        public string GetString(string cacheKey)
        {
            return cache.GetString(cacheKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string cacheKey)
        {
            return await cache.GetStringAsync(cacheKey);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }

        public void RemoveAll()
        {
            if (_redisOptions.Enable)
            {
                var redis = App.GetService<IConnectionMultiplexer>(false);
                var endpoints = redis.GetEndPoints();
                var server = redis.GetServer(endpoints[0]);
                server.FlushDatabase();
            }
            else
            {
                var manage = App.GetService<MemoryCacheManager>(false);
                manage.Reset();
            }
        }

        public void Set<T>(string cacheKey, T value, TimeSpan? expire = null)
        {
            cache.Set(cacheKey, GetBytes(value),
                expire == null
                    ? new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6) }
                    : new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expire });
        }

        public void SetPermanent<T>(string cacheKey, T value)
        {
            cache.Set(cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 增加对象缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string cacheKey, T value)
        {
            await cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)),
                new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6) });
        }

        /// <summary>
        /// 增加对象缓存,并设置过期时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan expire)
        {
            await cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)),
                new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expire });
        }

        public async Task SetPermanentAsync<T>(string cacheKey, T value)
        {
            await cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        public void SetString(string cacheKey, string value, TimeSpan? expire = null)
        {
            cache.SetString(cacheKey, value,
                expire == null
                    ? new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6) }
                    : new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expire });
        }

        /// <summary>
        /// 增加字符串缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetStringAsync(string cacheKey, string value)
        {
            await cache.SetStringAsync(cacheKey, value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6) });
        }

        /// <summary>
        /// 增加字符串缓存,并设置过期时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task SetStringAsync(string cacheKey, string value, TimeSpan expire)
        {
            await cache.SetStringAsync(cacheKey, value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expire });
        }

        /// <summary>
        ///  根据父键清空
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DelByParentKeyAsync(string key)
        {
            var allkeys = GetAllCacheKeys(key);
            if (allkeys == null) return;

            foreach (var s in allkeys) await cache.RemoveAsync(s);
        }

        private byte[] GetBytes<T>(T source)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(source));
        }
    }
}
