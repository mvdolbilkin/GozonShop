using Gozon.Payments.Api.Interfaces;
using Gozon.Shared.Events;
using MassTransit;

namespace Gozon.Payments.Api.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IAccountService _accountService;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(IAccountService accountService, ILogger<OrderCreatedConsumer> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Processing payment for Order {OrderId}, User {UserId}", message.OrderId, message.UserId);

        await _accountService.ProcessPaymentAsync(message.OrderId, message.UserId, message.Amount, context);
    }
}
