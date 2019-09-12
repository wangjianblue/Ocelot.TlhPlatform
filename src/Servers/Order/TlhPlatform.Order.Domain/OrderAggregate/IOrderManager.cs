using System.Threading.Tasks;

namespace TlhPlatform.Order.Domain.OrderAggregate
{
    public interface IOrderManager
    {
        Task<string> GetMessage();

        Task<string> Create(Order input);
    }
}