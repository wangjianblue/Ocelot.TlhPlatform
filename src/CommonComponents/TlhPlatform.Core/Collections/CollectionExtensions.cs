using System.Collections.Generic;
using TlhPlatform.Core.Data;


namespace TlhPlatform.Core.Collections
{
    /// <summary>
    /// 集合扩展方法
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 如果不存在，添加项
        /// </summary>
        public static void AddIfNotExist<T>(this ICollection<T> collection, T value)
        {
            Check.NotNull(collection, nameof(collection));
            if (!collection.Contains(value))
            {
                collection.Add(value);
            }
        }
        /// <summary>
        /// 检测当前源是否为空
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }
        /// <summary>
        /// 如果不为空，添加项
        /// </summary>
        public static void AddIfNotNull<T>(this ICollection<T> collection, T value) where T : class
        {
            Check.NotNull(collection, nameof(collection));
            if (value != null)
            {
                collection.Add(value);
            }
        }
    }
}
