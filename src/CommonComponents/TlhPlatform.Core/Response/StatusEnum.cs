using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Response
{
    public enum StatusEnum
    {
        /// <summary>
        /// 門店出貨
        /// </summary>
        Success = 200,

        /// <summary>
        /// 业务错误码 
        /// </summary>
        BusinessError = 400,

        /// <summary>
        /// 
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 系统异常
        /// </summary>
        SystemException = 500,

    }
}
