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
                new Loan(new Guid("9e0fd50a-d636-43d4-b47c-decd0ecb483c")){ BalanceLeft= 10, DueDate = DateTime.Now.AddDays(5), Msisdn = "string", Offer = GetOffer()},

            };
        }

        public static Offer GetOffer()
        {
            return new Offer(new Guid("b4cd3ffd-1261-44ba-a4b8-3c6228ea5b43"));
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
