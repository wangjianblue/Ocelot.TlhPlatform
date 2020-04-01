using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Filter;
using TlhPlatform.Core.Response;
using TlhPlatform.Product.Application;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.TodoI;
using TlhPlatform.Product.Repository;
using TlhPlatform.Product.ServerHost.Configs.Cache;
using TlhPlatform.Product.ServerHost.Events;
using TlhPlatform.Product.ServerHost.Filter;
using WebApplication1.Models.Cache;
using IKeyManager = TlhPlatform.Infrastructure.Cache.Key.IKeyManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TlhPlatform.Product.ServerHost.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]//添加特性，代表是一个Web API控制器类
    public class TodoController : Controller
    {

        public readonly Infrastructure.Cache.Key.IKeyManager KeyManager;
        public readonly ITodoItemService _TodoItemService;

        /// <summary>
        /// 实例化一个EF上下文，进行数据库操作。开始初始入库一条数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="keyManager"></param>
        /// <param name="todoItemService"></param>

        public TodoController(ITodoItemService todoItemService, IKeyManager keyManager)
        {
            _TodoItemService = todoItemService;
            KeyManager = keyManager;
        }
        /// <summary>
        /// 获取所有事项
        /// GET: api/Todo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(MyActionFilterAttribute), Arguments = new object[] { "HAHA" },IsReusable = true)]
        //[CustomIocFilterFactory(typeof(MyActionFilterAttribute))]
        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            var todoItem = new TodoItem()
            {
                Name = "张三",
            };
            //EventBusCommon.Trigger(new TodoItemEventData(todoItem));


            var key = KeyManager.Get(name: ProductKey.Admin_User_Session);

            // return await TodoItemService.GeTodoItems();

            return null;
        }

        /// <summary>
        /// 删除一个TodoItem
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///  DELETE: api/Todo/2
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>不返回内容</returns>
        /// <response code="204">删除成功，不返回内容</response>
        /// <response code="404">删除失败，未找到该记录</response>
        [HttpDelete("{id}", Name = "DeleteTodoItem")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ResponseResult<TodoItem>> DeleteTodoItem(long id)
        {
            var result = await _TodoItemService.GetByIdAsync(id);
            if (result == null)
            {
                return ResponseResult<TodoItem>.GenFaildResponse();
            }
            return ResponseResult<TodoItem>.GenSuccessResponse(data: result);

        }
        ///// <summary>
        ///// 根据id，获取一条事项
        ///// GET: api/Todo/5。  id 是参数，代表路由合并
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        /////[HttpGet("{id}")]
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
