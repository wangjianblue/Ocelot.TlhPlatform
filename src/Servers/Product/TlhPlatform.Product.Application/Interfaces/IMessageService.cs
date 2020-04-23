using System;
using System.Collections.Generic;
using System.Text;
using MimeKit;
using TlhPlatform.Product.Domain.Mime;

namespace TlhPlatform.Product.Application.Interfaces
{
    public interface IMessageService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiving">收件人集合</param>
        /// <param name="cc">抄送人集合</param>
        /// <param name="email">邮件标题和内容</param>
        /// <param name="mailAction">发送人</param>
        void SendEmail(EmailMessage email, Action<MailInfoData> mailAction = null);

        /// <summary>
        /// 
        /// </summary>
        void SendSMS();

    }
}
