using System.ComponentModel.DataAnnotations;

namespace Gozon.Payments.Api.Domain;

public class Account
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }

    [Timestamp]
    public uint Version { get; set; }

    public void Credit(decimal amount)
    {
        if (amount < 0) throw new ArgumentException("Amount must be positive");
        Balance += amount;
    }

    public bool TryDebit(decimal amount)
    {
        if (amount < 0) throw new ArgumentException("Amount must be positive");
        if (Balance < amount) return false;
        
        Balance -= amount;
        return true;
    }
}
