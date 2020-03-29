using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TlhPlatform.Core.Reflection.Dependency;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Core.Reflection.Finder
{
    public class TransientDependencyTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 获取或设置，全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder allAssemblyFinder { get; set; }

        /// <summary>
        /// 获取或设置 全部程序集查找器
        /// </summary>
        public TransientDependencyTypeFinder()
        {
            allAssemblyFinder = new AppDomainAllAssemblyFinder();
        }
        /// <summary>
        /// 查询所有依赖
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(ITransientDependency); 
            Type[] types = allAssemblyFinder.FindAll(formCache: true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.HasAttribute<IgnoreDependencyAttribute>() && !type.IsAbstract && !type.IsInterface)
                .ToArray(); 
            return types;

        }
    }
}
