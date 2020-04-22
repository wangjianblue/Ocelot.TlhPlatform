using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using SmartSql.DyRepository;
using TlhPlatform.Core.Dapper;
using TlhPlatform.Core.Extensions;
using TlhPlatform.Product.Domain.Entity;
using TlhPlatform.Product.Domain.Repository;
namespace TlhPlatform.Product.Repository
{
    public interface ITodoItemRepository : IRepository<TodoItem, long>,
        IRepositoryAsync<TodoItem, long>
    {
    }

    public class TodoItemRepository : ITodoItemRepository1
    {
        public async Task<bool> DeleteAllAsync()
        {
            var sql = "select * from TodoItem";
            var result = await DbContext.ExecuteAsync(sql);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(TodoItem entity)
        {
            var sql = "select * from TodoItem";
            var result = await DbContext.ExecuteAsync(sql);
            return result > 0;
        }

        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            var sql = "select * from TodoItem";
            return await DbContext.QueryAsync<TodoItem>(sql);

        }

        public async Task<TodoItem> GetAsync(object id)
        {
            var sql = "select * from TodoItem";
            return await DbContext.QueryFirstOrDefaultAsync<TodoItem>(sql);

        }

        public async Task<PagedResult<TodoItem>> GetPageListAsync(int pageIndex, int pageSize, object param = null)
        {
            var page = new PagedResult<TodoItem>();

            var sql = "select * from TodoItem;";
            var result = await DbContext.QueryAsync<TodoItem>(sql);
            page.Data = result;
            page.Paged=new Paged(){PageIndex = pageIndex,PageSize = pageSize,TotalRow=10};
            return page;
        }
        public async Task<long> InsertAsync(TodoItem entity)
        {
            /*
             *
             *
            //查 无参数
            var list = DapperHelper<T_User>.Query("select * from T_User ").ToList();
            //查 带参数
            var list = DapperHelper<T_User>.Query("select * from T_User where uid=@uid", new { uid = 1, }).ToList();
            //增
            int ins = DapperHelper<T_User>.Execute("insert into T_User (uid,username) value(@uid,@username)", new { uid = 1, username = "张三" });
            //改
            int upd = DapperHelper<T_User>.Execute("update T_User set username=@username where uid=@uid", new { username = "李四", uid = 1});
            //删
            int del = DapperHelper<T_User>.Execute("delete from T_User where uid=@uid", new { uid = 1 }); 
             *
             */
            var sql = "insert into TodoItem values(@id,@name)";
            return await DbContext.ExecuteAsync(sql, new { id = entity.Id, name = entity.Name });
        }

        public async Task QueryAsync(object param = null,
            CommandType? commandType = null)
        {

            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(TodoItem entity)
        {
            var sql = "select * from TodoItem";
            var result = await DbContext.ExecuteAsync(sql);
            return result > 0;
        }


    }

}
