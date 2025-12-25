using Gozon.Orders.Api.Interfaces;
using Gozon.Shared.Events;
using MassTransit;

namespace Gozon.Orders.Api.Consumers;

public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
{
    private readonly IOrderService _orderService;

    public PaymentFailedConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var message = context.Message;
        await _orderService.MarkAsFailedAsync(message.OrderId, message.Reason);
    }
}
