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
    /// <see cref="ServiceLifetime.Singleton"/>生命周期类型的服务映射类型查找器
    /// </summary>
    public class SingletonDependencyTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="SingletonDependencyTypeFinder"/>类型的新实例
        /// </summary>
        public SingletonDependencyTypeFinder()
        {
            AllAssemblyFinder = new AppDomainAllAssemblyFinder();
        }

        /// <summary>
        /// 获取或设置 全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder AllAssemblyFinder { get; set; }

        /// <summary>
        /// 查询所有依赖
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(ISingletonDependency);
            Type[] types = AllAssemblyFinder.FindAll(formCache: true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && baseType.IsAssignableFrom(type) && !type.HasAttribute<IgnoreDependencyAttribute>() && !type.IsAbstract)
                .ToArray();
            return types;
        }
    }
}
