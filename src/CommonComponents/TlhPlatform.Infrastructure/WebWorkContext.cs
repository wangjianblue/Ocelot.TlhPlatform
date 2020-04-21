using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using TlhPlatform.Core.Domain.Customers;
using TlhPlatform.Core.Domain.Vendors;
using TlhPlatform.Core.Http;

namespace TlhPlatform.Infrastructure
{
    public class WebWorkContext
    {
        #region Fields 
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Customer _cachedCustomer;
        #endregion

        #region Ctor

        public WebWorkContext(
            IAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
        } 
        #endregion 
        #region Utilities

        /// <summary>
        /// Get nop customer cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetCustomerCookie()
        {
            var cookieName = $"{TlhPlatformCookieDefaults.Prefix}{TlhPlatformCookieDefaults.CustomerCookie}";
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }

        /// <summary>
        /// Set nop customer cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            var cookieName = $"{TlhPlatformCookieDefaults.Prefix}{TlhPlatformCookieDefaults.CustomerCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 100;
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (customerGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate,
                //Secure = _webHelper.IsCurrentConnectionSecured()
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, customerGuid.ToString(), options);
        }



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual Customer CurrentCustomer
        {
            get
            {
            
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                Customer customer = null;
                return _cachedCustomer;
            }
            set
            {
                SetCustomerCookie(value.CustomerGuid);
                _cachedCustomer = value;
            }
        } 
        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
