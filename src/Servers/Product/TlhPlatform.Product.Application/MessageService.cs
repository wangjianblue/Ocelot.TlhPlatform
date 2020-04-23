using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit.Text;
using TlhPlatform.Infrastructure.MongoDB;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.Mime;

namespace TlhPlatform.Product.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageService : IMessageService
    {
        public readonly MailInfoData MailInfo = null;
        public readonly IMongoRepository MongoRepository;

        public MessageService(IMongoRepository iMongoRepository, Action<MailInfoData> configure)
        {
            MongoRepository = iMongoRepository; 
            configure(MailInfo = new MailInfoData());
        }

        /// <summary>
        /// 发送邮件
        /// </summary> 
        /// <param name="email">邮件标题和内容</param>
        /// <param name="mailAction">发送人</param>
        public void SendEmail(EmailMessage email, Action<MailInfoData> mailAction = null)
        {
            if (email?.Receiving.Count() == null)
                return;
            mailAction?.Invoke(MailInfo);
            var messageToSend = new MimeMessage
            {
                Sender = new MailboxAddress(MailInfo.Name, MailInfo.Address),
                Subject = email.Subject,
                Body = new TextPart(TextFormat.Html) { Text = email.Body },
            };
            try
            {
                messageToSend.From.Add(new MailboxAddress(MailInfo.Name, MailInfo.Address));
                messageToSend.To.AddRange(email.Receiving);
                messageToSend.Cc.AddRange(email.Cc);
                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                smtp.MessageSent += (sender, args) =>
                {
                    if (smtp != null)
                    {
                        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        smtp.ConnectAsync(MailInfo.Host, MailInfo.Port, Enum.Parse<SecureSocketOptions>(MailInfo.SetOptions));
                        // ReSharper disable once AccessToDisposedClosure
                        smtp.AuthenticateAsync(MailInfo.UserName, MailInfo.PassWord);
                        // ReSharper disable once AccessToDisposedClosure
                        smtp.SendAsync(messageToSend);
                        // ReSharper disable once AccessToDisposedClosure
                        smtp.DisconnectAsync(true);
                        /*
                         * 记录人MoogDB中
                         *
                         */
                        MongoRepository.AddAsync(email);
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void SendSMS()
        {

        }
    }
}
