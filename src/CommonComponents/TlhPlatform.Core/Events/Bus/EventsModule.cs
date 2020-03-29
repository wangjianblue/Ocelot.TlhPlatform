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
        private IAllAssemblyFinder _AllAssemblyFinder { get; set; }
        public EventsModule()
        {
            _AllAssemblyFinder = new AppDomainAllAssemblyFinder(); ;
        }
        public void FindAutoMapTypes()
        {
            var Assembly = _AllAssemblyFinder.FindAll(true).SelectMany(p => p.GetTypes());
            var baseTypes = Assembly.Where(p => typeof(IEventHandler).IsAssignableFrom(p) && p.IsClass == true && p.IsPublic == true);
            foreach (var item in baseTypes)
            {
                var EventDatas = item.GetInterfaces();

                var EventDataName = EventDatas[0].GenericTypeArguments[0];

                 IEventHandlerFactory factory = new IocEventHandlerFactory(item);
                //Type type = item.GetType();
                //var EventHandler = type.Assembly.CreateInstance(item.Name) as IEventHandler;

                //EventBusCommon.RegisterSingleEvent(EventDataName, factory);
            }
        }
    }
}
