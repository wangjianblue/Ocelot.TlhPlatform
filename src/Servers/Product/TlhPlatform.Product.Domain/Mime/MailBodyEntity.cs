using System;
using System.Collections.Generic;
using System.Text;
using MimeKit;
using MimeKit.Text;

namespace TlhPlatform.Product.Domain.Mime
{
    public class MailBodyEntity
    {
        /// <summary>
        /// 邮件内容类型
        /// </summary>
        public TextFormat MailBodyType { get; set; } = TextFormat.Html;

        /// <summary>
        /// 邮件附件集合
        /// </summary>
        public List<MailFile> MailFiles { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }
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
        public IEnumerable<string> Receiving { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        public IEnumerable<string> Cc { get; set; }
        /// <summary>
        /// 密送
        /// </summary>
        public List<string> Bcc { get; set; }
    }
}
