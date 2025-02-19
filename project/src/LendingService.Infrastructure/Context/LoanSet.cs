using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendingService.Core.Models;
using LendingService.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Context
{
    public  class LoanSet : IEntitySet<Loan>
    {
        private readonly DbSet<Loan> _loanDbSet;

        public LoanSet(DbSet<Loan> dealDbSet)
        {
            _loanDbSet = dealDbSet;
        }

        public void AddRange(List<Loan> deals)
        {
            _loanDbSet.AddRange(deals);
        }

        public void Add(Loan deal)
        {
            _loanDbSet.Add(deal);
        }

        public void Remove(Loan deal)
        {
            _loanDbSet.Remove(deal);
        }

        public T Find<T>(int id) where T : Loan
        {
            return _loanDbSet.Find(id) as T;
        }

        public List<T> ToList<T>() where T : Loan
        {
            return _loanDbSet.ToList() as List<T>;
        }

        Loan IEntitySet<Loan>.Find<IBaseEntity>(int id)
        {
            return _loanDbSet.Find(id);
        }

        //List<IBaseEntity> IEntitySet<Loan>.ToList<T>()
        //{
        //    return _loanDbSet.ToList();
        //}
    }
}
