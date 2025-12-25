namespace Gozon.Shared.Events;

public record PaymentSucceededEvent
{
    public Guid CorrelationId { get; init; }
    public Guid OrderId { get; init; }
}

public record PaymentFailedEvent
{
    public Guid CorrelationId { get; init; }
    public Guid OrderId { get; init; }
    public required string Reason { get; init; }
}
