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
        public IEntitySet<Loan> LoanSet { get; set; }

        public DbSet<Loan> DbLoans { get; set; }

        private IEntitySet<Loan> _loans;


        public IEntitySet<Loan> Loans
        {
            get { return _loans ??= new LoanSet(Set<Loan>()); }
            set => _loans = value;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
