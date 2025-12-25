using Gozon.Shared.Enums;

namespace Gozon.Orders.Api.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Order() { }

    public Order(Guid id, Guid userId, decimal amount)
    {
        Id = id;
        UserId = userId;
        Amount = amount;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Order is already paid.");
            
        Status = OrderStatus.Paid;
    }

    public void MarkAsFailed()
    {
        Status = OrderStatus.Failed;
    }
}
