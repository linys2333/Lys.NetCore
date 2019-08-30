using Lys.NetCore.Infrastructure.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lys.NetCore.Infrastructure.Entities
{
    [Serializable]
    public class Entity<T> : IEntity<T>
    {
        [Required]
        public T Id { get; set; }

        [Required]
        public T CreatorId { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public T UpdaterId { get; set; }

        public DateTime Updated { get; set; }
    }

    public class EntityBase : Entity<Guid>
    {
        public EntityBase()
        {
            CreatorId = Guid.Empty;
            UpdaterId = Guid.Empty;
            Id = SequentialGuid.NewGuid();
        }
    }
}