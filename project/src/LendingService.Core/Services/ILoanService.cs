using LendingService.Core.Models;

namespace LendingService.Core.Services;

public interface ILoanService
{
    Task<IEnumerable<Offer>> AddOrUpdateOffersAsync(IEnumerable<Offer> offers);
    Task<Loan?> GetActiveLoanAsync(string msisdn);
    Task<Loan> CreateLoanAsync(string msisdn, int offerId);
    Task<decimal> ProcessRepaymentAsync(string msisdn, decimal topUpAmount);
    Task<bool> CustomerExistsAsync(string msisdn);
}