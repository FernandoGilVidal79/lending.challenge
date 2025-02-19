using LendingService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Tests
{
    public class ApplicationDbContextTest
    {
        [Fact]
        public void CanInsertLoanIntoDatabase()
        {
            using var context = Seed.Create();
            var loan = new Loan(Guid.NewGuid());
            context.Loans.Add(loan);
            Assert.Equal(EntityState.Added, context.Entry(loan).State);
            var result = context.SaveChangesAsync();
            Assert.Equal(1, result.Result);
            Assert.Equal(Task.CompletedTask.Status, result.Status);
            Assert.Equal(EntityState.Unchanged, context.Entry(loan).State);
        }
    }
}