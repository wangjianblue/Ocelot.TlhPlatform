using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Core.Reflection
{
    /// <summary>
    /// 定义类型查找行为
    /// </summary>
    [IgnoreDependency]
    public interface ITypeFinder:Finders<Type>
    {
    }
}
