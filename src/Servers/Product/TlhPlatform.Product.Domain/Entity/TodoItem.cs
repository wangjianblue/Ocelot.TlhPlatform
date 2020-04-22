using TlhPlatform.Infrastructure.AutoMapper;

namespace TlhPlatform.Product.Domain.Entity
{
    public class TodoItem: IBaseEntity
    {
        //主键
        public  long Id { get; set; }
        //待办事项名称
        public string Name { get; set; }
        //是否完成
        public bool IsComplete { get; set; }
    }
}
