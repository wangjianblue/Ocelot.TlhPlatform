using System;

namespace TlhPlatform.Infrastructure.Hangfire
{
    /// <summary>
    /// Hangfire配置项从配置文件读取
    /// </summary>
    public class HangfireOption
    {
        /// <summary>
        /// 数据库连接地址
        /// </summary>
        public string DBConnectionString;
        /// <summary>
        /// 监控界面路径
        /// </summary>
        public string Dashboard;
        /// <summary>
        /// 重试次数
        /// </summary>
        public int Retry;
        /// <summary>
        /// 并发线程数
        /// </summary>
        public int MaxThread;
    }
}
