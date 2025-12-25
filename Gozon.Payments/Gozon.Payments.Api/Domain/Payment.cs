using System;

namespace Gozon.Payments.Api.Domain;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
}
