using Gozon.Payments.Api.Domain;

namespace Gozon.Payments.Api.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByOrderIdAsync(Guid orderId);
    Task AddAsync(Payment payment);
}
