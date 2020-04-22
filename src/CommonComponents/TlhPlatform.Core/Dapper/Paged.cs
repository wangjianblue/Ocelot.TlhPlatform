using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Dapper
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class Paged
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页数据行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public long TotalRow { get; set; }
        /// <summary>
        /// 总的分页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (this.TotalRow > 0 && this.PageSize > 0)
                    return (int)Math.Ceiling((decimal)this.TotalRow / this.PageSize);
                else
                    return 0;
            }
        }
    }
}
