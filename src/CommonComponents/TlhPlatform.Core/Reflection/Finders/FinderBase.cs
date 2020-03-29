using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TlhPlatform.Core.Reflection.Finders
{
    /// <summary>
    /// 查找器基类
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class FinderBase<TItem> : Finders<TItem>
    {
        private readonly object _lockObj = new object();
        /// <summary>
        /// 缓存
        /// </summary>
        protected readonly List<TItem> ItemsCache = new List<TItem>();

        /// <summary>
        /// 是否查找过
        /// </summary>
        protected bool Found = false;
        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="perdicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        public TItem[] Find(Func<TItem, bool> perdicate, bool fromCache = false)
        {
            return FindAll(fromCache).Where(perdicate).ToArray();
        }
        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="formCache"></param>
        /// <returns></returns>
        public TItem[] FindAll(bool formCache = false)
        {
            lock (_lockObj)
            {
                if (formCache && Found)
                {
                    return ItemsCache.ToArray();
                }
                TItem[] items = FindAllItems();
                Found = true;
                ItemsCache.Clear();
                ItemsCache.AddRange(items);
                return items;
            }
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected abstract TItem[] FindAllItems();
    }
}
