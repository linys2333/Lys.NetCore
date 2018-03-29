using LysCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Task<T> GetAsync(K id)
        {
            return m_DbContext.Set<T>().FindAsync(id);
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter)
        {
            return m_DbContext.Set<T>().Where(filter).ToListAsync();
        }

        public Task<List<T>> GetListAsync()
        {
            return m_DbContext.Set<T>().ToListAsync();
        }

        public Task CreateAsync(T entity)
        {
            m_DbContext.Set<T>().Add(entity);
            return m_DbContext.SaveChangesAsync();
        }

        public Task CreateAsync(IEnumerable<T> entitys)
        {
            m_DbContext.Set<T>().AddRange(entitys);
            return m_DbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            m_DbContext.Set<T>().Update(entity);
            return m_DbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(IEnumerable<T> entitys)
        {
            m_DbContext.Set<T>().UpdateRange(entitys);
            return m_DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(K id)
        {
            var entity = await GetAsync(id);
            m_DbContext.Set<T>().Remove(entity);
            await m_DbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(IEnumerable<T> entitys)
        {
            m_DbContext.Set<T>().RemoveRange(entitys);
            return m_DbContext.SaveChangesAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return m_DbContext.Set<T>().Where(filter);
        }
    }

    public class LysRepositoryBase<T> : Repository<T, Guid> where T : class, IEntity<Guid>
    {
        public LysRepositoryBase(DbContext context) : base(context) { }
    }
}
