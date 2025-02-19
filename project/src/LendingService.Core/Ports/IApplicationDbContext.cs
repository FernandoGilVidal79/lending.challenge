using LendingService.Core.Models;

namespace LendingService.Core.Ports
{
    public interface IApplicationDbContext
    {
        public IEntitySet<Loan> LoanSet { get; set; }
    }
}
