using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Core.Reflection.Finder;

namespace TlhPlatform.Core.Reflection
{
    /// <summary>
    /// 依赖注入服务类型扫描配置信息
    /// </summary>
    public class ServiceScanOptions
    {
        /// <summary>
        /// 初始化一个<see cref="ServiceScanOptions"/>类型的新实例
        /// </summary>
        public ServiceScanOptions()
        {
            TransientTypeFinder = new TransientDependencyTypeFinder();
            ScopedTypeFinder = new ScopedDependencyTypeFinder();
            SingletonTypeFinder = new SingletonDependencyTypeFinder();
        }

        /// <summary>
        /// 获取或设置 即时生命周期服务类型查找器
        /// </summary>
        public ITypeFinder TransientTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 作用域生命周期服务类型查找器
        /// </summary>
        public ITypeFinder ScopedTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 单例生命周期服务类型查找器
        /// </summary>
        public ITypeFinder SingletonTypeFinder { get; set; }
    }
}
