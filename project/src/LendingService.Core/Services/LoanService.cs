using System.Collections.Concurrent;
using LendingService.Core.Models;

namespace LendingService.Core.Services;

public class LoanService : ILoanService
{
    private readonly ConcurrentDictionary<Guid, Offer> _offers = new();
    private readonly ConcurrentDictionary<string, Loan> _activeLoans = new();
    private readonly HashSet<string> _knownCustomers = new();

    public async Task<IEnumerable<Offer>> AddOrUpdateOffersAsync(IEnumerable<Offer> offers)
    {
        foreach (var offer in offers)
        {
            _offers.AddOrUpdate(offer.Id, offer, (_, _) => offer);
        }
        return await Task.FromResult(offers);
    }

    public Task<Loan?> GetActiveLoanAsync(string msisdn)
    {
        _activeLoans.TryGetValue(msisdn, out var loan);
        return Task.FromResult(loan?.IsActive == true ? loan : null);
    }

    public async Task<Loan> CreateLoanAsync(string msisdn, Guid offerId)
    {
        if (!_offers.TryGetValue(offerId, out var offer))
        {
            throw new KeyNotFoundException("Offer not found");
        }

        var existingLoan = await GetActiveLoanAsync(msisdn);
        if (existingLoan != null)
        {
            throw new InvalidOperationException("Customer already has an active loan");
        }

        var loan = new Loan(Guid.NewGuid()) // In production, use a proper ID generation strategy
        {
           
            Msisdn = msisdn,
            BalanceLeft = offer.Balance * (1 + offer.Taxes),
            DueDate = DateTime.UtcNow.AddDays(30), // Example: 30-day loan term
            Offer = offer
        };

        _activeLoans.TryAdd(msisdn, loan);
        _knownCustomers.Add(msisdn);
        return loan;
    }

    public async Task<decimal> ProcessRepaymentAsync(string msisdn, decimal topUpAmount)
    {
        var loan = await GetActiveLoanAsync(msisdn);
        if (loan == null)
        {
            return 0;
        }

        var repaymentAmount = Math.Min(topUpAmount, loan.BalanceLeft);
        loan.BalanceLeft -= repaymentAmount;

        if (loan.BalanceLeft <= 0)
        {
            _activeLoans.TryRemove(msisdn, out _);
        }

        return repaymentAmount;
    }
    public Task<bool> CustomerExistsAsync(string msisdn)
    {
        return Task.FromResult(_knownCustomers.Contains(msisdn));
    }

}