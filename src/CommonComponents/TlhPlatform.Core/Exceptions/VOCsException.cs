using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TlhPlatform.Core.Exceptions
{
    /// <summary>
    /// 表示在应用程序执行期间发生的错误
    /// </summary>
    [Serializable]
    public class VOCsException : Exception
    {
        /// <summary>
        /// 异常代码
        /// </summary>
        public String ErrorCode { get; set; }
        /// <summary>
        /// 初始化Exception类的新实例。
        /// </summary>
        public VOCsException()
        {
            ErrorCode = "001";
        }

        /// <summary>
        ///使用指定的错误消息初始化Exception类的新实例。
        /// </ summary>
        /// <param name =“message”>描述错误的消息。</ param>
        public VOCsException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///使用指定的错误消息初始化Exception类的新实例。
        /// </ summary>
        /// <param name =“messageFormat”>异常消息格式。</ param>
        /// <param name =“args”>异常消息参数。</ param>
        public VOCsException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        ///使用序列化数据初始化Exception类的新实例。
        /// </ summary>
        /// <param name =“info”> SerializationInfo，用于保存有关抛出异常的序列化对象数据。</ param>
        /// <param name =“context”> StreamingContext，包含有关源或目标的上下文信息。</ param>
        protected VOCsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///使用指定的错误消息初始化Exception类的新实例，并引用导致此异常的内部异常。
        /// </ summary>
        /// <param name =“message”>解释异常原因的错误消息。</ param>
        /// <param name =“innerException”>导致当前异常的异常，如果没有指定内部异常则为空引用。</ param>
        public VOCsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
