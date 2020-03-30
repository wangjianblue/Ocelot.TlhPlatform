using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Reflection
{
    public static class ProxyHelper
    {
        /// <summary>
        /// Returns dynamic proxy target object if this is a proxied object, otherwise returns the given object. 
        /// </summary>
        public static object UnProxy(object obj)
        {
            return ProxyUtil.GetUnproxiedInstance(obj);
        }
    }
}
