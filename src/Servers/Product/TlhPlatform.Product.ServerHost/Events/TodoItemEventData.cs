using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TlhPlatform.Core.Event;
using TlhPlatform.Product.Domain.Entity;

namespace TlhPlatform.Product.ServerHost.Events
{
    public class TodoItemEventData: EventData
    {
        public TodoItem TodoItem { get; set; }
        public TodoItemEventData(TodoItem todoItem)
        {
            TodoItem = todoItem;
        }
    }
}
