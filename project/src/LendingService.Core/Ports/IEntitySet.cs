namespace LendingService.Core.Ports
{
    public interface IEntitySet<T>
    {
        void AddRange(List<T> getDeals);
        void Add(T deal);
        void Remove(T deal);
        public T Find<IBaseEntity>(int id);
      //  public List<IBaseEntity> ToList<T>();
    }
}
