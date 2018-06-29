using LysCore.Entities;
using LysCore.Interfaces;
using System;

namespace EF
{
    public abstract class MyRepository<T> : LysRepositoryBase<T> where T : class, IEntity<Guid>
    {
        protected new readonly MyDbContext m_DbContext;
        
        protected MyRepository(MyDbContext context) : base(context)
        {
            m_DbContext = context;
        }
    }
}
