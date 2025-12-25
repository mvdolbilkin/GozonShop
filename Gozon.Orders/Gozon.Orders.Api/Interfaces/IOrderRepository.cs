using Gozon.Orders.Api.Domain;

namespace Gozon.Orders.Api.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<List<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}
