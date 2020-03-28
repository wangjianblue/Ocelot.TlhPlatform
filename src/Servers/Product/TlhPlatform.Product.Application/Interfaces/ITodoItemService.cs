using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TlhPlatform.Product.Domain.TodoI;

namespace TlhPlatform.Product.Application.Interfaces
{
    public interface ITodoItemService
    { 

        Task<TodoItem> GetByIdAsync(long id);
    }
}
