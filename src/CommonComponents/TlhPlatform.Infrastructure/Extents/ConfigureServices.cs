using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TlhPlatform.Infrastructure.Extents
{
    public static class ConfigureServices
    {
        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(configuration["Service:DocName"], new OpenApiInfo()
                {
                    Title = configuration["Service:Title"],
                    Version = configuration["Service:Version"],
                    Description = configuration["Service:Description"],
                    Contact = new OpenApiContact
                    {
                        Name = configuration["Service:Contact:Name"],
                        Email = configuration["Service:Contact:Email"]
                    }
                });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, configuration["Service:XmlFile"]);
                s.IncludeXmlComments(xmlPath);
            });
            return services;
        }
        /// <summary>
        /// CAP模式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCAP(this IServiceCollection services, IConfiguration configuration)
        {
            // CAP
            services.AddCap(x =>
            { 
                x.UseMongoDB(configuration["DB:OrderDB"]); // SQL Server

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = configuration["MQ:Host"];
                    cfg.VirtualHost = configuration["MQ:VirtualHost"];
                    cfg.Port = Convert.ToInt32(configuration["MQ:Port"]);
                    cfg.UserName = configuration["MQ:UserName"];
                    cfg.Password = configuration["MQ:Password"];
                }); // RabbitMQ

                // Below settings is just for demo
                x.FailedRetryCount = 2;
                x.FailedRetryInterval = 5;
            });

            return services;
        }

        /// <summary>
        /// EasyNetQ RabbitMq 通信
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingleton(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(RabbitHutch.CreateBus(configuration["MQ:Dev"]));

            return services;
        }


        /// <summary>
        /// 封装一个Hystrix
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddRegistrServices(this IServiceCollection services, Assembly asm)
        {
            foreach (var type in asm.GetExportedTypes())
            {
                bool hasHystrixCommand = type.GetMethods().Any(m =>
                    m.GetCustomAttribute(typeof(HystrixCommandAttribute)) != null);
                if (hasHystrixCommand)
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}
