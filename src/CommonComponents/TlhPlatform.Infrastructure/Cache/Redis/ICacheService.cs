using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Infrastructure.Cache.Redis
{
    public interface ICacheService
    {
        #region Hash类型
        bool HashSet<T>(string key, string dataKey, T t);
        T HashGet<T>(string key, string dataKey);
        #endregion
    }
}
