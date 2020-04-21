using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AspectCore.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;

namespace TlhPlatform.Product.ServerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((logging) =>
                {
                    logging.AddFilter("System",LogLevel.Warning);
                    logging.AddFilter("Microsoft",LogLevel.Warning);
                    logging.AddLog4Net("Configs/Exception/log4net.config"); 
                }) 
                .UseServiceProviderFactory(new DynamicProxyServiceProviderFactory())//autofac ÒÀÀµ×¢Èë
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
