using Gozon.Orders.Api.Data;
using Gozon.Orders.Api.Domain;
using Gozon.Orders.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gozon.Orders.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _dbContext;

    public OrderRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _dbContext.Orders.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
