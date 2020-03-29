﻿using System;
using System.Collections.Generic;
using System.Text;
using SmartSql.DyRepository;
using TlhPlatform.Product.Domain.TodoI;

namespace TlhPlatform.Product.Repository
{
    public interface ITodoItemRepository: IRepository<TodoItem, long>,
        IRepositoryAsync<TodoItem, long>
    {
    }
}