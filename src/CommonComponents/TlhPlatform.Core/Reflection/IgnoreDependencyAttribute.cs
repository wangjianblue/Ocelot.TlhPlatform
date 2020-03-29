using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TlhPlatform.Core.Reflection
{
    /// <summary>
    /// 标记了此特性的类，将忽略依赖注入自动映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreDependencyAttribute : System.Attribute
    {
    }


}
