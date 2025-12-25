using Gozon.Payments.Api.Domain;
using Gozon.Payments.Api.Interfaces;
using Gozon.Shared.Events;
using MassTransit;

namespace Gozon.Payments.Api.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<AccountService> _logger;

    public AccountService(IAccountRepository accountRepository, IPaymentRepository paymentRepository, ILogger<AccountService> logger)
    {
        _accountRepository = accountRepository;
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    public async Task<Account> CreateAccountAsync(Guid userId)
    {
        var account = new Account { Id = Guid.NewGuid(), UserId = userId, Balance = 0 };
        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> GetAccountAsync(Guid userId)
    {
        return await _accountRepository.GetByUserIdAsync(userId);
    }

    public async Task<Account?> TopUpAsync(Guid userId, decimal amount)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId);
        if (account == null) return null;

        account.Credit(amount);
        await _accountRepository.SaveChangesAsync();
        return account;
    }

    public async Task ProcessPaymentAsync(Guid orderId, Guid userId, decimal amount, ConsumeContext context)
    {
        const int maxRetries = 3;
        
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                var existingPayment = await _paymentRepository.GetByOrderIdAsync(orderId);
                if (existingPayment != null)
                {
                    _logger.LogInformation("Payment for Order {OrderId} already processed", orderId);
                    return;
                }

                var account = await _accountRepository.GetByUserIdAsync(userId);

                if (account == null)
                {
                    _logger.LogWarning("Account not found for user {UserId}", userId);
                    await context.Publish(new PaymentFailedEvent
                    {
                        CorrelationId = Guid.NewGuid(),
                        OrderId = orderId,
                        Reason = "Account not found"
                    });
                    await _accountRepository.SaveChangesAsync();
                    return;
                }

                if (!account.TryDebit(amount))
                {
                    _logger.LogWarning("Insufficient funds for user {UserId}. Balance: {Balance}, Required: {Amount}", 
                        userId, account.Balance, amount);
                    await context.Publish(new PaymentFailedEvent
                    {
                        CorrelationId = Guid.NewGuid(),
                        OrderId = orderId,
                        Reason = "Insufficient funds"
                    });
                    await _accountRepository.SaveChangesAsync();
                    return;
                }

                await context.Publish(new PaymentSucceededEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    OrderId = orderId
                });
                
                await _paymentRepository.AddAsync(new Payment
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    Amount = amount,
                    ProcessedAt = DateTime.UtcNow
                });

                await _accountRepository.SaveChangesAsync();
                _logger.LogInformation("Payment successful for Order {OrderId} on attempt {Attempt}", orderId, attempt + 1);
                return;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict for Order {OrderId}, attempt {Attempt} of {MaxRetries}", 
                    orderId, attempt + 1, maxRetries);
                
                if (attempt == maxRetries - 1)
                {
                    _logger.LogError("Failed to process payment for Order {OrderId} after {MaxRetries} attempts", 
                        orderId, maxRetries);
                    await context.Publish(new PaymentFailedEvent
                    {
                        CorrelationId = Guid.NewGuid(),
                        OrderId = orderId,
                        Reason = "Concurrency conflict - please retry"
                    });
                    await _accountRepository.SaveChangesAsync();
                    throw;
                }
                
                await Task.Delay(TimeSpan.FromMilliseconds(100 * (attempt + 1)));
            }
        }
    }

    public async Task<bool> DebitAsync(Guid userId, decimal amount)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId);
        if (account == null) return false;

        if (!account.TryDebit(amount)) return false;

        await _accountRepository.UpdateAsync(account);
        return true;
    }
}
