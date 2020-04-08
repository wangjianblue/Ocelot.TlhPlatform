using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace TlhPlatform.Infrastructure.Exceptionless
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerHelper _loggerHelper;
        //构造函数注入ILoggerHelper
        public GlobalExceptionFilter(ILoggerHelper loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }

        public void OnException(ExceptionContext filterContext)
        {
            //_loggerHelper.Error(filterContext.Exception.TargetSite.GetType().FullName, filterContext.Exception.ToString(), MpcKeys.GlobalExceptionCommonTags, filterContext.Exception.GetType().FullName);
            //var result = new PageResult()
            //{ 
                 
            //    StatusCode = ResultCodeAddMsgKeys.CommonExceptionCode,//系统异常代码
            //     Page = ResultCodeAddMsgKeys.CommonExceptionMsg,//系统异常信息
            //};
            //filterContext.Result = new ObjectResult(result);
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.ExceptionHandled = true;
        }
    }
}
