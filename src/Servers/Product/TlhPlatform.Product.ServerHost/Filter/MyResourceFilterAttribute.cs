using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TlhPlatform.Product.ServerHost.Filter
{
    /// <summary>
    /// 优先级2：资源过滤器： 它在Authorzation后面运行，同时在后面的其它过滤器完成后还会执行。Resource filters 实现缓存或其它性能原因返回。因为它运行在模型绑定前，所以这里的操作都会影响模型绑定。
    /// </summary>
    public class MyResourceFilterAttribute : IResourceFilter
    {
        //这个ResourceFiltersAttribute是最适合做缓存了,在这里做缓存有什么好处?因为这个OnResourceExecuting是在控制器实例化之前运营，如果能再这里获取ViewReuslt就不必实例化控制器，在走一次视图了，提升性能
        private static readonly Dictionary<string, object> _Cache = new Dictionary<string, object>();
        private string _cacheKey;

        /// <summary>
        /// 这个方法会在控制器实例化之前之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString(); //这个是请求地址，它肯定是指向的视图
            if (_Cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _Cache[_cacheKey] as ViewResult;
                if (cachedValue != null)
                {
                    context.Result = cachedValue;
                }
            }
        }

        /// <summary>
        /// 这个方法是是Action的OnResultExecuted过滤器执行完之后在执行的（每次执行完Action之后得到就是一个ViewResult）
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString(); //这个是请求地址
            if (!string.IsNullOrEmpty(_cacheKey) && !_Cache.ContainsKey(_cacheKey))
            {
                //因为这个方法是是Action的OnResultExecuted过滤器执行完之后在执行的，所以context.Result必然有值了，这个值就是Action方法执行后得到的ViewResult
                var result = context.Result as ViewResult;
                if (result != null)
                {
                    _Cache.Add(_cacheKey, result);
                }
            }
        }
    }
}
