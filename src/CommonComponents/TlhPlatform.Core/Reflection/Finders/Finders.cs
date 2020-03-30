using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TlhPlatform.Core.Reflection.Finders
{
    /// <summary>
    /// 定义一个查找器
    /// </summary>
    [IgnoreDependency]
    public interface Finders<out TItem>
    {

        /// <summary> 
        /// 查找指定条件的项
        /// </summary>
        /// <param name="perdicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        TItem[] Find(Func<TItem,bool> perdicate,bool fromCache = false);

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="formCache"></param>
        /// <returns></returns>
        TItem[] FindAll(bool formCache = false);

        IList<Assembly> GetAssemblies();

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
