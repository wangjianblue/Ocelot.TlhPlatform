using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Product.Domain.Mime
{
    public class MailInfoData
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }

        public string Host { get; set; } = "smtp-mail.outlook.com";
        public int Port { get; set; } = 587;
        public SecureSocketOptions SetOptions { get; set; } = SecureSocketOptions.StartTls;

        public string Name { get; set; }
        public string Address { get; set; }
    }
}
