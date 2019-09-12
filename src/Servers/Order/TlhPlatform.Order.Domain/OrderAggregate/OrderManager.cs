using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TlhPlatform.Order.Domain.OrderAggregate
{
    public class OrderManager:IOrderManager
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<string> GetMessage()
        {
          return await _orderRepository.GetMessage();
        }

        public async Task<string> Create(Order order)
        {
            return await _orderRepository.Create(order);
        }
    }
}
