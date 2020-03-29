using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Core.Reflection
{
    /// <summary>
    /// 定义程序集查找器
    /// </summary>
    [IgnoreDependency]
    public interface IAllAssemblyFinder:Finders<Assembly>
    {

    }
}
