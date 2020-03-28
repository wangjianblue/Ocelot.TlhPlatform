using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartSql.ConfigBuilder;
using TlhPlatform.Infrastructure;
using TlhPlatform.Infrastructure.Extents;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Product.Application;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.TodoI;

namespace TlhPlatform.Product.ServerHost
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
            //services.AddSwagger(Configuration);
            //将SwaggerGen服务添加到服务集合中， OpenApiInfo是它的自带类。
            services.AddSwaggerGen(c =>
            {
                //注意不能用大写V1，不然老报错，Not Found /swagger/v1/swagger.json
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddControllers();
            //services.AddRedis(p =>
            //{
            //    p.Configuration = "";
            //    p.InstanceName = "212";
            //});
            services.AddCacheKey(p =>
            {
                p.FileName = "KeyConfigList.xml";
                p.FilePath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Configs\\Cache\\";
            });

            //#region RabbitMQ
            //var rabbitOptions = Configuration.GetSection("RabbitOptions").Get<RabbitOptions>();
            //var rabbit = services.AddReceives(action =>
            //{
            //    action.HostName = rabbitOptions.HostName;
            //    action.Port = rabbitOptions.Port;
            //    action.UserName = rabbitOptions.UserName;
            //    action.Password = rabbitOptions.Password;
            //    action.VirtualHost = rabbitOptions.VirtualHost;
            //});
            //rabbit?.Order_Receive1();
            //rabbit?.Order_Receive2();

            //#endregion
            services.AddTransient<ITodoItemService, TodoItemService>();
         
            #region SmartSql
            
           services.AddSmartSql() 
               .AddRepositoryFromAssembly(options =>
                {
                    options.AssemblyString = "TlhPlatform.Product.Repository";
                
                });

            #endregion

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Configs")),
            //    RequestPath = "/Configs"
            //});
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseSwaggerExtents(Configuration);
            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
