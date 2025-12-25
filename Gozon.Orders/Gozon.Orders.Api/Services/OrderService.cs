using Gozon.Orders.Api.Domain;
using Gozon.Orders.Api.Interfaces;
using Gozon.Shared.Events;
using MassTransit;

namespace Gozon.Orders.Api.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Order> CreateOrderAsync(Guid userId, decimal amount)
    {
        var order = new Order(Guid.NewGuid(), userId, amount);
        await _orderRepository.AddAsync(order);

        await _publishEndpoint.Publish(new OrderCreatedEvent
        {
            CorrelationId = Guid.NewGuid(),
            OrderId = order.Id,
            UserId = order.UserId,
            Amount = order.Amount
        });

        await _orderRepository.SaveChangesAsync();
        _logger.LogInformation("Order {OrderId} created for User {UserId}", order.Id, order.UserId);

        return order;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderAsync(Guid orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task MarkAsPaidAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", orderId);
            return;
        }

        try 
        {
            order.MarkAsPaid();
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} status updated to Paid", orderId);
        }
        catch (InvalidOperationException ex)
        {
             _logger.LogInformation(ex.Message);
        }
    }

    public async Task MarkAsFailedAsync(Guid orderId, string reason)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", orderId);
            return;
        }

        order.MarkAsFailed();
        await _orderRepository.SaveChangesAsync();
        _logger.LogInformation("Order {OrderId} status updated to Failed. Reason: {Reason}", orderId, reason);
    }
}
