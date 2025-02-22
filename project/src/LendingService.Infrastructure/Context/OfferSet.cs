using LendingService.Core.Models;
using LendingService.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Context
{
    public  class OfferSet : IEntitySet<Offer>
    {
        private readonly DbSet<Offer> _offerDbSet;

        public OfferSet(DbSet<Offer> offerDbSet)
        {
            _offerDbSet = offerDbSet;
        }

        public void AddRange(List<Offer> offers)
        {
            _offerDbSet.AddRange(offers);
        }

        public void Add(Offer offer)
        {
            _offerDbSet.Add(offer);
        }

        public void Update(Offer offer)
        {
            _offerDbSet.Update(offer);
        }

        public void Remove(Offer offer)
        {
            _offerDbSet.Remove(offer);
        }

        public T Find<T>(int id) where T : Offer
        {
            return _offerDbSet.Find(id) as T;
        }

        public List<T> ToList<T>() where T : Offer
        {
            return _offerDbSet.ToList() as List<T>;
        }

        Offer IEntitySet<Offer>.Find<IBaseEntity>(int id)
        {
            return _offerDbSet.Find(id);
        }

        public Offer GetBy<IBaseEntity>(string msisdn)
        {
            throw new NotImplementedException();
        }


        //List<IBaseEntity> IEntitySet<Loan>.ToList<T>()
        //{
        //    return _loanDbSet.ToList();
        //}
    }
}
