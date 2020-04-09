using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Infrastructure.AutoMapper;


namespace TlhPlatform.Product.Domain.Dto
{
   public class TodoItemDto: IBaseDtoModel
    { 

        //主键
        public  long Id { get; set; }
        //待办事项名称
        public string Name { get; set; }
   
    }
}
