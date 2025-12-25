namespace Gozon.Payments.Api.Dto;

public class TopUpRequestDto
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
