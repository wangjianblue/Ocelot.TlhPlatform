using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TlhPlatform.Product.ServerHost.Filter
{
    /// <summary>
    /// 优先级1：权限过滤器：它在Filter Pipleline中首先运行，并用于决定当前用户是否有请求权限。如果没有请求权限直接返回。
    /// </summary>
    public class MyAuthorization : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
