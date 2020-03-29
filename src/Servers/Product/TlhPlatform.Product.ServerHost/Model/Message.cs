using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TlhPlatform.Product.ServerHost.Model
{
    public class Message
    {
        public string MessageID { get; set; }

        public string MessageTitle { get; set; }

        public string MessageBody { get; set; }

        public string MessageRouter { get; set; }
    }
}
