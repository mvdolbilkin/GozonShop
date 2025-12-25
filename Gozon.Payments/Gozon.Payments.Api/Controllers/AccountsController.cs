using Gozon.Payments.Api.Dto;
using Gozon.Payments.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gozon.Payments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(Guid userId)
    {
        var account = await _accountService.CreateAccountAsync(userId);
        return Ok(account);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBalance(Guid userId)
    {
        var account = await _accountService.GetAccountAsync(userId);
        return account != null ? Ok(new { Balance = account.Balance }) : NotFound();
    }

    [HttpPost("topup")]
    public async Task<IActionResult> TopUp([FromBody] TopUpRequestDto request)
    {
        var account = await _accountService.TopUpAsync(request.UserId, request.Amount);
        if (account == null)
        {
            return NotFound("Account not found");
        }

        return Ok(new { Balance = account.Balance });
    }
}
