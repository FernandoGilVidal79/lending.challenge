using LendingService.Core.Dtos;
using LendingService.Core.Models;
using LendingService.Core.Ports;

namespace LendingService.Core.Services;

public class LoanService : ILoanService
{
    private readonly static HashSet<string> _knownCustomers = new();
    private readonly IApplicationDbContext _dbContext;
    public LoanService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Status()
    {
        try
        {
            var loans = _dbContext.Loans.ToList();
            foreach (var loan in loans)
            {
                _knownCustomers.Add(loan.Msisdn);
            }
        }

        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<Offer>> AddOrUpdateOffersAsync(IEnumerable<Offer> offers)
    {
        foreach (var offer in offers)
        {
            Offer offerUpdate = _dbContext.Offers.Find<Offer>(offer.Id);
            if (offerUpdate == null)
            {
                offerUpdate = new Offer();
                offerUpdate.Balance = offer.Balance;
                offerUpdate.Taxes = offer.Taxes;
                _dbContext.Offers.Add(offerUpdate);      
            }
            else
            {
                offerUpdate.Balance = offer.Balance;
                offerUpdate.Taxes = offer.Taxes;
                _dbContext.Offers.Update(offerUpdate);
            }
            await _dbContext.SaveChangesAsync();          
        }
        return await Task.FromResult(offers);
    }

    public Task<Loan> GetActiveLoanAsync(string msisdn)
    {
        var loan = _dbContext.Loans.GetBy<Loan>(msisdn); 
        return Task.FromResult(loan?.IsActive == true ? loan : null);
    }

    public async Task<Loan> CreateLoanAsync(string msisdn, int offerId)
    {
        var offer = _dbContext.Offers.Find<Loan>(offerId);
        if (offer == null)
        {
            throw new KeyNotFoundException("Offer not found");
        }

        var existingLoan = await GetActiveLoanAsync(msisdn);
        if (existingLoan != null)
        {
            throw new InvalidOperationException("Customer already has an active loan");
        }

        var loan = new Loan()
        {
            Msisdn = msisdn,
            BalanceLeft = offer.Balance * (1 + offer.Taxes),
            DueDate = DateTime.UtcNow.AddDays(30), // Example: 30-day loan term
            Offer = offer
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();
        _knownCustomers.Add(msisdn);
        return loan;
    }

    public async Task<ProcessRepaymentDto> ProcessRepaymentAsync(string msisdn, decimal topUpAmount)
    {
        var loan = await GetActiveLoanAsync(msisdn);
        if (loan == null)
        {
            throw new KeyNotFoundException("No encontrado");
        }

        var repaymentAmount = Math.Min(topUpAmount, loan.BalanceLeft);
        loan.BalanceLeft -= repaymentAmount;

        if (loan.BalanceLeft <= 0)
        {
            _dbContext.Loans.Remove(loan);
            await _dbContext.SaveChangesAsync();
        }

        return new ProcessRepaymentDto()
        {
            BalaceLeft = loan.BalanceLeft,
            DueDate = DateTime.UtcNow,
            Id = loan.Id,
        };
    }
    public Task<bool> CustomerExistsAsync(string msisdn)
    {
        return Task.FromResult(_knownCustomers.Contains(msisdn));
    }
}