using Gozon.Payments.Api.Data;
using Gozon.Payments.Api.Domain;
using Gozon.Payments.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gozon.Payments.Api.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentsDbContext _dbContext;

    public PaymentRepository(PaymentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task AddAsync(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
    }
}
