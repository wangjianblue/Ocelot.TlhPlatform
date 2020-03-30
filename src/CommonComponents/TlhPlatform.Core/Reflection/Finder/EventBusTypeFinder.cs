using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Event.Factories;
using TlhPlatform.Core.Events.Bus.Factories.Internals;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Core.Reflection.Finder
{
    public class EventBusTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 获取或设置 全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder AllAssemblyFinder { get; set; }
        /// <summary>
        /// 初始化一个<see cref="ScopedDependencyTypeFinder"/>类型的新实例
        /// </summary>
        public EventBusTypeFinder()
        {
            AllAssemblyFinder = new AppDomainAllAssemblyFinder();
        }

        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(IAsyncEventHandler<>); 
            var consumers = AllAssemblyFinder.FindAll(formCache: false).SelectMany(assembly => assembly.GetTypes())
                .Where(type =>  !type.IsAbstract && !type.IsInterface&& baseType.IsAssignableFrom(type))
                .ToArray(); 

            return consumers;
           
        }
    }
}
