using LendingService.Core.Ports;

namespace LendingService.Core.Models;

public class Offer : BaseEntity<int>
{
    public decimal Balance { get; set; }
    public decimal Taxes { get; set; }
}