using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Infrastructure.RabbitMQ
{
    public class RabbitOptions
    {
        /// <summary>
        /// Default password (value: "guest").
        /// </summary>
        /// <remarks>PLEASE KEEP THIS MATCHING THE DOC ABOVE.</remarks>
        public const string DefaultPass = "guest";

        /// <summary>
        /// Default user name (value: "guest").
        /// </summary>
        /// <remarks>PLEASE KEEP THIS MATCHING THE DOC ABOVE.</remarks>
        public const string DefaultUser = "guest";

        /// <summary>
        /// Default virtual host (value: "/").
        /// </summary>
        /// <remarks> PLEASE KEEP THIS MATCHING THE DOC ABOVE.</remarks>
        public const string DefaultVHost = "/";

        /// <summary>
        /// Default exchange name (value: "cap.default.router").
        /// </summary>
        public const string DefaultExchangeName = "rabbitmqExch.default.router";

        /// <summary> The topic exchange type. </summary>
        public const string ExchangeType = "topic";

        /// <summary> 
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        ///  
        /// </summary>
        public string Password { get; set; } = DefaultPass;

        /// <summary>
        ///  
        /// </summary>
        public string UserName { get; set; } = DefaultUser;

        /// <summary>
        ///  
        /// </summary>
        public string VirtualHost { get; set; } = DefaultVHost;

        /// <summary>
        ///  
        /// </summary>
        public string ExchangeName { get; set; } = DefaultExchangeName;

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } = -1;
    }
}
