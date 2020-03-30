using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TlhPlatform.Core.Dependency;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Event.Factories;
using TlhPlatform.Core.Events.Bus.Factories.Internals;
using TlhPlatform.Core.Reflection;

namespace TlhPlatform.Core.Events.Bus
{
    public class EventsModule
    { 
      
        public void FindEventsTypes()
        {
            var typeFinder = ServiceLocator.Instance.GetService<ITypeFinder>();
      

            var consumers = typeFinder.FindClassesOfType(typeof(IEventHandler<>)).ToList();
            foreach (var consumer in consumers)
            {
                //builder.RegisterType(consumer)
                //    .As(consumer.FindInterfaces((type, criteria) =>
                //    {
                //        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                //        return isMatch;
                //    }, typeof(IEventHandler<>)))
                //    .InstancePerLifetimeScope();
                var ls = consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType &&
                                  ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IEventHandler<>));
                IEventHandlerFactory factory = new IocEventHandlerFactory(ls[0]);

                EventBusCommon.RegisterSingleEvent(typeof(IEventHandler<>), factory);
            }
        }
    }
}
