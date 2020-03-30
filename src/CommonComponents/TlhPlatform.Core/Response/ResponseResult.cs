using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Response
{
    /// <summary>
    /// api 响应实体
    /// </summary>
    public class ResponseResult<T> where T : class
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public int Status { get; set; } = (int)StatusEnum.Success;

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; } = null;

        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; } = null;

        /// <summary>
        /// 返回响应码状态描述
        /// </summary>
        /// <param name="code">返回响应码状态枚举</param>
        /// <param name="message">返回消息</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseResult<T> GenFaildResponse(StatusEnum code = StatusEnum.BusinessError, string message = "", dynamic data = null)
        {
            return new ResponseResult<T>
            {
                Status = (int)code,
                Data = data,
                Message = message
            };
        }
        /// <summary>
        /// 返回响应码状态描述
        /// </summary>
        /// <param name="code">返回响应码状态枚举</param>
        /// <param name="message">返回消息</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseResult<T> GenSuccessResponse(StatusEnum code = StatusEnum.Success, string message = "", dynamic data = null)
        {
            return new ResponseResult<T>
            {
                Status = (int)code,
                Data = data,
                Message = message
            };
        }
    }
}
