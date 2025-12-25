using Gozon.Orders.Api.Interfaces;
using Gozon.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Gozon.Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto request)
    {
        var order = await _orderService.CreateOrderAsync(request.UserId, request.Amount);
        return Ok(new { OrderId = order.Id });
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders.Select(o => new {
            o.Id,
            o.UserId,
            o.Amount,
            Status = o.Status.ToString(),
            o.CreatedAt
        }));
    }

    [HttpGet("{orderId}/status")]
    public async Task<IActionResult> GetOrderStatus(Guid orderId)
    {
        var order = await _orderService.GetOrderAsync(orderId);
        if (order == null)
            return NotFound();

        return Ok(new { Status = order.Status.ToString() });
    }
}
