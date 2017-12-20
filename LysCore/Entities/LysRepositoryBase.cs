using LysCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LysCore.Entities
{
    public class Repository<T, K> : IRepository<T, K> where T : class, IEntity<K>
    {
        protected readonly DbContext m_DbContext;

        public Repository(DbContext context)
        {
            m_DbContext = context;
        }

        public async Task<T> GetAsync(K id)
        {
            return await m_DbContext.Set<T>().FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            m_DbContext.Set<T>().Add(entity);
            await m_DbContext.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(T entity)
        {
            m_DbContext.Set<T>().Update(entity);
            await m_DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(K id)
        {
            var entity = await GetAsync(id);
            m_DbContext.Set<T>().Remove(entity);
            await m_DbContext.SaveChangesAsync();
        }
    }

    public class LysRepositoryBase<T> : Repository<T, Guid> where T : class, IEntity<Guid>
    {
        public LysRepositoryBase(DbContext context) : base(context) { }
    }
}
