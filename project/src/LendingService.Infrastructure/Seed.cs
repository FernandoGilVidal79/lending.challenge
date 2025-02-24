using LendingService.Core.Models;
using LendingService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Tests
{
    /// <summary>
    /// Mock Class to emulate real Database Data
    /// </summary>
    public static class Seed
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.SaveChanges();
            return context;
        }

        public static List<Loan> GetMockLoans()
        {
            return new List<Loan>()
            {
                new Loan() { BalanceLeft = 10, DueDate = DateTime.Now.AddDays(5), Msisdn = "688777333", Offer = GetOffer()},
            };
        }

        public static List<Loan> GetMockMultipleLoans()
        {
            var listLoan = new List<Loan>();
            for (int i = 0; i < 1000; i++)
            {
                var loan = new Loan() { BalanceLeft = 10, DueDate = DateTime.Now.AddDays(5), Msisdn = (688776333 + i).ToString(), Offer = GetOffer() };
          
                listLoan.Add(loan);
            }

            return listLoan;  
        }



        public static Offer GetOffer()
        {
            return new Offer() 
            { 
                Balance= 7, 
                Taxes = 0.2m 
            };     
        }

        public static void Destroy(ApplicationDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}
