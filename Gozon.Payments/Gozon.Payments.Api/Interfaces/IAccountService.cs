using Gozon.Payments.Api.Domain;
using MassTransit;

namespace Gozon.Payments.Api.Interfaces;

public interface IAccountService
{
    Task<Account> CreateAccountAsync(Guid userId);
    Task<Account?> GetAccountAsync(Guid userId);
    Task<Account?> TopUpAsync(Guid userId, decimal amount);
    Task ProcessPaymentAsync(Guid orderId, Guid userId, decimal amount, ConsumeContext context);
}
