using LysCore.Common;
using LysCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace LysCore.Entities
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

        protected Entity()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }

    public class LysEntityBase : Entity<Guid>
    {
        public LysEntityBase()
        {
            CreatorId = Guid.Empty;
            UpdaterId = Guid.Empty;
            Id = SequentialGuid.NewGuid();
        }
    }
}