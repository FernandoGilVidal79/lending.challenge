using LendingService.Core.Ports;

namespace LendingService.Core.Models;

public class Loan : BaseEntity<int>
{

    public string Msisdn { get; set; } = string.Empty;
    public decimal BalanceLeft { get; set; }
    public DateTime DueDate { get; set; }
    public Offer Offer { get; set; } = null!;
    public bool IsActive => BalanceLeft > 0 && DueDate > DateTime.UtcNow;
}