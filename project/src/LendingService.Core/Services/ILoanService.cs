using LendingService.Core.Dtos;
using LendingService.Core.Models;

namespace LendingService.Core.Services;

public interface ILoanService
{
    Task<IEnumerable<Offer>> AddOrUpdateOffersAsync(IEnumerable<Offer> offers);
    Task<Loan?> GetActiveLoanAsync(string msisdn);
    Task<Loan> CreateLoanAsync(string msisdn, int offerId);
    Task<ProcessRepayment> ProcessRepaymentAsync(string msisdn, decimal topUpAmount);
    Task<bool> CustomerExistsAsync(string msisdn);
}