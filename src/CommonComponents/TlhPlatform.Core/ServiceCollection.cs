using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TlhPlatform.Core.Data.Core;
using TlhPlatform.Core.Dependency;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Event.Factories;
using TlhPlatform.Core.Events.Bus.Factories.Internals;
using TlhPlatform.Core.Reflection;

namespace TlhPlatform.Core
{
    /// <summary>
    /// 发射注入
    /// 暂时不支持Modules
    /// </summary>
    public class ServiceCollection
    {
        public readonly Reflection.ServiceScanOptions options;
        public ServiceCollection()
        {
            options = new Reflection.ServiceScanOptions();
        }

        public void AddServices(IServiceCollection service)
        {
            service.AddScoped<ScopedDictionary>();


            //添加即时生命周期类型的服务
            Type[] dependencyTypes = options.TransientTypeFinder.FindAll();
            AddTypeWithInterfaces(service, dependencyTypes, ServiceLifetime.Transient);

            //添加作用域生命周期类型的服务
            dependencyTypes = options.ScopedTypeFinder.FindAll();
            AddTypeWithInterfaces(service, dependencyTypes, ServiceLifetime.Scoped);

            //添加单例生命周期类型的服务
            dependencyTypes = options.SingletonTypeFinder.FindAll();
            AddTypeWithInterfaces(service, dependencyTypes, ServiceLifetime.Singleton);

            //添加Eventbus
            var Event = options.EventBusTypeFinder.FindAll();
            RegisterEventBus(Event);
        }

        protected void RegisterEventBus(Type[] consumers)
        {
            foreach (var consumer in consumers)
            {
                var ls = consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType &&
                                  ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IEventHandler<>));
                IEventHandlerFactory factory = new IocEventHandlerFactory(ls[0]);

                EventBusCommon.RegisterSingleEvent(typeof(IEventHandler<>), factory);
            }
        }

        /// <summary>
        /// 以类型实现的接口进行服务添加，需排除
        /// <see cref="IDisposable"/>等非业务接口，如无接口则注册自身
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="implementationTypes">要注册的实现类型集合</param>
        /// <param name="lifetime">注册的生命周期类型</param>
        protected virtual IServiceCollection AddTypeWithInterfaces(IServiceCollection services, Type[] implementationTypes, ServiceLifetime lifetime)
        {
            foreach (Type implementationType in implementationTypes)
            {
                if (implementationType.IsAbstract || implementationType.IsInterface)
                {
                    continue;
                }
                Type[] interfaceTypes = GetImplementedInterfaces(implementationType);
                if (interfaceTypes.Length == 0)
                {
                    services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime));
                    continue;
                }
                for (int i = 0; i < interfaceTypes.Length; i++)
                {
                    Type interfaceType = interfaceTypes[i];
                    if (lifetime == ServiceLifetime.Transient)
                    {
                        services.TryAddEnumerable(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                        continue;
                    }
                    bool multiple = interfaceType.HasAttribute<Reflection.MultipleDependencyAttribute>();
                    if (i == 0)
                    {
                        if (multiple)
                        {
                            services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                        }
                        else
                        {
                            services.TryAdd(new ServiceDescriptor(interfaceType, implementationType, lifetime));
                        }
                    }
                    else
                    {
                        //有多个接口时，后边的接口注册使用第一个接口的实例，保证同个实现类的多个接口获得同一个实例
                        Type firstInterfaceType = interfaceTypes[0];
                        if (multiple)
                        {
                            services.Add(new ServiceDescriptor(interfaceType, provider => provider.GetService(firstInterfaceType), lifetime));
                        }
                        else
                        {
                            services.TryAdd(new ServiceDescriptor(interfaceType, provider => provider.GetService(firstInterfaceType), lifetime));
                        }
                    }
                }
            }
            return services;
        }

        /// <summary>
        ///  以类型实现的接口进行服务添加，需排除
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type[] GetImplementedInterfaces(Type type)
        {
            Type[] exceptInterfaces = { typeof(IDisposable) };
            Type[] interfaceTypes = type.GetInterfaces().Where(t => !exceptInterfaces.Contains(t) && !t.HasAttribute<Reflection.IgnoreDependencyAttribute>()).ToArray();
            for (int index = 0; index < interfaceTypes.Length; index++)
            {
                Type interfaceType = interfaceTypes[index];
                if (interfaceType.IsGenericType && !interfaceType.IsGenericTypeDefinition && interfaceType.FullName == null)
                {
                    interfaceTypes[index] = interfaceType.GetGenericTypeDefinition();
                }
            }
            return interfaceTypes;
        }
    }
}
