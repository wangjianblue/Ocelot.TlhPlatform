using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Consul;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TlhPlatform.Infrastructure;
using TlhPlatform.Infrastructure.Extents;


namespace TlhPlatform.Order.ServerHost
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwagger(Configuration);
            services.AddSingleton(Configuration);
   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSubscribe("TlhPlatform.Order.ServerHost", Assembly.GetExecutingAssembly());
            app.UseMvc();
            app.UseSwaggerExtents(Configuration);

            app.UseConsul(applicationLifetime,"orderService", "127.0.0.1", 61571);
            //app.UseConsul(applicationLifetime, new ConsulModel()
            //{ 
            //    IP = Configuration["Service:IP"],
            //    ServiceName = Configuration["Service:Name"],
            //    Port = Convert.ToInt32(Configuration["Service:Port"]) 
            //});


        }



    }
}
