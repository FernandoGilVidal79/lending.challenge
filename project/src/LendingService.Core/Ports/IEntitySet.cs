namespace LendingService.Core.Ports
{
    public interface IEntitySet<T>
    {
        void AddRange(List<T> entities);
        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);
        public T Find<IBaseEntity>(int id);
      //  public List<IBaseEntity> ToList<T>();
    }
}
