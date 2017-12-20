using System;

namespace LysCore.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }

        T CreatorId { get; set; }

        DateTime Created { get; set; }
        
        T UpdaterId { get; set; }

        DateTime Updated { get; set; }
    }
}