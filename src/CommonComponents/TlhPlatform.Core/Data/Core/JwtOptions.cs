using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Data.Core
{
    /// <summary>
    /// JWT身份认证选项
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// 获取或设置 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 获取或设置 发行方
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 获取或设置 订阅方
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Token 过去时间分钟
        /// </summary>
        public int Expires { get; set; }
    }
}
