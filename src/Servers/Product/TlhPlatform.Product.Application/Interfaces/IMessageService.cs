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
        /// <param name="email">邮件标题和内容</param>
        /// <param name="mailAction">发送人</param>
       void SendEmail(MailBodyEntity email, Action<SendServerConfigurationEntity> mailAction = null);

        /// <summary>
        /// 
        /// </summary>
        void SendSMS();

    }
}
