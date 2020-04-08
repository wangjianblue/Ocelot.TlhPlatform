﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
