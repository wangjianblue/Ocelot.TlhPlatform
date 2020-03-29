using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace TlhPlatform.Core.Dependency
{
    public class ScopedDictionary : ConcurrentDictionary<string, object>, IDisposable
    { 
        /// <summary>
        /// 获取或设置 当前用户
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
