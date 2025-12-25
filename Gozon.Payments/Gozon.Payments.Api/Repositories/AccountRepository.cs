using Gozon.Payments.Api.Data;
using Gozon.Payments.Api.Domain;
using Gozon.Payments.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gozon.Payments.Api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly PaymentsDbContext _dbContext;

    public AccountRepository(PaymentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account?> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }
}
