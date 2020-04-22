using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using TlhPlatform.Core.Event;
using TlhPlatform.Core.Filter;
using TlhPlatform.Core.Response;
using TlhPlatform.Infrastructure.AutoMapper;
using TlhPlatform.Product.Application;
using TlhPlatform.Product.Application.Interfaces;
using TlhPlatform.Product.Domain.Dto;
using TlhPlatform.Product.Domain.Entity;
using TlhPlatform.Product.Domain.Mime;
using TlhPlatform.Product.Repository;
using TlhPlatform.Product.ServerHost.Configs.Cache;
using TlhPlatform.Product.ServerHost.Events;
using TlhPlatform.Product.ServerHost.Filter;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TlhPlatform.Product.ServerHost.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]//添加特性，代表是一个Web API控制器类
    public class TodoController : Controller
    {

        public readonly TlhPlatform.Infrastructure.Cache.Key.IKeyManager KeyManager;
        public readonly ITodoItemService _TodoItemService;
        private readonly IHttpClientFactory _httpClient;
        private readonly IMessageService _messageService;

        /// <summary>
        /// 实例化一个EF上下文，进行数据库操作。开始初始入库一条数据
        /// </summary>
        /// <param name="keyManager"></param>
        /// <param name="todoItemService"></param>
        /// <param name="httpClient"></param>
        public TodoController(ITodoItemService todoItemService, TlhPlatform.Infrastructure.Cache.Key.IKeyManager keyManager, IHttpClientFactory httpClient, IMessageService messageService)
        {
            _TodoItemService = todoItemService;
            KeyManager = keyManager;
            _httpClient = httpClient;
            _messageService = messageService;
        }
        /// <summary>
        /// 获取所有事项
        /// GET: api/Todo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [TypeFilter(typeof(MyActionFilterAttribute), Arguments = new object[] { "HAHA" }, IsReusable = true)]
        //[CustomIocFilterFactory(typeof(MyActionFilterAttribute))]
        public async Task<TodoItem> GetTodoItems()
        {
            var todoItem = new TodoItem()
            {
                Name = "张三",
            };
            //EventBusCommon.Trigger(new TodoItemEventData(todoItem));

            var todoItemDto = todoItem.ToModel<TodoItemDto>();

            var key = KeyManager.Get(name: ProductKey.Admin_User_Session);

            await _TodoItemService.GetByIdAsync(1);


            _messageService.SendEmail(null, null, new EmailMessage()
            {
                Subject = "主题",
                Body = String.Format("1212")
            });


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
