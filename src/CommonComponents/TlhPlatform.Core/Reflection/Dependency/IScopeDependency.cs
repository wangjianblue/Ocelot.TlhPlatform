using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using TlhPlatform.Core.Reflection;

namespace TlhPlatform.Core.Reflection.Dependency
{
    /// <summary>
    /// 实现此接口的类型将被注册为<see cref="ServiceLifetime.Scoped"/>模式
    /// </summary>
    [IgnoreDependency]
    public interface IScopeDependency
    { }
}
