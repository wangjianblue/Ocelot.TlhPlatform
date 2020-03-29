﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Product.Domain.TodoI
{
    public class TodoItem
    {
        //主键
        public long Id { get; set; }
        //待办事项名称
        public string Name { get; set; }
        //是否完成
        public bool IsComplete { get; set; }
    }
}