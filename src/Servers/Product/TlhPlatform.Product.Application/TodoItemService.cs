using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TlhPlatform.Infrastructure.AOP;
using TlhPlatform.Infrastructure.Extents;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.Entity;
using TlhPlatform.Product.Domain.Repository;
using TlhPlatform.Product.Repository;

namespace TlhPlatform.Product.Application
{

    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ITodoItemRepository1 _todoItemRepository1;

        public TodoItemService(ITodoItemRepository todoItemRepository,
            ITodoItemRepository1 todoItemRepository1)
        {
            _todoItemRepository = todoItemRepository;
            _todoItemRepository1 = todoItemRepository1;
        }
        [CustomInterceptor]
        public async Task<TodoItem> GetByIdAsync(long id)
        {
            return await _todoItemRepository.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(long id)
        {
           return await _todoItemRepository1.AddAsync();
        }
    }
}
