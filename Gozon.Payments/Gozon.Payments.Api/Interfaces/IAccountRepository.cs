using Gozon.Payments.Api.Domain;

namespace Gozon.Payments.Api.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByUserIdAsync(Guid userId);
    Task AddAsync(Account account);
    Task SaveChangesAsync();
    Task UpdateAsync(Account account);
}
