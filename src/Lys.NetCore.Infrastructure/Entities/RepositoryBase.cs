using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lys.NetCore.Infrastructure.Entities
{
    public abstract class Repository<T, K> : IRepository<T, K> where T : class, IEntity<K>
    {
        protected DbContext m_DbContext;

        protected Repository(DbContext context)
        {
            m_DbContext = context;
        }

        public virtual Task<T> GetAsync(K id)
        {
            return m_DbContext.Set<T>().FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual Task<List<T>> GetListAsync()
        {
            return m_DbContext.Set<T>().ToListAsync();
        }

        public virtual Task CreateAsync(T entity)
        {
            m_DbContext.Set<T>().Add(entity);
            return m_DbContext.SaveChangesAsync();
        }

        public virtual Task CreateAsync(IEnumerable<T> entitys)
        {
            m_DbContext.Set<T>().AddRange(entitys);
            return m_DbContext.SaveChangesAsync();
        }

        public virtual Task UpdateAsync(T entity)
        {
            m_DbContext.Set<T>().Update(entity);
            return m_DbContext.SaveChangesAsync();
        }

        public virtual Task UpdateAsync(IEnumerable<T> entitys)
        {
            m_DbContext.Set<T>().UpdateRange(entitys);
            return m_DbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(K id)
        {
            var entity = await GetAsync(id);
            m_DbContext.Set<T>().Remove(entity);
            await m_DbContext.SaveChangesAsync();
        }

        public virtual Task DeleteAsync(IEnumerable<T> entitys)
        {
            m_DbContext.Set<T>().RemoveRange(entitys);
            return m_DbContext.SaveChangesAsync();
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return m_DbContext.Set<T>().Where(filter);
        }

        public virtual IQueryable<T> WhereIf(Expression<Func<T, bool>> filter, bool condition)
        {
            return condition ? m_DbContext.Set<T>().Where(filter) : m_DbContext.Set<T>();
        }

        public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return m_DbContext.Set<T>().FirstOrDefaultAsync(filter);
        }
    }
}
