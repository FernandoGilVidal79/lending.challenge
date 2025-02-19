using LendingService.Core.Ports;

namespace LendingService.Core.Models;

public class Offer : BaseEntity<Guid>
{
    public Offer(Guid id) : base(id)
    {
    }

    public decimal Balance { get; set; }
    public decimal Taxes { get; set; }
}