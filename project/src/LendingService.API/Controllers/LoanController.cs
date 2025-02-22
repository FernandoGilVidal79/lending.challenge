using Microsoft.AspNetCore.Mvc;
using LendingService.Core.Services;
using LendingService.Core.Models;

namespace LendingService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet("/status")]
    public IActionResult GetStatus()
    {
        return Ok();
    }

    [HttpPost("/offers")]
    public async Task<IActionResult> AddOffers([FromBody] List<Offer> offers)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _loanService.AddOrUpdateOffersAsync(offers);
        return Ok(result);
    }

    [HttpPost("/customers/{msisdn}/loans")]
    public async Task<IActionResult> CreateLoan(string msisdn, [FromForm] int id)
    {
        try
        {
            var loan = await _loanService.CreateLoanAsync(msisdn, id);
            return Ok(new
            {
                loan.Id,
                loan.BalanceLeft,
                loan.DueDate
            });
        }
        catch (KeyNotFoundException)
        {
            return BadRequest("Offer not found");
        }
        catch (InvalidOperationException)
        {
            return Conflict("Customer already has an active loan");
        }
    }

    [HttpPut("/customers/{msisdn}/loans")]
    public async Task<IActionResult> ProcessRepayment(string msisdn, [FromForm] decimal topUp)
    {
        // First check if customer exists by trying to get their loan
        var loan = await _loanService.GetActiveLoanAsync(msisdn);

        // If no loan found and customer has never had a loan, return 404
        if (loan == null && !await _loanService.CustomerExistsAsync(msisdn))
        {
            return NotFound("Customer not found");
        }

        // If customer exists but has no active loan, return 204
        if (loan == null)
        {
            return NoContent();
        }

        var repaidAmount = await _loanService.ProcessRepaymentAsync(msisdn, topUp);
        return Ok(new { Repaid = repaidAmount });
    }

    [HttpGet("/customers/{msisdn}/loans")]
    public async Task<IActionResult> GetActiveLoan(string msisdn)
    {
        if (!await _loanService.CustomerExistsAsync(msisdn))
        {
            return NotFound("Customer not found");
        }

        var loan = await _loanService.GetActiveLoanAsync(msisdn);
        if (loan == null)
        {
            return NoContent();
        }

        return Ok(new
        {
            loan.BalanceLeft,
            loan.DueDate,
            Offer = new
            {
                loan.Offer.Balance,
                loan.Offer.Taxes
            }
        });
    }
}