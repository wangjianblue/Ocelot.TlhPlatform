using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TlhPlatform.Core.Reflection.Dependency;
using TlhPlatform.Product.Domain.Entity;

namespace TlhPlatform.Product.Application.Interfaces
{
    public interface ITodoItemService:ITransientDependency
    { 

        Task<TodoItem> GetByIdAsync(long id);
    }
}
