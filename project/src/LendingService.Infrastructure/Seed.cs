using LendingService.Core.Models;
using LendingService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Tests
{
    public static class Seed
    {

        public static List<Loan> GetMockLoans()
        {
            return new List<Loan>()
            {
                new Loan(){ BalanceLeft= 10, DueDate = DateTime.Now.AddDays(5), Msisdn = "string", Offer = GetOffer()},

            };
        }

        


        public static Offer GetOffer()
        {
            return new Offer() 
            { 
                Balance= 7, 
                Taxes = 0.2m 
            };     
        }

        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.Loans.AddRange(GetMockLoans());
            context.SaveChanges();
            return context;
        }
        public static void Destroy(ApplicationDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}
