using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TlhPlatform.Core.Response;

namespace TlhPlatform.Product.ServerHost.Filter
{
    /// <summary>
    /// 优先级4：异常过滤器：被应用全局策略处理未处理的异常发生前异常被写入响应体
    /// </summary>
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _moprovider;
        public MyExceptionFilterAttribute(IModelMetadataProvider moprovider)
        {
            this._moprovider = moprovider;
        }
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (!context.ExceptionHandled)//如果异常没有被处理过
            {
                string controllerName = (string)context.RouteData.Values["controller"];
                string actionName = (string)context.RouteData.Values["action"];
                //string msgTemplate =string.Format( "在执行controller[{0}的{1}]方法时产生异常",controllerName,actionName);//写入日志

                if (this.IsAjaxRequest(context.HttpContext.Request))
                {

                    context.Result = new JsonResult(ResponseResult<object>.GenFaildResponse(message: context.Exception.Message
                        , data: "系统出现异常，请联系管理员"));
                }
                else
                {
                    var result = new ViewResult { ViewName = "~Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(_moprovider, context.ModelState);
                    result.ViewData.Add("Execption", context.Exception);
                    context.Result = result;
                }
            }
        }
        //判断是否为ajax请求
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }
}
