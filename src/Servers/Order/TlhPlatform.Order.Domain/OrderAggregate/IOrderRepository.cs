using System;
using System.Threading.Tasks;

namespace TlhPlatform.Order.Domain.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<string> GetMessage();
        Task<string> Create(Order order);

    }

}
