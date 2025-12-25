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
}
