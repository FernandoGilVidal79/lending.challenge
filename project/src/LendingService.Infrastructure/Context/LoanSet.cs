using LendingService.Core.Models;
using LendingService.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Context
{
    public  class LoanSet : IEntitySet<Loan>
    {
        private readonly DbSet<Loan> _loanDbSet;

        public LoanSet(DbSet<Loan> loanDbSet)
        {
            _loanDbSet = loanDbSet;
        }

        public void AddRange(List<Loan> loans)
        {
            _loanDbSet.AddRange(loans);
        }

        public void Add(Loan loan)
        {
            _loanDbSet.Add(loan);
        }

        public void Update(Loan loan)
        {
            _loanDbSet.Update(loan); 
        }

        public void Remove(Loan loan)
        {
            _loanDbSet.Remove(loan);
        }

        public List<Loan> ToList() 
        {
            return _loanDbSet.ToList();
        }

        Loan IEntitySet<Loan>.Find<IBaseEntity>(int id)
        {
            return _loanDbSet.Find(id);
        }

        Loan IEntitySet<Loan>.GetBy<IBaseEntity>(string msisdn)
        {
            return _loanDbSet.Where(x => x.Msisdn.Equals(msisdn)).Include(x => x.Offer).FirstOrDefault();
        }

    }
}
