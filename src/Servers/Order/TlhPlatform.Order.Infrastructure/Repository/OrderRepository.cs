using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TlhPlatform.Order.Domain;
using TlhPlatform.Order.Domain.OrderAggregate;
namespace TlhPlatform.Order.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {   
        public async Task<string> Create(Domain.OrderAggregate.Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetMessage()
        {
            return await Task.Run(() => "OrderRepository" );
        }
    }
}
