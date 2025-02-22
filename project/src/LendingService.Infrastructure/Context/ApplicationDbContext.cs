using LendingService.Core.Models;
using LendingService.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        public ApplicationDbContext()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
     
        public DbSet<Loan> DbLoans { get; set; }

        public DbSet<Offer> DbOffer { get; set; }

        private IEntitySet<Loan> _loans;

        private IEntitySet<Offer> _offers;


        public IEntitySet<Loan> Loans
        {
            get { return _loans ??= new LoanSet(Set<Loan>()); }
            set => _loans = value;
        }

        public IEntitySet<Offer> Offers
        {
            get { return _offers ??= new OfferSet(Set<Offer>()); }
            set => _offers = value;
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
