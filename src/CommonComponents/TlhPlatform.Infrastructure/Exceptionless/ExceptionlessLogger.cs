using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Exceptionless;

namespace TlhPlatform.Infrastructure.Exceptionless
{
    /// <summary>
    /// Gary
    /// 2019年9月10日10:24:41
    /// Exceptionless日志实现
    /// </summary>
    public class ExceptionlessLogger : ILoggerHelper
    {
        /// <summary>
        /// 记录trace日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">添加标记</param>
        public void Trace(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Trace.ToString()).AddTags(args).Submit();

            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Trace.ToString());
            }
        }
        /// <summary>
        /// 记录debug信息
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Debug(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Debug.ToString()).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Debug.ToString());
            }
        }
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Info(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Info.ToString()).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Info.ToString());
            }
        }
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Warn(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Warn.ToString()).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Warn.ToString());
            }
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Error(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Err.ToString()).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Err.ToString());
            }
        }
    }
}
