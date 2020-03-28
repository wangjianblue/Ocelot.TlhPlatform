using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Infrastructure.Cache.Redis
{
    public class RedisCacheOptions
    {
        //
        // 摘要:
        //     The configuration used to connect to Redis.
        public string Configuration { get; set; }
        //
        // 摘要:
        //     The Redis instance name.
        public string InstanceName { get; set; }
    }
}
