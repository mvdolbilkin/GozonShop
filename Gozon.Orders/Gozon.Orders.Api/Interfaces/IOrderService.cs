using Gozon.Orders.Api.Domain;

namespace Gozon.Orders.Api.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Guid userId, decimal amount);
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(Guid orderId);
    Task MarkAsPaidAsync(Guid orderId);
    Task MarkAsFailedAsync(Guid orderId, string reason);
}
