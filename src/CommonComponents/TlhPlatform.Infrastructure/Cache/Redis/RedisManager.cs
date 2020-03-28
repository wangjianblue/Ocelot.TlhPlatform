using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace TlhPlatform.Infrastructure.Cache.Redis
{
    public class RedisManager: ICacheService
    {
        #region 字段
        protected IDatabase Cache;
        private readonly ConnectionMultiplexer _connection;
        private readonly string _instance;
        private readonly int _database = 0;
        #endregion
        public RedisManager(Action<RedisCacheOptions> redisCacheOption, int database = 0)
        {
            var options = new RedisCacheOptions();
            redisCacheOption(options);
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            Cache = _connection.GetDatabase(database);
            _instance = options.InstanceName;
            _database = database;
        }
        public bool HashSet<T>(string key, string dataKey, T t)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            var json = t is string ? t.ToString() : JsonConvert.SerializeObject(t);
            return Cache.HashSet(RealKey(key), dataKey, json);
        }

        public T HashGet<T>(string key, string dataKey)
        {
            if (string.IsNullOrEmpty(key)) return default(T);
            var value = Cache.HashGet(RealKey(key), dataKey);
            return value.IsNullOrEmpty ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public IDictionary<string, T> HashGetAll<T>(string key)
        {
            var realKey = RealKey(key);
            var dic = new Dictionary<string, T>();
            foreach (var kv in Cache.HashGetAll(realKey))
            {
                dic.Add(kv.Name, JsonConvert.DeserializeObject<T>(kv.Value));
            }

            return dic;
        }
        public bool HashExists(string key, string field)
        {
            var realKey = RealKey(key);
            return Cache.HashExists(realKey, field);
        }

        public long HashLength(string key)
        {
            var realKey = RealKey(key);
            return Cache.HashLength(realKey);
        }
        public long HashRemove(string key, IEnumerable<string> fields)
        {
            var realKey = RealKey(key);
            var redisFields = fields.Select(p => (RedisValue)p).ToArray();
            return Cache.HashDelete(realKey, redisFields);
        }

        #region SortedSet

        public bool SortedSetAdd(string key, string member, double score)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return Cache.SortedSetAdd(RealKey(key), member, score);
        }
        public bool SortedSetAdd<T>(string key, T value, double score)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return Cache.SortedSetAdd(RealKey(key), JsonConvert.SerializeObject(value), score);
        }

        /// <summary>
        ///  获取全部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> SortedSetRangeByRank<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;
            var values = Cache.SortedSetRangeByRank(RealKey(key));

            return values?.Select(item => JsonConvert.DeserializeObject<T>(item)).ToList();
        }

        /// <summary>
        /// SortedSetRangeByRank
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] SortedSetRangeByRank(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;
            var values = Cache.SortedSetRangeByRank(RealKey(key));
            return values.Select(u => (string)u).ToArray();
        }


        public string[] SortedSetRangeByScore(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;
            var values = Cache.SortedSetRangeByScore(RealKey(key));
            return values.Select(u => (string)u).ToArray();
        }

        public string[] SortedSetRangeByValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;
            var values = Cache.SortedSetRangeByValue(RealKey(key));
            return values.Select(u => (string)u).ToArray();
        }

        public bool SortedSetRemove(string key, string member)
        {
            return !string.IsNullOrEmpty(key) && Cache.SortedSetRemove(RealKey(key), member);
        }
        public bool SortedSetRemove<T>(string key, T value)
        {
            return !string.IsNullOrEmpty(key) && Cache.SortedSetRemove(RealKey(key), JsonConvert.SerializeObject(value));
        }

        #endregion


        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return Cache.KeyDelete(RealKey(key));
        }

        /// <summary>
        /// 匹配删除
        /// </summary>
        /// <param name="pattern"></param>
        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return;

            foreach (var ep in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(ep);
                var keys = server.Keys(pattern: _instance + ":" + pattern + "*", database: _database);
                foreach (var key in keys)
                {
                    Cache.KeyDelete(key);
                }
            }
        }
        /// <summary>
        /// 生成key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual string RealKey(string key)
        {
            return $"{_instance}{key}";
        }
    }
}
