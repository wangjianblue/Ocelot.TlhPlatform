using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;

namespace TlhPlatform.Gateway.WebApi
{ 
    public class Program
    {
        //public static void Main(string[] args)
        //{ 
        //    CreateWebHostBuilder(args).Build().Run();
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //    .ConfigureAppConfiguration((hostingContext, obj) =>
        //    {
        //            obj.AddOcelot(hostingContext.HostingEnvironment)
        //            .AddEnvironmentVariables();
        //    })
        //    .UseStartup<Startup>();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, obj) =>
                {
                   // obj.AddOcelot(hostingContext.HostingEnvironment).AddEnvironmentVariables();

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
