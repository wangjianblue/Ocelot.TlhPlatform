using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TlhPlatform.Infrastructure.AOP;
using TlhPlatform.Infrastructure.Extents;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.Entity;
using TlhPlatform.Product.Repository;

namespace TlhPlatform.Product.Application
{

    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemService(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        [CustomInterceptor]
        public async Task<TodoItem> GetByIdAsync(long id)
        {
            return await _todoItemRepository.GetByIdAsync(id);
        }
    }
}
