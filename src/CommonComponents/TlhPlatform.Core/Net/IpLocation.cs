

namespace TlhPlatform.Core.Net
{
    /// <summary>
    /// IP位置信息类
    /// </summary>
    public class IpLocation
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// IP地址所属国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 位置信息
        /// </summary>
        public string Local { get; set; }
    }
}