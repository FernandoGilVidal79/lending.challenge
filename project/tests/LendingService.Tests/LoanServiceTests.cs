using System.Runtime.CompilerServices;
using LendingService.Core.Models;
using LendingService.Core.Services;
using Xunit;

namespace LendingService.Tests;

public class LoanServiceTests
{
    private readonly ILoanService _service;
    private readonly List<Offer> _testOffers;
    private const string TestMsisdn = "688777333";

    private Guid Guid_Offer1 = new Guid("fe7a0f0d-3008-43b6-8bc0-72a053011f66");
    private Guid Guid_Offer2 = new Guid("3b8893aa-3e83-4b9a-86c6-07ae8ff4173f");
    public LoanServiceTests()
    {
        _service = new LoanService();
        _testOffers = new List<Offer>
        {
            new(Guid_Offer1) {Balance = 7, Taxes = 0.2m },
            new(Guid_Offer2) {Balance = 10, Taxes = 0.7m }
        };
    }

    [Fact]
    public async Task AddOrUpdateOffers_ShouldAddNewOffers()
    {
        var result = await _service.AddOrUpdateOffersAsync(_testOffers);
        Assert.Equal(_testOffers.Count, result.Count());
    }

    [Fact]
    public async Task CreateLoan_ShouldCreateNewLoan()
    {
        await _service.AddOrUpdateOffersAsync(_testOffers);
        var loan = await _service.CreateLoanAsync(TestMsisdn, Guid_Offer1);

        Assert.NotNull(loan);
        Assert.Equal(TestMsisdn, loan.Msisdn);
        Assert.Equal(8.4m, loan.BalanceLeft); // 7 * (1 + 0.2)
    }

    [Fact]
    public async Task CreateLoan_ShouldThrowWhenOfferNotFound()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.CreateLoanAsync(TestMsisdn, Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateLoan_ShouldThrowWhenActiveLoanExists()
    {
        await _service.AddOrUpdateOffersAsync(_testOffers);
        await _service.CreateLoanAsync(TestMsisdn, Guid_Offer1);

        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.CreateLoanAsync(TestMsisdn, Guid_Offer1));
    }

    [Fact]
    public async Task ProcessRepayment_ShouldRepayCorrectAmount()
    {
        await _service.AddOrUpdateOffersAsync(_testOffers);
        await _service.CreateLoanAsync(TestMsisdn, Guid_Offer1);

        var repaid = await _service.ProcessRepaymentAsync(TestMsisdn, 5m);
        Assert.Equal(5m, repaid);

        var loan = await _service.GetActiveLoanAsync(TestMsisdn);
        Assert.Equal(3.4m, loan?.BalanceLeft);
    }

    [Fact]
    public async Task ProcessRepayment_ShouldFullyRepayLoan()
    {
        await _service.AddOrUpdateOffersAsync(_testOffers);
        await _service.CreateLoanAsync(TestMsisdn, Guid_Offer1);

        var repaid = await _service.ProcessRepaymentAsync(TestMsisdn, 10m);
        Assert.Equal(8.4m, repaid);

        var loan = await _service.GetActiveLoanAsync(TestMsisdn);
        Assert.Null(loan);
    }

    [Fact]
    public async Task GetActiveLoan_ShouldReturnNullWhenNoLoan()
    {
        var loan = await _service.GetActiveLoanAsync(TestMsisdn);
        Assert.Null(loan);
    }

    [Fact]
    public async Task GetActiveLoan_ShouldReturnLoanWhenActive()
    {
        await _service.AddOrUpdateOffersAsync(_testOffers);
        await _service.CreateLoanAsync(TestMsisdn, Guid_Offer1);

        var loan = await _service.GetActiveLoanAsync(TestMsisdn);
        Assert.NotNull(loan);
        Assert.True(loan.IsActive);
    }
}