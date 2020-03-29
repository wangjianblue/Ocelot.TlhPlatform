using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Core.Reflection.Finders;

namespace TlhPlatform.Core.Reflection.Finder
{
    public class AutoMapTypeFinder : FinderBase<Type>, ITypeFinder
    {
        public AutoMapTypeFinder()
        {
            allAssemblyFinder = new AppDomainAllAssemblyFinder();
        }
        /// <summary>
        /// 获取货设置，全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder allAssemblyFinder { get; set; }

        protected override Type[] FindAllItems()
        {
            throw new NotImplementedException();
        }
    }
}
