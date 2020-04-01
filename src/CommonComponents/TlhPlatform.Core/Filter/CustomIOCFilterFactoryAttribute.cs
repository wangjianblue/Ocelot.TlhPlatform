using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TlhPlatform.Core.Filter
{
    /// <summary>
    /// 自己重写IFilterFactory
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class CustomIocFilterFactoryAttribute:Attribute,IFilterFactory
    {
        private readonly Type _filterType = null;

        public CustomIocFilterFactoryAttribute(Type filterType)
        {
            _filterType = filterType;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return (IFilterMetadata)serviceProvider.GetService(this._filterType);
        }

        public bool IsReusable => true;
    }
}
