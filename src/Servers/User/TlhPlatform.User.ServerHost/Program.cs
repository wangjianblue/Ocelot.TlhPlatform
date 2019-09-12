using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Thrift.Server;
using Thrift.Transport;
using TlhPlatform.User.ServerHost.Extents;

namespace apione
{
    public class Program
    {
        private const int port = 8885;
        public static void Main(string[] args)
        {
     

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        { 
            return WebHost.CreateDefaultBuilder(args)  
                .UseStartup<Startup>();
        }


    }
}
