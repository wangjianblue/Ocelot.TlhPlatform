using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Core.Dependency;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Event.Factories;
using TlhPlatform.Core.Reflection;

namespace TlhPlatform.Core.Events.Bus.Factories.Internals
{
    /// <summary>
    /// 依赖注入事件处理器实例获取方式
    /// </summary>
    internal class IocEventHandlerFactory : IEventHandlerFactory
    {
        private readonly Type _handlerType;

        /// <summary>
        /// 初始化一个<see cref="IocEventHandlerFactory"/>类型的新实例
        /// </summary>
        /// <param name="handlerType">事件处理器类型</param>
        public IocEventHandlerFactory(Type handlerType)
        {
            _handlerType = handlerType;
        }

        /// <summary>
        /// 获取事件处理器实例
        /// </summary>
        /// <returns></returns>
        public IEventHandler GetHandler()
        {
            return _handlerType as IEventHandler;
        }

        public Type GetHandlerType()
        {
            return _handlerType;
           // return ProxyHelper.UnProxy(_handlerType).GetType();
        }

        /// <summary>
        /// 释放事件处理器实例
        /// </summary>
        /// <param name="handler"></param>
        public void ReleaseHandler(IEventHandler handler)
        { }
    }
}
