using LendingService.Core.Models;

namespace LendingService.Core.Ports
{
    public interface IApplicationDbContext
    {
        public IEntitySet<Loan> Loans { get; set; }

        public IEntitySet<Offer> Offers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
