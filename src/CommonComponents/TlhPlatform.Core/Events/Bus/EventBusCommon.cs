using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Event.Entities;
using TlhPlatform.Core.Event.Factories;
using TlhPlatform.Core.Event.Handlers;

namespace TlhPlatform.Core.Event
{
    public class EventBusCommon
    {
        /// <summary>
        /// 以Transient方式注册事件（生命周期：瞬时）
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        public static void RegisterTransientEvent<TEventData, THandler>()
            where TEventData : IEventData
            where THandler : IEventHandler, new()
        {
            EventBus.Default.Register<TEventData, THandler>();
        }
        /// <summary>
        /// 以Single方式注册事件（生命周期：单例）
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public static void RegisterSingleEvent(Type eventType, IEventHandler handler)
        {
            EventBus.Default.Register(eventType, handler);
        }


        public static void RegisterSingleEvent(Type eventType, IEventHandlerFactory handler)
        { 
            EventBus.Default.Register(eventType, handler);
        }
 

        public static void UnRegisterEvent(Type eventType, IEventHandlerFactory handler)
        {
            EventBus.Default.Unregister(eventType, handler);
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        public static void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            EventBus.Default.Trigger(eventData);
        }
    }
}
