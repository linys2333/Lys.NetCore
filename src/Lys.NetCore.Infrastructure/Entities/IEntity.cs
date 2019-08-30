using System;

namespace Lys.NetCore.Infrastructure.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IEntityBase<T> : IEntity<T>
    {
        T CreatorId { get; set; }

        DateTime Created { get; set; }
        
        T UpdaterId { get; set; }

        DateTime Updated { get; set; }
    }
}