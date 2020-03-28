using Consul;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TlhPlatform.Infrastructure
{
    public static class ConfigureExtents
    {
        /// <summary>
        /// 服务器发现
        /// </summary>
        /// <param name="app"></param>
        /// <param name="applicationLifetime"></param>
        /// <param name="ServiceName"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IApplicationLifetime applicationLifetime, string ServiceName, string ip, int port)
        {
            //var services = app.ApplicationServices.CreateScope().ServiceProvider;
            //var applicationLifetime = services.GetService<IApplicationLifetime>();

            string serviceId = ServiceName + Guid.NewGuid();
            using (var client = new ConsulClient(ConsulConfig))
            {
                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = serviceId,//服务编号，不能重复，用Guid最简单
                    Name = ServiceName,//服务的名字
                    Address = ip,//服务提供者的能被消费者访问的ip地址(可以被其他应用访问的地址，本地测试可以用127.0.0.1，机房环境中一定要写自己的内网ip地址)
                    Port = port,//服务提供者的能被消费者访问的端口
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册(注销)
                        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                        HTTP = $"http://{ip}:{port}/api/health",//健康检查地址
                        Timeout = TimeSpan.FromSeconds(5)
                    }
                }).Wait();//Consult客户端的所有方法几乎都是异步方法，但是都没按照规范加上Async后缀，所以容易误导。记得调用后要Wait()或者await
            }


            //程序正常退出的时候从Consul注销服务//要通过方法参数注入IApplicationLifetime
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                using (var client = new ConsulClient(ConsulConfig))
                {
                    client.Agent.ServiceDeregister(serviceId).Wait();
                }
            });
            void ConsulConfig(ConsulClientConfiguration c)
            {
                c.Address = new Uri("http://127.0.0.1:8500");
                c.Datacenter = "dc1";
            }
            return app;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerExtents(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "doc/{documentName}/swagger.json";

            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/doc/{configuration["Service:DocName"]}/swagger.json",
                    $"{configuration["Service:Name"]} {configuration["Service:Version"]}");

            });
            return app;
        }

        /// <summary>
        /// 全局订阅
        /// </summary>
        /// <param name="app"></param>
        /// <param name="subscriptionIdPrefix"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder app, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = app.ApplicationServices.CreateScope().ServiceProvider;
            var lifeTime = services.GetService<IApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    var hasHystrixCommand = type.GetMethods().Where(p => p.IsDefined(typeof(AutoSubscriberConsumerAttribute), false)).ToList();

                    if (hasHystrixCommand.Count() > 0)
                    {
                        var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                        subscriber.Subscribe(type);
                        subscriber.SubscribeAsync(type);

                    }
                }
            });
            lifeTime.ApplicationStopped.Register(() => bus.Dispose());
            return app;

        }



    }
}
