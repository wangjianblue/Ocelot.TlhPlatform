using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    [Serializable]
    public partial class Customer 
    {
         
        /// <summary>
        /// Ctor
        /// </summary>
        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid(); 
        }

        /// <summary>
        /// Gets or sets the customer Guid
        /// </summary>
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the pwd
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the pwd format
        /// </summary>
        public int PasswordFormatId { get; set; }
    
        /// <summary>
        /// Gets or sets the pwd salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is tax exempt
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the vendor identifier with which this customer is associated (maganer)
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this customer has some products in the shopping cart
        /// <remarks>The same as if we run this.ShoppingCartItems.Count > 0
        /// We use this property for performance optimization:
        /// if this property is set to false, then we do not need to load "ShoppingCartItems" navifation property for each page load
        /// It's used only in a couple of places in the presenation layer
        /// </remarks>
        /// </summary>
        public bool HasShoppingCartItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 访问校验票据
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }
        /// <summary>
        /// 市话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobel { get; set; }
        /// <summary>
        /// 线上账户余额
        /// </summary>
        public decimal? OnlineBalance { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        //public DateTime? Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        //public int? Sex { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Job { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        //public string ZipCode { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 会员积分
        /// </summary>
        //public int Score { get; set; }
        /// <summary>
        /// 来源 是否官网注册
        /// </summary>
        public int IsWebreg { get; set; }

        /// <summary>
        /// 账户类型Id
        /// </summary>
        public int CustomerAccountTypeId { get; set; }

        /// <summary>
        /// 会员标签（老年，时尚等）多个用，分隔符隔开;
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 是否接受订阅
        /// </summary>
        public bool IsSubscribe { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// 第三方授权
        /// </summary>
        public string IsThirdParty { get; set; }

        /// <summary>
        ///Email是否验证,
        ///201911新UI改成手机验证
        /// </summary>
        public bool Isverify { get; set; }

        /// <summary>
        ///Email验证時間
        /// </summary>
        public DateTime? VerifyTime { get; set; }
        /// <summary>
        ///经销商Id
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        ///职员Id
        /// </summary>
        public int ClerkId { get; set; }

 
    }
}
