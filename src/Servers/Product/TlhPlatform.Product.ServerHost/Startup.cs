using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Extensions.DependencyInjection;
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
using TlhPlatform.Core;
using TlhPlatform.Core.Dependency;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Events.Bus;
using TlhPlatform.Core.Filter;
using TlhPlatform.Infrastructure;
using TlhPlatform.Infrastructure.AutoMapper;
using TlhPlatform.Infrastructure.Extents;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Product.Application;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Infrastructure;
using TlhPlatform.Product.Infrastructure.HttpClientFactory;
using TlhPlatform.Product.ServerHost.Events;
using TlhPlatform.Product.ServerHost.Filter;

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

            #region 注册全局拦截器 
            Action<MvcOptions> filters = new Action<MvcOptions>(r =>
            {
                //r.Filters.Add(typeof(MyAuthorization));
                //r.Filters.Add(typeof(MyExceptionFilterAttribute));
                //r.Filters.Add(typeof(MyResourceFilterAttribute));
                //r.Filters.Add(typeof(MyActionFilterAttribute));
                //r.Filters.Add(typeof(MyResultFilterAttribute));
            });
            services.AddControllers(filters);
       

            #endregion

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

            #region Authorization

      

            #endregion


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
            EventBusCommon.RegisterTransientEvent<TodoItemEventData, TodoItemEventEmailHandler>();
            EventBusCommon.RegisterTransientEvent<TodoItemEventData, TodoItemEventSMSHandler>();

            //services.AddServices();
            #region SmartSql 
            services.AddSmartSql()
               .AddRepositoryFromAssembly(options =>
                {
                    options.AssemblyString = "TlhPlatform.Product.Repository"; 
                });
            #endregion

            #region AutoMapper

            services.AddAutoMapperException();
            #endregion

         
            services.AddHttpClient<IUserClient, UserClient>();
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseSwaggerExtents(Configuration);
            app.UseRouting();

            ServiceLocator.Instance.SetApplicationServiceProvider(app.ApplicationServices);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
