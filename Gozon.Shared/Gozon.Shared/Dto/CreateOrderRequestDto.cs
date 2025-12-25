namespace Gozon.Shared.Dto;

public class CreateOrderRequestDto
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public List<OrderItemDto>? Items { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
