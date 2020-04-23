using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Product.Domain.Mime
{
    /// <summary>
    /// 邮件发送服务器配置
    /// </summary>
    public class SendServerConfigurationEntity
    {
        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string SenderAccount { get; set; }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string SenderPassword { get; set; }

        /// <summary>
        /// 邮件编码
        /// </summary>
        public string MailEncoding { get; set; }
        /// <summary>
        /// 邮箱SMTP服务器地址
        /// </summary>
        public string Host { get; set; } = "smtp-mail.outlook.com";
        /// <summary>
        /// 邮箱SMTP服务器端口
        /// </summary>
        public int Port { get; set; } = 587;
        /// <summary>
        /// 是否启用IsSsl
        /// </summary>
        public bool IsSsl { get; set; }
      
      
    }
}
