using System;
using System.Linq;
using TlhPlatform.Infrastructure.MongoDB;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.Mime;
using TlhPlatform.Product.Infrastructure.MimeKit;

namespace TlhPlatform.Product.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageService : IMessageService
    {
        public readonly SendServerConfigurationEntity SendServerConfiguration = null;
        public readonly IMongoRepository MongoRepository;

        public MessageService(IMongoRepository iMongoRepository, Action<SendServerConfigurationEntity> configure)
        {
            MongoRepository = iMongoRepository;
            configure(SendServerConfiguration = new SendServerConfigurationEntity());
        }

        /// <summary>
        /// 发送邮件
        /// </summary> 
        /// <param name="mailBodyEntity">邮件标题和内容</param>
        /// <param name="mailAction">发送人</param>
        public void SendEmail(MailBodyEntity mailBodyEntity, Action<SendServerConfigurationEntity> mailAction = null)
        {
            if (mailBodyEntity?.Receiving.Count() == null)
                return;
            mailAction?.Invoke(SendServerConfiguration);
            try
            {
                var result = SeedMailHelper.SendMail(mailBodyEntity, SendServerConfiguration);
                
                Console.WriteLine("成功！");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            } 
        }

        public void SendSMS()
        {

        }

    }
}
