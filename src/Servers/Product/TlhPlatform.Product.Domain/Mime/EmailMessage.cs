using System;
using System.Collections.Generic;
using System.Text;
using MimeKit;

namespace TlhPlatform.Product.Domain.Mime
{
    public class EmailMessage
    {
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public IEnumerable<InternetAddress> Receiving { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        public IEnumerable<InternetAddress> Cc { get; set; }
    }
}
