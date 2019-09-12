using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using TlhPlatform.Gateway.WebApi;
using TlhPlatform.Infrastructure.Extents;

namespace GateTlhPlatform.Gateway.WebApi
{ 
    public class Program
    {
        public static void Main(string[] args)
        { 
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, obj) =>
            {
                    obj.AddOcelot(hostingContext.HostingEnvironment)
                    .AddEnvironmentVariables();
            })
            .UseStartup<Startup>();
    }
}
