using Lys.NetCore.Infrastructure.Entities;
using System;

namespace Lys.NetCore.EF
{
    public abstract class MyRepository<T> : MyRepository<T, Guid> where T : class, IEntity<Guid>
    {
        protected MyRepository(MyDbContext context) : base(context)
        {
        }
    }

    public abstract class MyRepository<T, K> : Repository<T, K> where T : class, IEntity<K>
    {
        protected MyRepository(MyDbContext context) : base(context)
        {
        }
    }
}
