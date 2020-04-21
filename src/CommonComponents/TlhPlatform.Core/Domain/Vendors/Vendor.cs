using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Domain.Vendors
{
    /// <summary>
    /// 供应商实体
    /// </summary>
    [Serializable]
    public partial class Vendor 
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the admin comment管理员备注
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order 显示顺序
        /// </summary>
        public int? DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }
        /// <summary>
        /// 代号
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// 供应商登录用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string SimpleName { get; set; }
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 市话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobel { get; set; }
        /// <summary>
        /// 供应商地区映射表ID
        /// </summary>
        public int VendorCityId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// 统一编号
        /// </summary>
        public string UnifiedCode { get; set; }
        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers can select the page size
        /// </summary>
        public bool AllowCustomersToSelectPageSize { get; set; }
        /// <summary>
        /// Gets or sets the available customer selectable page size options
        /// </summary>
        public string PageSizeOptions { get; set; }

        ///// <summary>
        ///// 是否是自营供应商
        ///// </summary>
        //public bool IsLocalVendor { get; set; }

        /// <summary>
        /// 供应商物流负责人Email
        /// </summary>
        public string TransContactEmail { get; set; }

        /// <summary>
        /// 临时字段 已延迟
        /// </summary>
        public int Delayed { get; set; }
        /// <summary>
        /// 临时字段 未延迟
        /// </summary>
        public int NotDelayed { get; set; }
        /// <summary>
        /// 临时字段 未发货
        /// </summary>
        public int NotPost { get; set; }
    }
}
