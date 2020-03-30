using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Consul;
using EasyNetQ;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TlhPlatform.Infrastructure;
using TlhPlatform.Infrastructure.Exceptionless;
using TlhPlatform.Infrastructure.Extents;
using TlhPlatform.User.ServerHost.Extents;


namespace apione
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { /*options.Filters.Add<GlobalExceptionFilter>(); */});
            services.AddRegistrServices(this.GetType().Assembly);
            services.AddSwagger(Configuration);
            services.AddSingleton(Configuration);
      
        
            return services.ToServiceContainer().Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionless(Configuration["ExceptionLess:ApiKey"]);
            app.UseThriftServer();
            app.UseMvc();
            app.UseSwaggerExtents(Configuration);

            app.UseConsul(applicationLifetime,"userService", "127.0.0.1", 61601);
            //app.UseConsul(applicationLifetime,new ConsulModel()
            //{
            //    IP = Configuration["Service:IP"],
            //    ServiceName = Configuration["Service:Name"],
            //    Port = Convert.ToInt32(Configuration["Service:Port"])
            //}); 
        } 
    }
}
