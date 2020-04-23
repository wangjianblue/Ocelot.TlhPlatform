using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TlhPlatform.Core.Event;

namespace TlhPlatform.Product.ServerHost.Events
{
    public class TodoItemEventSmsHandler : IAsyncEventHandler<TodoItemEventData>, IEventHandler<TodoItemEventData>
    {
        public async Task HandleEventAsync(TodoItemEventData eventData)
        {
            await Task.Run(() => { return; });

            Console.WriteLine(typeof(TodoItemEventSmsHandler));
        }

        public void HandleEvent(TodoItemEventData eventData)
        {
            Console.WriteLine(eventData.ToString());
        }
    }
}
