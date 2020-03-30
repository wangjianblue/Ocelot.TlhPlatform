using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TlhPlatform.Product.ServerHost.Filter
{
    /// <summary>
    /// 优先级5：结果过滤器：它可以在执行Action结果之前执行，且执行Action成功后执行，使用逻辑必须围绕view或格式化执行结果。
    /// </summary>
    public class MyResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    } 
}
