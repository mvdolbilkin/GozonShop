using Gozon.Orders.Api.Interfaces;
using Gozon.Shared.Events;
using MassTransit;

namespace Gozon.Orders.Api.Consumers;

public class PaymentSucceededConsumer : IConsumer<PaymentSucceededEvent>
{
    private readonly IOrderService _orderService;

    public PaymentSucceededConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
    {
        var message = context.Message;
        await _orderService.MarkAsPaidAsync(message.OrderId);
    }
}
