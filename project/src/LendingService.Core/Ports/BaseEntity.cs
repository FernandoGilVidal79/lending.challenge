using System.ComponentModel.DataAnnotations;

namespace LendingService.Core.Ports
{
    public class BaseEntity<TKey> : IBaseEntity
    {
        public BaseEntity(TKey id)
        {
            Id = id;
        }


        [Key]
        public TKey Id { get; set; }
    }
}
