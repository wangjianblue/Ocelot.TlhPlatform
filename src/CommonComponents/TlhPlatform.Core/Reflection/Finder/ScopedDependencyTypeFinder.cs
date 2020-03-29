using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using TlhPlatform.Core.Reflection.Dependency;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Core.Reflection.Finder
{
    /// <summary>
    /// <see cref="ServiceLifetime.Scoped"/>生命周期类型的服务映射查找器
    /// </summary>
    public class ScopedDependencyTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="ScopedDependencyTypeFinder"/>类型的新实例
        /// </summary>
        public ScopedDependencyTypeFinder()
        {
            AllAssemblyFinder = new AppDomainAllAssemblyFinder();
        }

        /// <summary>
        /// 获取或设置 全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder AllAssemblyFinder { get; set; }

        /// <inheritdoc />
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(IScopeDependency);
            Type[] types = AllAssemblyFinder.FindAll(formCache: true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.HasAttribute<IgnoreDependencyAttribute>() && !type.IsAbstract && !type.IsInterface)
                .ToArray();
            return types;
        }
    }
}
