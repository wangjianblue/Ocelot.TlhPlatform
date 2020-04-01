using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using TlhPlatform.Infrastructure.Cache.Key;
using TlhPlatform.Product.ServerHost.Configs.Cache;

namespace TlhPlatform.Product.ServerHost.Filter
{
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IKeyManager _keyManager;
        private readonly ILogger<MyActionFilterAttribute> _logger;

        public MyActionFilterAttribute(IKeyManager keyManager,string arg1, ILogger<MyActionFilterAttribute> logger)
        {
            _keyManager = keyManager;
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("执行"+typeof(MyActionFilterAttribute));
            _keyManager.Get(ProductKey.Admin_User_Session,new []{"myAction"});
            //context.HttpContext.Response.WriteAsync("abc");
        }
    }
}
