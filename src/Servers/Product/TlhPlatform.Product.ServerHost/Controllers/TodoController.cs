using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TlhPlatform.Product.Domain.TodoI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TlhPlatform.Product.ServerHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]//添加特性，代表是一个Web API控制器类
    public class TodoController : Controller
    {
        private IEnumerable<TodoItem> TodoItems = null;

        /// <summary>
        /// 实例化一个EF上下文，进行数据库操作。开始初始入库一条数据
        /// </summary>
        /// <param name="context"></param>
        public TodoController()
        {
            TodoItems = new List<TodoItem>()
            {
                 new TodoItem(){Id=1,IsComplete = true,Name = "张三"},
                 new TodoItem(){Id=2,IsComplete = true,Name = "李四"},
                 new TodoItem(){Id=3,IsComplete = false,Name = "王五"},
                 new TodoItem(){Id=4,IsComplete = false,Name = "赵柳"},
                 new TodoItem(){Id=5,IsComplete = true,Name = "哦哦"}
            };
        }
        /// <summary>
        /// 获取所有事项
        /// GET: api/Todo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await Task.Run(() => TodoItems.ToList()) ;
        }


        /// <summary>
        /// 根据id，获取一条事项
        /// GET: api/Todo/5。  id 是参数，代表路由合并
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///[HttpGet("{id}")]
        //public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        //{

        //    var todoItem = await TodoItems.ToList();
        //    if (todoItem == null)
        //    {
        //        return NotFound();
        //    }
        //    return todoItem;
        //}
    }
}
